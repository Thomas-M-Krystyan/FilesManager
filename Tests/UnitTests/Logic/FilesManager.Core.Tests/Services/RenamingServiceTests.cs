using FileManager.Layers.Logic;
using NUnit.Framework;

namespace FilesManager.Core.Tests.Services
{
    [TestFixture]
    public class RenamingServiceTests
    {
        #region Replacing name with incremented number
        [Test]
        public void TestMethod_GetNumberIncrementedName_ForValidPath_AndNumber_ReturnsChangedFileName()
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\abc.jpg";
            const string ResultFilePath = @"C:\Drive\Folder\7.jpg";

            // Act
            string actualFilePath = RenamingService.GetNumberIncrementedName(TestFilePath, string.Empty, 7, string.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(ResultFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void TestMethod_GetNumberIncrementedName_ForValidPath_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}4.jpg";

            // Act
            string actualFilePath = RenamingService.GetNumberIncrementedName(TestFilePath, testPrefix, 4, string.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void TestMethod_GetNumberIncrementedName_ForValidPath_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\1.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\4{expectedPostfix}.jpg";

            // Act
            string actualFilePath = RenamingService.GetNumberIncrementedName(TestFilePath, string.Empty, 4, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void TestMethod_GetNumberIncrementedName_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = RenamingService.GetNumberIncrementedName(testPath, "a", 9, "z");

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(string.Empty));
        }
        #endregion

        #region Replacing name with prepend and append
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void TestMethod_GetPrependedAndAppendedName_ForValidPath_AndPrefix_AddsPrefixToChangedName(string testPrefix, string expectedPrefix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\{expectedPrefix}Test.jpg";

            // Act
            string actualFilePath = RenamingService.GetPrependedAndAppendedName(TestFilePath, testPrefix, string.Empty);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase(null, "")]
        [TestCase("X", "X")]  // No path
        public void TestMethod_GetPrependedAndAppendedName_ForValidPath_AndPostfix_AddsPostfixToChangedName(string testPostfix, string expectedPostfix)
        {
            // Arrange
            const string TestFilePath = @"C:\Drive\Folder\Subfolder\Test.jpg";
            string expectedFilePath = @$"C:\Drive\Folder\Subfolder\Test{expectedPostfix}.jpg";

            // Act
            string actualFilePath = RenamingService.GetPrependedAndAppendedName(TestFilePath, string.Empty, testPostfix);

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(@"abc.jpg")]              // No path
        [TestCase(@"C:\Drive\Folder\abc")]  // No extension
        public void TestMethod_GetPrependedAndAppendedName_ForInvalidPath_ReturnsEmptyString(string testPath)
        {
            // Act
            string actualFilePath = RenamingService.GetPrependedAndAppendedName(testPath, "a", "z");

            // Assert
            Assert.That(actualFilePath, Is.EqualTo(string.Empty));
        }
        #endregion

        #region Replacing name with adding leading zeros
        [TestCaseSource(nameof(GetTestFileNames_Zeros_MaxLength))]
        public void TestMethod_GetLeadingZeros_ForValidInput_ReturnsExpectedChangedFileNames((string OriginalName, byte ZerosCount, ushort MaxFileLength, string ExpectedName) testData)
        {
            // Act
            string actualName = RenamingService.GetDigitsWithLeadingZeros(testData.OriginalName, testData.ZerosCount, testData.MaxFileLength);

            // Assert
            Assert.That(actualName, Is.EqualTo(testData.ExpectedName));
        }

        private static IEnumerable<(string OriginalName, byte ZerosCount, ushort MaxFileLength, string ExpectedName)> GetTestFileNames_Zeros_MaxLength()
        {
            yield return ("1", 2, 1, "001");
            yield return ("1", 1, 1, "01");
            yield return ("1", 0, 1, "1");
            yield return ("", 2, 0, "");
            yield return ("", 1, 0, "");
            yield return ("", 0, 0, "");
        }
        #endregion
    }
}
