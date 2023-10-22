using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Strategies
{
    [TestFixture]
    internal sealed class IncrementNumberViewModelTests
    {
        [Test]
        public void GetNewFilePath_ForValidPath_WithNumber_ReturnsChangedFileName()
        {
            // Arrange
            StrategyBase strategy = new IncrementNumberViewModel
            {
                NamePrefix = string.Empty,
                StartingNumber = "7",
                NamePostfix = string.Empty
            };

            const string ActualOldFilePath = @"C:\Drive\Folder\abc.jpg";
            const string ExpectedNewFilePath = @"C:\Drive\Folder\7.jpg";

            // Act
            string actualFilePath = strategy.GetNewFilePath(ActualOldFilePath);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(ExpectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("pre", "pre")]
        public void GetNewFilePath_ForValidPath_WithNumber_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            StrategyBase strategy = new IncrementNumberViewModel
            {
                NamePrefix = testPrefix,
                StartingNumber = "4",
                NamePostfix = string.Empty
            };

            const string ActualOldFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}4.jpg";

            // Act
            string actualFilePath = strategy.GetNewFilePath(ActualOldFilePath);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("post", "post")]
        public void GetNewFilePath_ForValidPath_WithNumber_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            StrategyBase strategy = new IncrementNumberViewModel
            {
                NamePrefix = string.Empty,
                StartingNumber = "4",
                NamePostfix = testPostfix
            };

            const string ActualOldFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualFilePath = strategy.GetNewFilePath(ActualOldFilePath);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void GetNewFilePath_ForInvalidPath_WithOtherParameters_ReturnsEmptyString(string testPath)
        {
            // Arrange
            StrategyBase strategy = new IncrementNumberViewModel
            {
                NamePrefix = "a",
                StartingNumber = "9",
                NamePostfix = "z"
            };

            // Act
            string actualFilePath = strategy.GetNewFilePath(testPath);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(string.Empty));
        }
    }
}
