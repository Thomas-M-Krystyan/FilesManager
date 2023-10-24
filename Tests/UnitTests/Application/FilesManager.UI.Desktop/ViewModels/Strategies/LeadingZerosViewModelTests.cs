using FilesManager.UI.Desktop.UnitTests._TestHelpers;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Strategies
{
    [TestFixture]
    internal sealed class LeadingZerosViewModelTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public void GetNewFilePath_ForGivenInput_ReturnsExpectedFileName((string Name, string LeadingZeros, int? MaxFileLength, string ExpectedNewFilePath) testData)
        {
            // Arrange
            StrategyBase strategy = new LeadingZerosViewModel
            {
                LeadingZeros = testData.LeadingZeros,
                MaxFileLength = testData.MaxFileLength ?? testData.Name.Length
            };

            // Act
            string actualNewFilePath = strategy.GetNewFilePath(
                TestHelpers.GetMockedMatch(@"C:\Drive\Folder\Subfolder\", testData.Name, ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(testData.ExpectedNewFilePath));
        }

        private static IEnumerable<(string Name, string LeadingZeros, int? MaxFileLength, string ExpectedNewFilePath)> GetTestCases()
        {
            // Digits
            yield return ("3",          "2", null, @"C:\Drive\Folder\Subfolder\003.jpg");
            yield return ("2",          "1", null, @"C:\Drive\Folder\Subfolder\02.jpg");
            yield return ("1",          "0", null, @"C:\Drive\Folder\Subfolder\1.jpg");
            // Zeros + Digits
            yield return ("03",         "2", 4,    @"C:\Drive\Folder\Subfolder\0003.jpg");
            yield return ("03",         "2", null, @"C:\Drive\Folder\Subfolder\003.jpg");
            yield return ("02",         "1", 3,    @"C:\Drive\Folder\Subfolder\002.jpg");
            yield return ("02",         "1", null, @"C:\Drive\Folder\Subfolder\02.jpg");
            yield return ("01",         "0", null, @"C:\Drive\Folder\Subfolder\1.jpg");
            // Zeros (only)
            yield return ("0",          "0", null, @"C:\Drive\Folder\Subfolder\0.jpg");    // Cannot remove 0 because there would be no file name afterward
            yield return ("00",         "0", null, @"C:\Drive\Folder\Subfolder\00.jpg");   // Cannot remove 0 because there would be no file name afterward
            // Text
            yield return ("test3",      "2", null, @"C:\Drive\Folder\Subfolder\00test3.jpg");
            yield return ("test2",      "1", null, @"C:\Drive\Folder\Subfolder\0test2.jpg");
            yield return ("test1",      "0", null, @"C:\Drive\Folder\Subfolder\test1.jpg");
            // Digit + Text
            yield return ("3test",      "2", null, @"C:\Drive\Folder\Subfolder\003test.jpg");
            yield return ("2test",      "1", null, @"C:\Drive\Folder\Subfolder\02test.jpg");
            yield return ("1test",      "0", null, @"C:\Drive\Folder\Subfolder\1test.jpg");
            yield return ("10test",     "0", null, @"C:\Drive\Folder\Subfolder\10test.jpg");
            // Zeros + Text
            yield return ("0test",      "0", null, @"C:\Drive\Folder\Subfolder\test.jpg");   // This time removing 0 is allowed, because there would be a file name afterward
            yield return ("00test",     "0", null, @"C:\Drive\Folder\Subfolder\test.jpg");   // This time removing 0 is allowed, because there would be a file name afterward
            // Zeros + Digit + Text
            yield return ("01test",     "0", null, @"C:\Drive\Folder\Subfolder\1test.jpg");  // This time removing 0 is allowed, because there would be a file name afterward
            yield return ("01test",     "1", null, @"C:\Drive\Folder\Subfolder\01test.jpg");
            yield return ("01test",     "2", null, @"C:\Drive\Folder\Subfolder\001test.jpg");
            // Argument out of range
            yield return ("1",          "8", null, @"C:\Drive\Folder\Subfolder\1.jpg");
            yield return ("00000001",   "8", null, @"C:\Drive\Folder\Subfolder\00000001.jpg");
            yield return ("000000001",  "8", null, @"C:\Drive\Folder\Subfolder\000000001.jpg");
            yield return ("0000000001", "8", null, @"C:\Drive\Folder\Subfolder\0000000001.jpg");
            // No change of leading zeros
            yield return ("00000001",   "7", null, @"C:\Drive\Folder\Subfolder\00000001.jpg");
            // Removing of leading zeros
            yield return ("00000001",   "6", null, @"C:\Drive\Folder\Subfolder\0000001.jpg");
            yield return ("00000001",   "5", null, @"C:\Drive\Folder\Subfolder\000001.jpg");
            yield return ("00000001",   "4", null, @"C:\Drive\Folder\Subfolder\00001.jpg");
            yield return ("00000001",   "3", null, @"C:\Drive\Folder\Subfolder\0001.jpg");
            yield return ("00000001",   "2", null, @"C:\Drive\Folder\Subfolder\001.jpg");
            yield return ("00000001",   "1", null, @"C:\Drive\Folder\Subfolder\01.jpg");
            yield return ("00000001",   "0", null, @"C:\Drive\Folder\Subfolder\1.jpg");
            // Empty
            yield return (string.Empty, "2", null, string.Empty);
            yield return (string.Empty, "1", null, string.Empty);
            yield return (string.Empty, "0", null, string.Empty);
        }
    }
}
