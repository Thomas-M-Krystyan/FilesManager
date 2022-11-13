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
        [TestCaseSource(nameof(GetTestFileNames))]
        public void TestMethod_GetMaxLength_ForValidInput_ReturnsExpectedMaxFileLength((string[] Names, int ExpectedCount) testData)
        {
            // Act
            int result = RenamingService.GetMaxLength(testData.Names);

            // Assert
            Assert.That(result, Is.EqualTo(testData.ExpectedCount));
        }

        private static IEnumerable<(string[] Names, int ExpectedCount)> GetTestFileNames()
        {
            yield return (new[] { "" }, 0);
            yield return (new[] { "test1" }, 5);
            yield return (new[] { "test1", "test25" }, 6);
            yield return (new[] { "test1", "test25", "test300" }, 7);
        }

        [TestCaseSource(nameof(GetTestFileNames_Zeros_MaxLength))]
        public void TestMethod_AddLeadingZeros_ForValidInput_ReturnsExpectedChangedFileNames((string[] OriginalNames, byte ZerosCount, ushort MaxFileLength, string[] ExpectedNames) testData)
        {
            // Act
            string[] actualNames = RenamingService.AddLeadingZeros(testData.OriginalNames, testData.ZerosCount, testData.MaxFileLength);

            // Assert
            for (int index = 0; index < actualNames.Length; index++)
            {
                Assert.That(actualNames[index], Is.EqualTo(testData.ExpectedNames[index]));
            }
        }

        private static IEnumerable<(string[] OriginalNames, byte ZerosCount, ushort MaxFileLength, string[] ExpectedNames)> GetTestFileNames_Zeros_MaxLength()
        {
            yield return (new[] { "1" }, 2, 1, new[] { "001" });
            yield return (new[] { "1" }, 0, 1, new[] { "1" });
            yield return (new[] { "1", "10" }, 2, 2, new[] { "0001", "0010" });
            yield return (new[] { "1", "10" }, 0, 2, new[] { "1", "10" });
            yield return (new[] { "1", "10", "101" }, 2, 3, new[] { "00001", "00010", "00101" });
            yield return (new[] { "1", "", "101" }, 1, 3, new[] { "0001", "", "0101" });
            yield return (new[] { "" }, 2, 0, new[] { "" });
            yield return (new[] { "" }, 0, 0, new[] { "" });
        }
        #endregion
    }
}
