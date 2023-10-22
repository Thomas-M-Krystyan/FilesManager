using FilesManager.Core.Services.Strategies;

namespace FilesManager.Core.UnitTests.Services.Strategies
{
    [TestFixture]
    internal class PrependAppendTests
    {
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("X", "X")]  // No path
        public void GetPrependedAndAppendedName_ForValidPath_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}Test.jpg";

            // Act
            string actualFilePath = PrependAppend.GetPrependedAndAppendedName(TestFilePath, testPrefix, string.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("X", "X")]  // No path
        public void GetPrependedAndAppendedName_ForValidPath_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\Test{expectedPostfix}.jpg";

            // Act
            string actualFilePath = PrependAppend.GetPrependedAndAppendedName(TestFilePath, string.Empty, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void GetPrependedAndAppendedName_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = PrependAppend.GetPrependedAndAppendedName(testPath, "a", "z");

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(string.Empty));
        }
    }
}
