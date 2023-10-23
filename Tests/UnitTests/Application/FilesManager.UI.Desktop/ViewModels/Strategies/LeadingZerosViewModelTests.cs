using FilesManager.UI.Desktop.UnitTests._TestHelpers;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.Core.UnitTests.Services.Renaming.Strategies
{
    [TestFixture]
    internal sealed class LeadingZerosViewModelTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public void GetLeadingZeros_ForValidInput_ReturnsExpectedChangedFileNames((string Name, byte ZerosCount, ushort MaxFileLength, string ExpectedNewFilePath) testData)
        {
            // Arrange
            StrategyBase strategy = new LeadingZerosViewModel
            {
                LeadingZeros = testData.ZerosCount,
                MaxFileLength = testData.MaxFileLength
            };

            // Act
            string actualNewFilePath = strategy.GetNewFilePath(
                TestHelpers.GetMockedMatch(@"C:\Drive\Folder\Subfolder\", testData.Name, ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(testData.ExpectedNewFilePath));
        }

        private static IEnumerable<(string Name, byte ZerosCount, ushort MaxFileLength, string ExpectedNewFilePath)> GetTestCases()
        {
            yield return ("3",          2, 1, @"C:\Drive\Folder\Subfolder\003.jpg");
            yield return ("2",          1, 1, @"C:\Drive\Folder\Subfolder\02.jpg");
            yield return ("1",          0, 1, @"C:\Drive\Folder\Subfolder\1.jpg");
            yield return ("0",          0, 1, @"C:\Drive\Folder\Subfolder\0.jpg");
            yield return (string.Empty, 2, 0, string.Empty);
            yield return (string.Empty, 1, 0, string.Empty);
            yield return (string.Empty, 0, 0, string.Empty);
        }
    }
}
