using FilesManager.Core.Helpers;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.UI.Desktop.UnitTests._TestHelpers;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Text.RegularExpressions;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Strategies
{
    [TestFixture]
    internal sealed class LeadingZerosViewModelTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public void GetNewFilePath_ForGivenInput_ReturnsExpectedFileName((int Id, string[] TestNames, string LeadingZeros, string[] ExpectedPaths) data)
        {
            // Arrange
            StrategyBase<PathZerosDigitsExtensionDto> strategy = new LeadingZerosViewModel
            {
                LeadingZeros = data.LeadingZeros,
                MaxDigitLength = data.TestNames.Select(name =>
                    #pragma warning disable SYSLIB1045  // Do not convert to 'GeneratedRegexAttribute'
                    Regex.Match(name, @"(0*)(?<Digits>[0-9]+)?").Groups["Digits"].Value)
                    #pragma warning restore SYSLIB1045
                    .GetMaxLength()
            };

            // Act
            string[] actualPaths = data.TestNames.Select(name =>
                strategy.GetNewFilePath(
                    TestHelpers.GetMockedDto(@"C:\Drive\Folder\Subfolder\", name, ".jpg")))
                    .ToArray();

            // Assert
            Assert.That(
                string.Join(", ", actualPaths), 
                Is.EqualTo(string.Join(", ", data.ExpectedPaths)),
                $"Test id: {data.Id}");
        }

        private static IEnumerable<(int Id, string[] TestNames, string LeadingZeros, string[] ExpectedPaths)> GetTestCases()
        {
            // Invalid zeros
            yield return (1, new[] { "1", "02" }, "",   new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (2, new[] { "1", "02" }, " ",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (3, new[] { "1", "02" }, "a",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (4, new[] { "1", "02" }, "Z",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (5, new[] { "1", "02" }, "*",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (6, new[] { "1", "02" }, "-1", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (7, new[] { "1", "02" }, "8",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });

            // No zeros: only digits
            yield return (8,  new[] { "1",   "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (9,  new[] { "01",  "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (10, new[] { "012", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\12.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            // No zeros: digits + name
            yield return (11, new[] { "1a",     "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1a.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (12, new[] { "01a",    "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1a.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (13, new[] { "012a",   "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\12a.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (14, new[] { "012abc", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\12abc.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            // No zeros: name
            yield return (15, new[] { "0a", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\a.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            // No zeros: nothing else
            yield return (16, new[] { "00", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (17, new[] { "0",  "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });

            // Empty
            yield return (18, new[] { string.Empty, string.Empty }, "2", new[] { "00", "00" });
            yield return (19, new[] { string.Empty, string.Empty }, "1", new[] { "0", "0" });
            yield return (20, new[] { string.Empty, string.Empty }, "0", new[] { string.Empty, string.Empty });

            // Digits + 2 zeros
            yield return (21, new[] { "1",   "2"   }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (22, new[] { "1",   "02"  }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (23, new[] { "1",   "002" }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (24, new[] { "01",  "2"   }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (25, new[] { "01",  "02"  }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (26, new[] { "01",  "002" }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (27, new[] { "001", "2"   }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (28, new[] { "001", "02"  }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (29, new[] { "001", "002" }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            
            // Digits + 1 zero
            yield return (30, new[] { "1",   "2"   }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (31, new[] { "1",   "02"  }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (32, new[] { "1",   "002" }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (33, new[] { "01",  "2"   }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (34, new[] { "01",  "02"  }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (35, new[] { "01",  "002" }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (36, new[] { "001", "2"   }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (37, new[] { "001", "02"  }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (38, new[] { "001", "002" }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            
            // Digits without any zeros requested
            yield return (39, new[] { "1",   "2"   }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (40, new[] { "1",   "02"  }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (41, new[] { "1",   "002" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (42, new[] { "01",  "2"   }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (43, new[] { "01",  "02"  }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (44, new[] { "01",  "002" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (45, new[] { "001", "2"   }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (46, new[] { "001", "02"  }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (47, new[] { "001", "002" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });

            // Symmetric match with a lot of zeros
            yield return (48, new[] { "1", "00000002" }, "7", new[] { @"C:\Drive\Folder\Subfolder\00000001.jpg", @"C:\Drive\Folder\Subfolder\00000002.jpg" });
            yield return (49, new[] { "01", "0000002" }, "6", new[] { @"C:\Drive\Folder\Subfolder\0000001.jpg",  @"C:\Drive\Folder\Subfolder\0000002.jpg" });
            yield return (50, new[] { "001", "000002" }, "5", new[] { @"C:\Drive\Folder\Subfolder\000001.jpg",   @"C:\Drive\Folder\Subfolder\000002.jpg" });
            yield return (51, new[] { "0001", "00002" }, "4", new[] { @"C:\Drive\Folder\Subfolder\00001.jpg",    @"C:\Drive\Folder\Subfolder\00002.jpg" });
            yield return (52, new[] { "00001", "0002" }, "3", new[] { @"C:\Drive\Folder\Subfolder\0001.jpg",     @"C:\Drive\Folder\Subfolder\0002.jpg" });
            yield return (53, new[] { "000001", "002" }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg",      @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (54, new[] { "0000001", "02" }, "1", new[] { @"C:\Drive\Folder\Subfolder\01.jpg",       @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (55, new[] { "00000001", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg",        @"C:\Drive\Folder\Subfolder\2.jpg" });

            // Zeros + Digits + Name
            yield return (56, new[] { "01Test", "2Test15" }, "2", new[] { @"C:\Drive\Folder\Subfolder\001Test.jpg", @"C:\Drive\Folder\Subfolder\002Test15.jpg" });

            // Numbers
            yield return (57, new[] { "010", "002" }, "3", new[] { @"C:\Drive\Folder\Subfolder\00010.jpg", @"C:\Drive\Folder\Subfolder\00002.jpg" });
            yield return (58, new[] { "010", "002" }, "2", new[] { @"C:\Drive\Folder\Subfolder\0010.jpg",  @"C:\Drive\Folder\Subfolder\0002.jpg" });
            yield return (59, new[] { "010", "002" }, "1", new[] { @"C:\Drive\Folder\Subfolder\010.jpg",   @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (60, new[] { "010", "002" }, "0", new[] { @"C:\Drive\Folder\Subfolder\10.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
        }
    }
}
