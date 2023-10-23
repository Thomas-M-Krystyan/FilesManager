using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.UnitTests._TestHelpers;
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
                StartingNumber = "4",
                NamePostfix = string.Empty
            };

            const string ExpectedNewFilePath = @"C:\Drive\Folder\Subfolder\4.jpg";
            
            // Act
            string actualNewFilePath = strategy.GetNewFilePath(
                TestHelpers.GetMockedMatch(@$"C:\Drive\Folder\Subfolder\", "7", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(ExpectedNewFilePath));
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

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}4.jpg";

            // Act
            string actualNewFilePath = strategy.GetNewFilePath(
                TestHelpers.GetMockedMatch(@"C:\Drive\Folder\Subfolder\", "4", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
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

            string expectedNewFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualNewFilePath = strategy.GetNewFilePath(
                TestHelpers.GetMockedMatch(@"C:\Drive\Folder\Subfolder\", "ABC", ".jpg"));

            // Assert
            Assert.That(actualNewFilePath, Is.EqualTo(expectedNewFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("", ".jpg")]                  // No path
        [TestCase(@"C:\Drive\Folder\abc", "")]  // No extension
        public void GetNewFilePath_ForInvalidPath_WithOtherParameters_ReturnsEmptyString(string testPath, string testExtension)
        {
            // Arrange
            StrategyBase strategy = new IncrementNumberViewModel
            {
                NamePrefix = "a",
                StartingNumber = "9",
                NamePostfix = "z"
            };

            Match testMatch = TestHelpers.GetMockedMatch(testPath, "Test_#1", testExtension);

            // Act & Assert
            InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(() => strategy.GetNewFilePath(testMatch));

            Assert.That(exception?.Message?.StartsWith(Resources.ERROR_Internal_InvalidFilePathDto), Is.True);
        }
    }
}
