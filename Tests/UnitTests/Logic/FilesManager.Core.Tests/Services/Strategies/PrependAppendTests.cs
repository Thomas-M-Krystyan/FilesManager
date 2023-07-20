using FilesManager.Core.Services.Strategies;
using NUnit.Framework;

namespace FilesManager.Core.Tests.Services.Strategies
{
    [TestFixture]
    internal class PrependAppendTests
    {
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void GetPrependedAndAppendedName_ForValidPath_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}Test.jpg";

            // Act
            string actualFilePath = PrependAppend.GetPrependedAndAppendedName(TestFilePath, testPrefix, String.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void GetPrependedAndAppendedName_ForValidPath_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\Test{expectedPostfix}.jpg";

            // Act
            string actualFilePath = PrependAppend.GetPrependedAndAppendedName(TestFilePath, String.Empty, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void GetPrependedAndAppendedName_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = PrependAppend.GetPrependedAndAppendedName(testPath, "a", "z");

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(String.Empty));
        }
    }
}
