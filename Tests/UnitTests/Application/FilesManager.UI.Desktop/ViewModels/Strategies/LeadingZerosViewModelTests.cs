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
        private StrategyBase<PathZerosDigitsExtensionDto>? _strategy;

        [TestCaseSource(nameof(GetAbsoluteTestCases))]
        [TestCaseSource(nameof(GetRelativeTestCases))]
        public void GetNewFilePath_ForGivenInput_ReturnsExpectedFileName(
            (int Id, string[] TestNames, byte LeadingZeros, bool IsAbsoluteModeOn, string[] ExpectedPaths) data)
        {
            // Arrange
            this._strategy = new LeadingZerosViewModel
            {
                LeadingZeros = data.LeadingZeros,
                IsAbsoluteModeOn = data.IsAbsoluteModeOn,
                MaxDigitsLength = data.TestNames.Select(name =>
                    #pragma warning disable SYSLIB1045  // Do not convert to 'GeneratedRegexAttribute'
                    Regex.Match(name, @"(0*)(?<Digits>[0-9]+)?").Groups["Digits"].Value)
                    #pragma warning restore SYSLIB1045
                    .GetMaxLength()
            };

            // Act
            string[] actualPaths = data.TestNames.Select(name =>
                this._strategy.GetNewFilePath(
                    TestHelpers.GetTestDto(@"C:\Drive\Folder\Subfolder\", name, ".jpg")))
                    .ToArray();

            // Assert
            Assert.That(
                string.Join(", ", actualPaths), 
                Is.EqualTo(string.Join(", ", data.ExpectedPaths)),
                $"Test: {data.Id} | Max digits length: {((LeadingZerosViewModel)this._strategy).MaxDigitsLength}");
        }

        private static IEnumerable<(int Id, string[] TestNames, byte LeadingZeros, bool IsAbsoluteModeOn, string[] ExpectedPaths)> GetAbsoluteTestCases()
        {
            // No zeros: only Digits
            yield return (1, new[] { "1",   "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (2, new[] { "01",  "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (3, new[] { "012", "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\12.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            
            // No zeros: Digits + Name
            yield return (4, new[] { "1a",     "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1a.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (5, new[] { "01a",    "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1a.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (6, new[] { "012a",   "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\12a.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (7, new[] { "012abc", "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\12abc.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            
            // No zeros: Name
            yield return (8, new[] { "0a", "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\a.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            
            // No zeros: nothing else
            yield return (9,  new[] { "000", "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (10, new[] { "00",  "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (11, new[] { "0",   "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });

            // Digits + 2 Zeros
            yield return (12, new[] { "1",   "2"   }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (13, new[] { "1",   "02"  }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (14, new[] { "1",   "002" }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (15, new[] { "01",  "2"   }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (16, new[] { "01",  "02"  }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (17, new[] { "01",  "002" }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (18, new[] { "001", "2"   }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (19, new[] { "001", "02"  }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (20, new[] { "001", "002" }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });

            // Digits + 1 Zero
            yield return (21, new[] { "1",   "2"   }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (22, new[] { "1",   "02"  }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (23, new[] { "1",   "002" }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (24, new[] { "01",  "2"   }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (25, new[] { "01",  "02"  }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (26, new[] { "01",  "002" }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (27, new[] { "001", "2"   }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (28, new[] { "001", "02"  }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (29, new[] { "001", "002" }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });

            // Digits without any Zeros requested
            yield return (30, new[] { "1",   "2"   }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (31, new[] { "1",   "02"  }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (32, new[] { "1",   "002" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (33, new[] { "01",  "2"   }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (34, new[] { "01",  "02"  }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (35, new[] { "01",  "002" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (36, new[] { "001", "2"   }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (37, new[] { "001", "02"  }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (38, new[] { "001", "002" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });

            // Symmetric match with a lot of Zeros
            yield return (38, new[] { "1", "00000002" }, 7, true, new[] { @"C:\Drive\Folder\Subfolder\00000001.jpg", @"C:\Drive\Folder\Subfolder\00000002.jpg" });
            yield return (39, new[] { "01", "0000002" }, 6, true, new[] { @"C:\Drive\Folder\Subfolder\0000001.jpg",  @"C:\Drive\Folder\Subfolder\0000002.jpg" });
            yield return (40, new[] { "001", "000002" }, 5, true, new[] { @"C:\Drive\Folder\Subfolder\000001.jpg",   @"C:\Drive\Folder\Subfolder\000002.jpg" });
            yield return (41, new[] { "0001", "00002" }, 4, true, new[] { @"C:\Drive\Folder\Subfolder\00001.jpg",    @"C:\Drive\Folder\Subfolder\00002.jpg" });
            yield return (42, new[] { "00001", "0002" }, 3, true, new[] { @"C:\Drive\Folder\Subfolder\0001.jpg",     @"C:\Drive\Folder\Subfolder\0002.jpg" });
            yield return (43, new[] { "000001", "002" }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001.jpg",      @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (44, new[] { "0000001", "02" }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\01.jpg",       @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (45, new[] { "00000001", "2" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",        @"C:\Drive\Folder\Subfolder\2.jpg" });

            // Zeros + Digits + Name
            yield return (46, new[] { "01Test", "2Test15" }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\001Test.jpg", @"C:\Drive\Folder\Subfolder\002Test15.jpg" });

            // Numbers
            yield return (47, new[] { "100", "002" }, 3, true, new[] { @"C:\Drive\Folder\Subfolder\000100.jpg", @"C:\Drive\Folder\Subfolder\000002.jpg" });
            yield return (48, new[] { "100", "002" }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\00100.jpg",  @"C:\Drive\Folder\Subfolder\00002.jpg" });
            yield return (49, new[] { "100", "002" }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\0100.jpg",   @"C:\Drive\Folder\Subfolder\0002.jpg" });
            yield return (50, new[] { "100", "002" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\100.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });

            // Digits + Numbers
            yield return (51, new[] { "010", "002" }, 3, true, new[] { @"C:\Drive\Folder\Subfolder\00010.jpg", @"C:\Drive\Folder\Subfolder\00002.jpg" });
            yield return (52, new[] { "010", "002" }, 2, true, new[] { @"C:\Drive\Folder\Subfolder\0010.jpg",  @"C:\Drive\Folder\Subfolder\0002.jpg" });
            yield return (53, new[] { "010", "002" }, 1, true, new[] { @"C:\Drive\Folder\Subfolder\010.jpg",   @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (54, new[] { "010", "002" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\10.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });

            // No zeros: Zeros & Digits vs Name
            yield return (55, new[] { "01", "test" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\test.jpg" });
            yield return (56, new[] { "1", "0test" }, 0, true, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\test.jpg" });
        }

        private static IEnumerable<(int Id, string[] TestNames, byte LeadingZeros, bool IsAbsolutePathOn, string[] ExpectedPaths)> GetRelativeTestCases()
        {
            // No zeros: only Zeros + alignment
            yield return (57, new[] { "000", "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (58, new[] { "00",  "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (59, new[] { "00",  "00"  }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (60, new[] { "00",  "0"   }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (61, new[] { "0",   "0"   }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (62, new[] { "0",   "00"  }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (63, new[] { "0",   "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (64, new[] { "00",  "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (65, new[] { "000", "00"  }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });
            yield return (66, new[] { "000", "0"   }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });

            // No zeros: Zeros vs Digits + alignment
            yield return (67, new[] { "000", "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\000.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (68, new[] { "00",  "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\000.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (69, new[] { "00",  "20"  }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\00.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (70, new[] { "00",  "2"   }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (71, new[] { "0",   "2"   }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (72, new[] { "0",   "20"  }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\00.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (73, new[] { "0",   "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\000.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (74, new[] { "00",  "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\000.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (75, new[] { "000", "20"  }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\00.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (76, new[] { "000", "2"   }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\0.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });

            // No zeros: Zeros & Digits + alignment
            yield return (77, new[] { "100", "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (78, new[] { "100", "020" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\020.jpg" });
            yield return (79, new[] { "100", "002" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (80, new[] { "100", "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\000.jpg" });

            yield return (81, new[] { "100", "20" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\020.jpg" });
            yield return (82, new[] { "100", "02" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (83, new[] { "100", "00" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\000.jpg" });

            yield return (84, new[] { "100", "2" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            yield return (85, new[] { "100", "0" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\100.jpg", @"C:\Drive\Folder\Subfolder\000.jpg" });

            yield return (86, new[] { "010", "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\010.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (87, new[] { "010", "020" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (88, new[] { "010", "002" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg",  @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (89, new[] { "010", "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg",  @"C:\Drive\Folder\Subfolder\00.jpg" });

            yield return (90, new[] { "010", "20" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (91, new[] { "010", "02" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (92, new[] { "010", "00" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\00.jpg" });

            yield return (93, new[] { "010", "2" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (94, new[] { "010", "0" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\00.jpg" });

            yield return (95, new[] { "001", "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (96, new[] { "001", "020" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\01.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (97, new[] { "001", "002" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (98, new[] { "001", "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",   @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (99,  new[] { "001", "20" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (100, new[] { "001", "02" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (101, new[] { "001", "00" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (102, new[] { "001", "2" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (103, new[] { "001", "0" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (104, new[] { "10", "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\010.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (105, new[] { "10", "020" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (106, new[] { "10", "002" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg",  @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (107, new[] { "10", "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg",  @"C:\Drive\Folder\Subfolder\00.jpg" });

            yield return (108, new[] { "10", "20" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (109, new[] { "10", "02" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (110, new[] { "10", "00" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\00.jpg" });

            yield return (111, new[] { "10", "2" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\02.jpg" });
            yield return (112, new[] { "10", "0" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\10.jpg", @"C:\Drive\Folder\Subfolder\00.jpg" });

            yield return (113, new[] { "01", "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (114, new[] { "01", "020" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\01.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (115, new[] { "01", "002" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (116, new[] { "01", "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",   @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (117, new[] { "01", "20" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (118, new[] { "01", "02" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (119, new[] { "01", "00" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (120, new[] { "01", "2" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (121, new[] { "01", "0" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (122, new[] { "1", "200" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\200.jpg" });
            yield return (123, new[] { "1", "020" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\01.jpg",  @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (124, new[] { "1", "002" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (125, new[] { "1", "000" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",   @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (126, new[] { "1", "20" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\01.jpg", @"C:\Drive\Folder\Subfolder\20.jpg" });
            yield return (127, new[] { "1", "02" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (128, new[] { "1", "00" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\0.jpg" });

            yield return (129, new[] { "1", "2" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (130, new[] { "1", "0" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\0.jpg" });

            // No zeros: Zeros & Digits vs Name
            yield return (131, new[] { "01", "test" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\0test.jpg" });
            yield return (132, new[] { "1", "0test" }, 0, false, new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\0test.jpg" });
        }
    }
}
