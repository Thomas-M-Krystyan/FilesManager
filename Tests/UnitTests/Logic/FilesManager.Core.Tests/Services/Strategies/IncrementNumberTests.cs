using FilesManager.Core.Services.Strategies;
using NUnit.Framework;

namespace FilesManager.Core.UnitTests.Services.Strategies
{
    [TestFixture]
    internal class IncrementNumberTests
    {
        [Test]
        public void GetNumberIncrementedName_ForValidPath_AndNumber_ReturnsChangedFileName()
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\abc.jpg";
            const string ResultFilePath = @"C:\Drive\Folder\7.jpg";

            // Act
            string actualFilePath = IncrementNumber.GetNumberIncrementedName(TestFilePath, string.Empty, 7, string.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(ResultFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("X", "X")]  // No path
        public void GetNumberIncrementedName_ForValidPath_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}4.jpg";

            // Act
            string actualFilePath = IncrementNumber.GetNumberIncrementedName(TestFilePath, testPrefix, 4, string.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("X", "X")]  // No path
        public void GetNumberIncrementedName_ForValidPath_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualFilePath = IncrementNumber.GetNumberIncrementedName(TestFilePath, string.Empty, 4, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void GetNumberIncrementedName_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = IncrementNumber.GetNumberIncrementedName(testPath, "a", 9, "z");

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(string.Empty));
        }
    }
}
