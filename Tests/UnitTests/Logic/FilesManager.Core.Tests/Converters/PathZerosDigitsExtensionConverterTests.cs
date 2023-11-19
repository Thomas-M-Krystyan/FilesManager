using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.UnitTests._TestHelpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace FilesManager.Core.UnitTests.Converters
{
    [TestFixture]
    internal class PathZerosDigitsExtensionConverterTests
    {
        private readonly IFilePathConverter<FilePathNameDto, FileZerosDigitsDto> _converter
            = new FileZerosDigitsDtoConverter();

        #region ConvertToDto
        [Test]
        public void ConvertToDto_ForValidDto_ReturnsExpectedDto()
        {
            // Arrange
            FilePathNameDto testDto = TestHelpers.GetTestDto(
                @"C:\Drive\Folder\", "01New test", ".csr");

            // Act
            FileZerosDigitsDto actualDto = this._converter.ConvertToDto(testDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualDto.Path, Is.EqualTo(@"C:\Drive\Folder\"));
                Assert.That(actualDto.Zeros, Is.EqualTo("0"));
                Assert.That(actualDto.Digits, Is.EqualTo("1"));
                Assert.That(actualDto.Name, Is.EqualTo("New test"));
                Assert.That(actualDto.Extension, Is.EqualTo(".csr"));
            });
        }

        [Test]
        public void ConvertToDto_ForEmptyDto_ReturnsExpectedDto()
        {
            // Arrange
            FilePathNameDto testDto = new(string.Empty, string.Empty, string.Empty);

            // Act
            FileZerosDigitsDto actualDto = this._converter.ConvertToDto(testDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualDto.Path, Is.Empty);
                Assert.That(actualDto.Zeros, Is.Empty);
                Assert.That(actualDto.Digits, Is.Empty);
                Assert.That(actualDto.Name, Is.Empty);
                Assert.That(actualDto.Extension, Is.Empty);
            });
        }
        #endregion

        #region GetFilePath
        [TestCase("",  "",  "")]
        [TestCase(" ", " ", " ")]
        public void GetFilePath_ForInvalidPath_ReturnsEmptyString(string testZeros, string testDigits, string testName)
        {
            // Arrange
            FileZerosDigitsDto testDto
                = new(@"C:\Drive\Folder\", testZeros, testDigits, testName, ".csr");

            // Act
            string actualResult = this._converter.GetFilePath(testDto);

            // Assery
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void GetFilePath_ForValidPath_ReturnsExpectedString()
        {
            // Arrange
            FileZerosDigitsDto testDto
                = new(@"C:\Drive\Folder\", "0", "1", "New test", ".csr");

            // Act
            string actualResult = this._converter.GetFilePath(testDto);

            // Assery
            Assert.That(actualResult, Is.EqualTo(@"C:\Drive\Folder\01New test.csr"));
        }
        #endregion
    }
}
