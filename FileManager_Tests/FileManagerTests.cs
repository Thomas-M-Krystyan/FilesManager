using FileManager_Logic;
using NUnit.Framework;
using System;

#pragma warning disable CA1707   // Allow underscores in tests names
#pragma warning disable IDE0130  // Allow underscores in namespaces

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
            string actualFilePath = FilesManager.RenameFile(TestFilePath, 7, String.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(ResultFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void CheckIfMethod_RenameFile_ForValidPath_AndSuffix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualFilePath = FilesManager.RenameFile(TestFilePath, 4, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void CheckIfMethod_RenameFile_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = FilesManager.RenameFile(testPath, 9, String.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(String.Empty));
        }
    }
}
