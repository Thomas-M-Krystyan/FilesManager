using FilesManager.Core.Models.DTOs.Files.Abstractions;
using FilesManager.UI.Desktop.UnitTests._TestHelpers;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Strategies
{
    [TestFixture]
    internal sealed class IncrementNumberViewModelTests
    {
        private StrategyBase<BasePathDto>? _strategy;

        [Test]
        public void GetNewFilePath_ForValidPath_WithNumber_ReturnsChangedFileName()
        {
            // Arrange
            this._strategy = new IncrementNumberViewModel
            {
                NamePrefix = string.Empty,
                StartingNumber = "4",
                NamePostfix = string.Empty
            };

            const string ExpectedNewFilePath = @"C:\Drive\Folder\Subfolder\4.jpg";
            
            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(
                TestHelpers.GetMockedDto(@$"C:\Drive\Folder\Subfolder\", "7", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(ExpectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("pre", "pre")]
        public void GetNewFilePath_ForValidPath_WithNumber_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            this._strategy = new IncrementNumberViewModel
            {
                NamePrefix = testPrefix,
                StartingNumber = "4",
                NamePostfix = string.Empty
            };

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}4.jpg";

            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(
                TestHelpers.GetMockedDto(@"C:\Drive\Folder\Subfolder\", "4", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("post", "post")]
        public void GetNewFilePath_ForValidPath_WithNumber_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            this._strategy = new IncrementNumberViewModel
            {
                NamePrefix = string.Empty,
                StartingNumber = "4",
                NamePostfix = testPostfix
            };

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualNewFilePath = this._strategy.GetNewFilePath(
                TestHelpers.GetMockedDto(@"C:\Drive\Folder\Subfolder\", "ABC", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }
    }
}
