using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.UnitTests._TestHelpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace FilesManager.Core.UnitTests.Converters
{
    [TestFixture]
    internal class PathNameExtensionConverterTests
    {
        private readonly IFilePathConverter<Match, PathNameExtensionDto> _converter = new PathNameExtensionConverter();

        #region ConvertToDto
        [Test]
        public void ConvertToDto_ForValidMatch_ReturnsExpectedDto()
        {
            // Arrange
            Match match = TestHelpers.GetTestMatch(@"C:\Drive\Folder\New test.csr");

            // Act
            PathNameExtensionDto actualDto = this._converter.ConvertToDto(match);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualDto.Path, Is.EqualTo(@"C:\Drive\Folder\"));
                Assert.That(actualDto.Name, Is.EqualTo("New test"));
                Assert.That(actualDto.Extension, Is.EqualTo(".csr"));
            });
        }

        [Test]
        public void ConvertToDto_ForEmptyMatch_ReturnsExpectedDto()
        {
            // Act
            PathNameExtensionDto actualDto = this._converter.ConvertToDto(Match.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualDto.Path, Is.Empty);
                Assert.That(actualDto.Name, Is.Empty);
                Assert.That(actualDto.Extension, Is.Empty);
            });
        }
        #endregion

        #region GetFilePath
        [TestCase("")]
        [TestCase(" ")]
        public void GetFilePath_ForInvalidPath_ReturnsEmptyString(string testName)
        {
            // Arrange
            PathNameExtensionDto testDto = new(@"C:\Drive\Folder\", testName, ".csr");

            // Act
            string actualResult = this._converter.GetFilePath(testDto);

            // Assery
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void GetFilePath_ForValidPath_ReturnsExpectedString()
        {
            // Arrange
            PathNameExtensionDto testDto = new(@"C:\Drive\Folder\", "New test", ".csr");

            // Act
            string actualResult = this._converter.GetFilePath(testDto);

            // Assery
            Assert.That(actualResult, Is.EqualTo(@"C:\Drive\Folder\New test.csr"));
        }
        #endregion
    }
}
