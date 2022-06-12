using FileManager_Logic;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

#pragma warning disable CA1707   // Allow underscores in tests names
#pragma warning disable IDE0130  // Allow underscores in namespaces

namespace FileManager_Tests
{
    public class FileManagerTests
    {
        #region RegEx for path
        [TestCase("", false, "", "", "")]
        [TestCase(" ", false, "", "", "")]
        [TestCase("C:\\Users\\User\\Desktop\\Folder\\Subfolder\\File.dat", true, "C:\\Users\\User\\Desktop\\Folder\\Subfolder\\", "File", ".dat")]
        [TestCase("C:\\Users\\User\\Desktop\\Folder\\Subfolder\\!File.dat", true, "C:\\Users\\User\\Desktop\\Folder\\Subfolder\\", "!File", ".dat")]
        [TestCase("C:\\Users\\User\\Desktop\\Folder\\Subfolder\\Save File.dat", true, "C:\\Users\\User\\Desktop\\Folder\\Subfolder\\", "Save File", ".dat")]
        [TestCase("C:\\Users\\User\\File", false, "", "", "")]
        [TestCase("File.dat", false, "", "", "")]
        public void CheckIfField_WithRegExPattern_ReturnsAllPathComponents(string testPath, bool isSuccess, string path, string name, string extension)
        {
            // Act
            Match match = Regex.Match(testPath, FilesManager.FilePathPattern);

            // Arrange
            Assert.IsTrue(match.Success == isSuccess);

            if (isSuccess)
            {
                GroupCollection groups = match.Groups;

                Assert.That(groups[FilesManager.PathGroup].Value, Is.EqualTo(path));
                Assert.That(groups[FilesManager.NameGroup].Value, Is.EqualTo(name));
                Assert.That(groups[FilesManager.ExtensionGroup].Value, Is.EqualTo(extension));
            }
        }
        #endregion

        #region Replacing name with incremented number
        [Test]
        public void CheckIfMethod_GetNumberIncrementedName_ForValidPath_AndNumber_ReturnsChangedFileName()
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\abc.jpg";
            const string ResultFilePath = @"C:\Drive\Folder\7.jpg";

            // Act
            string actualFilePath = FilesManager.GetNumberIncrementedName(TestFilePath, String.Empty, 7, String.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(ResultFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void CheckIfMethod_GetNumberIncrementedName_ForValidPath_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}4.jpg";

            // Act
            string actualFilePath = FilesManager.GetNumberIncrementedName(TestFilePath, testPrefix, 4, String.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void CheckIfMethod_GetNumberIncrementedName_ForValidPath_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualFilePath = FilesManager.GetNumberIncrementedName(TestFilePath, String.Empty, 4, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void CheckIfMethod_GetNumberIncrementedName_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = FilesManager.GetNumberIncrementedName(testPath, "a", 9, "z");

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(String.Empty));
        }
        #endregion

        #region Replacing name with prepend and append
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void CheckIfMethod_GetPrependedAndAppendedName_ForValidPath_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}Test.jpg";

            // Act
            string actualFilePath = FilesManager.GetPrependedAndAppendedName(TestFilePath, testPrefix, String.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void CheckIfMethod_GetPrependedAndAppendedName_ForValidPath_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\Test{expectedPostfix}.jpg";

            // Act
            string actualFilePath = FilesManager.GetPrependedAndAppendedName(TestFilePath, String.Empty, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void CheckIfMethod_GetPrependedAndAppendedName_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = FilesManager.GetPrependedAndAppendedName(testPath, "a", "z");

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(String.Empty));
        }
        #endregion
    }
}
