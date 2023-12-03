using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Desktop.ViewModels.Layers.Specific.Renaming;
using FilesManager.UI.Desktop.ViewModels.Layers.Specific.Renaming.Base;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Layers.Renaming
{
    [TestFixture]
    internal sealed class IncrementNumberViewModelTests
    {
        private static readonly IFilePathConverter<Match, FilePathNameDto> Converter = new FilePathNameDtoConverter();

        private RenamingBase? _strategy;

        #region GetNewFilePath()
        [Test]
        public void GetNewFilePath_ForValidPath_WithNumber_ReturnsChangedFileName()
        {
            // Arrange
            this._strategy = new IncrementNumberViewModel(Converter)
            {
                NamePrefix = string.Empty,
                StartingNumber = 4,
                NamePostfix = string.Empty
            };

            FilePathNameDto dto = new(@$"C:\Drive\Folder\Subfolder\", "7", ".jpg");
            FileData fileData = new(dto);

            const string ExpectedNewFilePath = @"C:\Drive\Folder\Subfolder\4.jpg";
            
            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(fileData);

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(ExpectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("pre", "pre")]
        public void GetNewFilePath_ForValidPath_WithNumber_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            this._strategy = new IncrementNumberViewModel(Converter)
            {
                NamePrefix = testPrefix,
                StartingNumber = 4,
                NamePostfix = string.Empty
            };

            FilePathNameDto dto = new(@"C:\Drive\Folder\Subfolder\", "4", ".jpg");
            FileData fileData = new(dto);

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}4.jpg";

            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(fileData);

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("post", "post")]
        public void GetNewFilePath_ForValidPath_WithNumber_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            this._strategy = new IncrementNumberViewModel(Converter)
            {
                NamePrefix = string.Empty,
                StartingNumber = 4,
                NamePostfix = testPostfix
            };

            FilePathNameDto dto = new(@"C:\Drive\Folder\Subfolder\", "ABC", ".jpg");
            FileData fileData = new(dto);

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(fileData);

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }
        #endregion
    }
}
