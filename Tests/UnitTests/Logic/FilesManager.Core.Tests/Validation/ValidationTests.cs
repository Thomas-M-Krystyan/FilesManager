using FilesManager.Core.Validation;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Tests.Validation
{
    [TestFixture]
    public class ValidationTests
    {
        #region RegEx for invalid characters
        [TestCase("", false)]
        [TestCase(" ", false)]
        // Invalid characters:  \/:*?""<>|
        [TestCase(@"n\ame", true)]
        [TestCase(@"name/", true)]
        [TestCase(@":name", true)]
        [TestCase(@"*name", true)]
        [TestCase(@"name?", true)]
        [TestCase(@"""name", true)]
        [TestCase(@"nam<e", true)]
        [TestCase(@"na>me", true)]
        [TestCase(@"na|me", true)]
        // Valid name
        [TestCase("name", false)]
        public void TestField_InvalidCharactersPattern_WithRegExPattern_RecognizesInvalidCharacters(string fileName, bool isSuccess)
        {
            // Act
            Match match = Regex.Match(fileName, Validate.InvalidCharactersPattern);

            // Arrange
            Assert.That(match.Success, Is.EqualTo(isSuccess));
        }
        #endregion

        #region RegEx for file path
        [TestCase("", false, "", "", "")]
        [TestCase(" ", false, "", "", "")]
        [TestCase("C:\\Users\\User\\Desktop\\Folder\\Subfolder\\File.dat", true, "C:\\Users\\User\\Desktop\\Folder\\Subfolder\\", "File", ".dat")]    // Normal valid file path
        [TestCase("C:\\Users\\User\\Desktop\\Folder\\Subfolder\\!File.dat", true, "C:\\Users\\User\\Desktop\\Folder\\Subfolder\\", "!File", ".dat")]  // With special character in name
        [TestCase("C:\\Users\\User\\Desktop\\Folder\\Subfolder\\Save File.dat", true, "C:\\Users\\User\\Desktop\\Folder\\Subfolder\\", "Save File", ".dat")]  // With space in name
        [TestCase("C:\\Users\\User\\File", false, "", "", "")]  // Without file extension
        [TestCase("File.dat", false, "", "", "")]  // Without file directory
        public void TestField_FilePathPattern_WithRegExPattern_ReturnsAllMatchGroups(string testPath, bool isSuccess, string expectedPath, string expectedName, string expectedExtension)
        {
            // Act
            Match match = Regex.Match(testPath, Validate.FilePathPattern);

            // Arrange
            Assert.Multiple(() =>
            {
                Assert.That(match.Success, Is.EqualTo(isSuccess));
                Assert.That(match.Groups[Validate.PathGroup].Value, Is.EqualTo(expectedPath));
                Assert.That(match.Groups[Validate.NameGroup].Value, Is.EqualTo(expectedName));
                Assert.That(match.Groups[Validate.ExtensionGroup].Value, Is.EqualTo(expectedExtension));
            });
        }
        #endregion

        #region RegEx for zeros and digits
        [TestCase("", true, "", "", "")]
        [TestCase(" ", true, "", "", " ")]
        // Zeros
        [TestCase("000", true, "000", "", "")]
        // Digits
        [TestCase("1000", true, "", "1000", "")]
        // Name
        [TestCase("test", true, "", "", "test")]
        // Combination of data
        [TestCase("0001000test", true, "000", "1000", "test")]
        [TestCase("000test", true, "000", "", "test")]
        [TestCase("0001000", true, "000", "1000", "")]
        [TestCase("1000test", true, "", "1000", "test")]
        public void TestField_LeadingZerosPattern_WithRegExPattern_ReturnsAllMatchGroups(string fileName, bool isSuccess, string expectedZeros, string expectedDigits, string exetedName)
        {
            // Act
            Match match = Regex.Match(fileName, Validate.DigitsNamePattern);

            // Arrange
            Assert.Multiple(() =>
            {
                Assert.That(match.Success, Is.EqualTo(isSuccess));
                Assert.That(match.Groups[Validate.ZerosGroup].Value, Is.EqualTo(expectedZeros));
                Assert.That(match.Groups[Validate.DigitsGroup].Value, Is.EqualTo(expectedDigits));
                Assert.That(match.Groups[Validate.NameGroup].Value, Is.EqualTo(exetedName));
            });
        }
        #endregion

        #region HasValidExtension
        // Invalid caases
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase(null, false)]
        // Too short
        [TestCase("a", false)]
        [TestCase("a.", false)]
        [TestCase(".", false)]
        [TestCase(".z", false)]
        // Missing part
        [TestCase("test", false)]
        [TestCase("test.", false)]
        // Valid cases
        [TestCase("test.h", true)]
        [TestCase("test.7z", true)]
        [TestCase("test.exe", true)]
        [TestCase("test.001", true)]
        [TestCase("test.jpeg", true)]
        // Too long
        [TestCase("test.abcdefghijkl", false)]
        public void TestMethod_HasValidExtension_ForGivenInput_ReturnsExpectedResult(string filePath, bool expectedResult)
        {
            // Act
            bool actualResult = Validate.HasValidExtension(filePath);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        #endregion

        #region IsFilePathValid
        // Invalid cases
        [TestCase("", false, "", "", "")]
        [TestCase(" ", false, "", "", "")]
        [TestCase(null, false, "", "", "")]
        // Valid cases
        [TestCase("C:\\Folder\\File.dat", true, "C:\\Folder\\", "File", ".dat")]  // Simplified file path (with valid structure)
        public void TestMethod_IsFilePathValid_ForValidInput_ReturnsExpectedResult_AndMatch(string testPath, bool isSuccess, string expectedPath, string expectedName, string expectedExtension)
        {
            // Act
            Match actualResult = Validate.IsFilePathValid(testPath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.Success, Is.EqualTo(isSuccess));
                
                if (!actualResult.Success)
                {
                    Assert.That(actualResult, Is.EqualTo(Match.Empty));
                }

                Assert.That(actualResult.Groups[Validate.PathGroup].Value, Is.EqualTo(expectedPath));
                Assert.That(actualResult.Groups[Validate.NameGroup].Value, Is.EqualTo(expectedName));
                Assert.That(actualResult.Groups[Validate.ExtensionGroup].Value, Is.EqualTo(expectedExtension));
            });
        }
        #endregion

        #region ContainsIllegalCharacters
        [TestCaseSource(nameof(TestInputs))]
        public void TestMethod_ContainsIllegalCharacters_ForInvalidInput_ReturnsExpectedResult_AndValue((string[] TextInputs, bool ExpectedResult, string ExpectedValue) test)
        {
            // Act
            bool actualResult = Validate.ContainsIllegalCharacters(test.TextInputs, out string actualValue);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.EqualTo(test.ExpectedResult));
                Assert.That(actualValue, Is.EqualTo(test.ExpectedValue));
            });
        }

        private static IEnumerable<(string[]?, bool, string)> TestInputs()
        {
            yield return (Array.Empty<string>(), false, "");
            yield return (null,                  false, "");

            // Invalid characters:  \/:*?""<>|
            yield return (new[] { @"n\ame" },    true,  @"n\ame");
            yield return (new[] { @"name/" },    true,  @"name/");
            yield return (new[] { @":name" },    true,  @":name");
            yield return (new[] { @"*name" },    true,  @"*name");
            yield return (new[] { @"name?" },    true,  @"name?");
            yield return (new[] { @"""name" },   true,  @"""name");
            yield return (new[] { @"nam<e" },    true,  @"nam<e");
            yield return (new[] { @"na>me" },    true,  @"na>me");
            yield return (new[] { @"na|me" },    true,  @"na|me");
        }
        #endregion
    }
}
