using FilesManager.UI.Desktop.UnitTests._TestHelpers;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Strategies
{
    [TestFixture]
    internal sealed class LeadingZerosViewModelTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public void GetNewFilePath_ForGivenInput_ReturnsExpectedFileName((int Id, string[] TestNames, string LeadingZeros, string[] ExpectedPaths) data)
        {
            // Arrange
            StrategyBase strategy = new LeadingZerosViewModel
            {
                LeadingZeros = data.LeadingZeros,
                MaxFileLength = Math.Max(data.TestNames[0].Length, data.TestNames[1].Length)
            };

            // Act
            string[] actualPaths = data.TestNames.Select(name =>
                strategy.GetNewFilePath(
                    TestHelpers.GetMockedMatch(@"C:\Drive\Folder\Subfolder\", name, ".jpg")))
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
            yield return (1, new[] { "1", "2" }, "a",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (2, new[] { "1", "2" }, "Z",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (3, new[] { "1", "2" }, "*",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (4, new[] { "1", "2" }, "-1", new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (5, new[] { "1", "2" }, "8",  new[] { @"C:\Drive\Folder\Subfolder\1.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });

            // No zeros: only digits
            yield return (6,  new[] { "1",   "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (7,  new[] { "01",  "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1.jpg",  @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (8,  new[] { "012", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\12.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            // No zeros: digits + name
            yield return (9,  new[] { "1a",     "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1a.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (10, new[] { "01a",    "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\1a.jpg",    @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (11, new[] { "012a",   "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\12a.jpg",   @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (12, new[] { "012abc", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\12abc.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            // No zeros: name
            yield return (13, new[] { "0a", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\a.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            // No zeros: nothing else
            yield return (14, new[] { "00", "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });
            yield return (15, new[] { "0",  "2" }, "0", new[] { @"C:\Drive\Folder\Subfolder\0.jpg", @"C:\Drive\Folder\Subfolder\2.jpg" });

            //// Empty
            //yield return (new[] { string.Empty, string.Empty }, "2", new[] { string.Empty, string.Empty });
            //yield return (new[] { string.Empty, string.Empty }, "1", new[] { string.Empty, string.Empty });
            //yield return (new[] { string.Empty, string.Empty }, "0", new[] { string.Empty, string.Empty });

            // Digits
            yield return (16, new[] { "1", "02" }, "2", new[] { @"C:\Drive\Folder\Subfolder\001.jpg", @"C:\Drive\Folder\Subfolder\002.jpg" });
            //yield return ("3",          "003",  "2", @"C:\Drive\Folder\Subfolder\003.jpg");
            //yield return ("3",          "0003", "2", @"C:\Drive\Folder\Subfolder\0003.jpg");


            //yield return ("2",          "", "1", @"C:\Drive\Folder\Subfolder\02.jpg");
            //yield return ("1",          "", "0", @"C:\Drive\Folder\Subfolder\1.jpg");
            //// Zeros + Digits
            //yield return ("03",         "", "2", @"C:\Drive\Folder\Subfolder\0003.jpg");
            //yield return ("03",         "", "2", @"C:\Drive\Folder\Subfolder\003.jpg");
            //yield return ("02",         "", "1", @"C:\Drive\Folder\Subfolder\002.jpg");
            //yield return ("02",         "", "1", @"C:\Drive\Folder\Subfolder\02.jpg");
            //yield return ("01",         "", "0", @"C:\Drive\Folder\Subfolder\1.jpg");
            //// Zeros (only)
            //yield return ("0",          "", "0", @"C:\Drive\Folder\Subfolder\0.jpg");    // Cannot remove 0 because there would be no file name afterward
            //yield return ("00",         "", "0", @"C:\Drive\Folder\Subfolder\00.jpg");   // Cannot remove 0 because there would be no file name afterward
            //// Text
            //yield return ("test3",      "", "2", @"C:\Drive\Folder\Subfolder\00test3.jpg");
            //yield return ("test2",      "", "1", @"C:\Drive\Folder\Subfolder\0test2.jpg");
            //yield return ("test1",      "", "0", @"C:\Drive\Folder\Subfolder\test1.jpg");
            //// Digit + Text
            //yield return ("3test",      "", "2", @"C:\Drive\Folder\Subfolder\003test.jpg");
            //yield return ("2test",      "", "1", @"C:\Drive\Folder\Subfolder\02test.jpg");
            //yield return ("1test",      "", "0", @"C:\Drive\Folder\Subfolder\1test.jpg");
            //yield return ("10test",     "", "0", @"C:\Drive\Folder\Subfolder\10test.jpg");
            //// Zeros + Text
            //yield return ("0test",      "", "0", @"C:\Drive\Folder\Subfolder\test.jpg");   // This time removing 0 is allowed, because there would be a file name afterward
            //yield return ("00test",     "", "0", @"C:\Drive\Folder\Subfolder\test.jpg");   // This time removing 0 is allowed, because there would be a file name afterward
            //// Zeros + Digit + Text
            //yield return ("01test",     "", "0", @"C:\Drive\Folder\Subfolder\1test.jpg");  // This time removing 0 is allowed, because there would be a file name afterward
            //yield return ("01test",     "", "1", @"C:\Drive\Folder\Subfolder\01test.jpg");
            //yield return ("01test",     "", "2", @"C:\Drive\Folder\Subfolder\001test.jpg");
            //// Argument out of range
            //yield return ("1",          "", "8", @"C:\Drive\Folder\Subfolder\1.jpg");
            //yield return ("00000001",   "", "8", @"C:\Drive\Folder\Subfolder\00000001.jpg");
            //yield return ("000000001",  "", "8", @"C:\Drive\Folder\Subfolder\000000001.jpg");
            //yield return ("0000000001", "", "8", @"C:\Drive\Folder\Subfolder\0000000001.jpg");
            //// No change of leading zeros
            //yield return ("00000001",   "", "7", @"C:\Drive\Folder\Subfolder\00000001.jpg");
            //// Removing of leading zeros
            //yield return ("00000001",   "", "6", @"C:\Drive\Folder\Subfolder\0000001.jpg");
            //yield return ("00000001",   "", "5", @"C:\Drive\Folder\Subfolder\000001.jpg");
            //yield return ("00000001",   "", "4", @"C:\Drive\Folder\Subfolder\00001.jpg");
            //yield return ("00000001",   "", "3", @"C:\Drive\Folder\Subfolder\0001.jpg");
            //yield return ("00000001",   "", "2", @"C:\Drive\Folder\Subfolder\001.jpg");
            //yield return ("00000001",   "", "1", @"C:\Drive\Folder\Subfolder\01.jpg");
            //yield return ("00000001",   "", "0", @"C:\Drive\Folder\Subfolder\1.jpg");
        }
    }
}
