using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.Core.UnitTests.Services.Renaming.Strategies
{
    [TestFixture]
    internal sealed class LeadingZerosViewModelTests
    {
        [TestCaseSource(nameof(GetTestFileNames_Zeros_MaxLength))]
        public void GetLeadingZeros_ForValidInput_ReturnsExpectedChangedFileNames((string Path, byte ZerosCount, ushort MaxFileLength, string ExpectedName) testData)
        {
            // Arrange
            StrategyBase strategy = new LeadingZerosViewModel
            {
                LeadingZeros = testData.ZerosCount,
                MaxFileLength = testData.MaxFileLength
            };

            // Act
            string actualName = strategy.GetNewFilePath(testData.Path);

            // Assert
            Assert.That(actualName, Is.EqualTo(testData.ExpectedName));
        }

        private static IEnumerable<(string Path, byte ZerosCount, ushort MaxFileLength, string ExpectedName)> GetTestFileNames_Zeros_MaxLength()
        {
            yield return ("1",          2, 1, "001");
            yield return ("1",          1, 1, "01");
            yield return ("1",          0, 1, "1");
            yield return (string.Empty, 2, 0, string.Empty);
            yield return (string.Empty, 1, 0, string.Empty);
            yield return (string.Empty, 0, 0, string.Empty);
        }
    }
}
