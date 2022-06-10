using FileManager_Logic;
using NUnit.Framework;
using System;

namespace FileManager_Tests
{
    public class FileManagerTests
    {
        [Test]
        public void CheckIfMethod_RenameFile_ForValidPath_AndNumber_ReturnsChangedFileName()
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\abc.jpg";
            const string ResultFilePath = @"C:\Drive\Folder\7.jpg";

            // Act
            string actualFilePath = FilesManager.RenameFile(TestFilePath, 7);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(ResultFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void CheckIfMethod_RenameFile_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = FilesManager.RenameFile(testPath, 9);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(String.Empty));
        }
    }
}
