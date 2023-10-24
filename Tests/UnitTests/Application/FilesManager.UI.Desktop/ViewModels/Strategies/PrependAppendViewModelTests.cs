using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.UnitTests._TestHelpers;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

namespace FilesManager.UI.Desktop.UnitTests.ViewModels.Strategies
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

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrepend}Test.jpg";

            // Act
            string actualNewFilePath = strategy.GetNewFilePath(
                TestHelpers.GetMockedMatch(@"C:\Drive\Folder\Subfolder\", "Test", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
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

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\Test{expectedAppend}.jpg";

            // Act
            string actualNewFilePath = strategy.GetNewFilePath(
                TestHelpers.GetMockedMatch(@"C:\Drive\Folder\Subfolder\", "Test", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("", "", "")]
        [TestCase(" ", " ", " ")]
        [TestCase("", "abc", ".jpg")]               // No path
        [TestCase(@"C:\Drive\Folder\", "abc", "")]  // No extension
        [TestCase("", "abc", "")]                   // No path and extension
        public void GetNewFilePath_ForInvalidPath_WithOtherParameters_ReturnsEmptyString(string testPath, string testName, string testExtension)
        {
            // Arrange
            PrependAppendViewModel strategy = new PrependAppendViewModel
            {
                PrependName = "a",
                AppendName = "z"
            };

            Match testMatch = TestHelpers.GetMockedMatch(testPath, testName, testExtension);

            // Act & Assert
            InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(() => strategy.GetNewFilePath(testMatch));

            Assert.That(exception?.Message?.StartsWith(Resources.ERROR_Internal_InvalidFilePathDto), Is.True);
        }
    }
}
