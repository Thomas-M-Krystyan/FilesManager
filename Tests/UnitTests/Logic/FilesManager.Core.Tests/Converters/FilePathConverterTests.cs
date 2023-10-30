using FilesManager.Core.Converters;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Files.Abstractions;

namespace FilesManager.Core.UnitTests.Converters
{
    [TestFixture]
    internal class FilePathConverterTests
    {
        private readonly BasePathDto _testDto = new PathNameExtensionDto(@"C:\Drive\Folder\", "Test", ".csr");

        [TestCase("")]
        [TestCase(" ")]
        public void GetFilePath_ForInvalidPath_ReturnsEmptyString(string testName)
        {
            // Act
            string actualResult = this._testDto.GetFilePath(testName);

            // Assery
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void GetFilePath_ForValidPath_ReturnsExpectedString()
        {
            // Act
            string actualResult = this._testDto.GetFilePath("New test");

            // Assery
            Assert.That(actualResult, Is.EqualTo(@"C:\Drive\Folder\New test.csr"));
        }
    }
}
