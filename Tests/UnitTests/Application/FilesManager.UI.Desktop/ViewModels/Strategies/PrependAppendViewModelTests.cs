using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.Core.UnitTests.Services.Renaming.Strategies
{
    [TestFixture]
    internal sealed class PrependAppendViewModelTests
    {
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("pre", "pre")]
        public void GetNewFilePath_ForValidPath_AndPrepend_AddsPrependToChangedName(string testPrepend, string expectedPrepend)
        {
            // Arrange
            StrategyBase strategy = new PrependAppendViewModel
            {
                PrependName = testPrepend,
                AppendName = string.Empty
            };

            const string ActualOldFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrepend}Test.jpg";

            // Act
            string actualFilePath = strategy.GetNewFilePath(ActualOldFilePath);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("post", "post")]
        public void GetNewFilePath_ForValidPath_AndAppend_AddsAppendToChangedName(string testAppend, string expectedAppend)
        {
            // Arrange
            StrategyBase strategy = new PrependAppendViewModel
            {
                PrependName = string.Empty,
                AppendName = testAppend
            };

            const string ActualOldFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\Test{expectedAppend}.jpg";

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
            StrategyBase strategy = new PrependAppendViewModel
            {
                PrependName = "a",
                AppendName = "z"
            };

            // Act
            string actualFilePath = strategy.GetNewFilePath(testPath);

            // Assert
            Assert.That(actualFilePath, Is.Empty);
        }
    }
}
