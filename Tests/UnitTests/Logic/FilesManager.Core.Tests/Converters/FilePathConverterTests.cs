using FilesManager.Core.Converters;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Files.Abstractions;

namespace FilesManager.Core.UnitTests.Converters
{
    [TestFixture]
    internal class FilePathConverterTests
    {
        private BasePathDto? _testDto;

        [TestCase("")]
        [TestCase(" ")]
        public void GetNewFilePath_ForInvalidPath_WithOtherParameters_ReturnsEmptyString(string testName)
        {
            // Arrange
            this._testDto = new PathNameExtensionDto(@"C:\Drive\Folder\", "Test", ".csr");

            // Act
            string actualResult = this._testDto.GetFilePath(testName);

            // Assery
            Assert.That(actualResult, Is.Empty);
        }
    }
}
