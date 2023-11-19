using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Strategies
{
    [TestFixture]
    internal sealed class PrependAppendViewModelTests
    {
        private static readonly IFilePathConverter<Match, FilePathNameDto> Converter = new FilePathNameDtoConverter();

        private StrategyBase? _strategy;

        #region GetNewFilePath()
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("pre", "pre")]
        public void GetNewFilePath_ForValidPath_AndPrepend_AddsPrependToChangedName(string testPrepend, string expectedPrepend)
        {
            // Arrange
            this._strategy = new PrependAppendViewModel(Converter)
            {
                PrependName = testPrepend,
                AppendName = string.Empty
            };

            FilePathNameDto dto = new(@"C:\Drive\Folder\Subfolder\", "Test", ".jpg");
            FileData fileData = new(dto);

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrepend}Test.jpg";

            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(fileData);

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("post", "post")]
        public void GetNewFilePath_ForValidPath_AndAppend_AddsAppendToChangedName(string testAppend, string expectedAppend)
        {
            // Arrange
            this._strategy = new PrependAppendViewModel(Converter)
            {
                PrependName = string.Empty,
                AppendName = testAppend
            };

            FilePathNameDto dto = new(@"C:\Drive\Folder\Subfolder\", "Test", ".jpg");
            FileData fileData = new(dto);

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\Test{expectedAppend}.jpg";

            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(fileData);

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }
        #endregion
    }
}
