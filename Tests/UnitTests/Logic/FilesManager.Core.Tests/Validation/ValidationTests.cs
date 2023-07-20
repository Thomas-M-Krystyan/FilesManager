using FilesManager.Core.Validation;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Tests.Validation
{
    [TestFixture]
    public class ValidationTests
    {
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
        public void HasValidExtension_ForGivenInput_ReturnsExpectedResult(string filePath, bool expectedResult)
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
        public void IsFilePathValid_ForValidInput_ReturnsExpectedResult_AndMatch(string testPath, bool isSuccess, string expectedPath, string expectedName, string expectedExtension)
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

                Assert.That(actualResult.Groups[RegexPatterns.PathGroup].Value, Is.EqualTo(expectedPath));
                Assert.That(actualResult.Groups[RegexPatterns.NameGroup].Value, Is.EqualTo(expectedName));
                Assert.That(actualResult.Groups[RegexPatterns.ExtensionGroup].Value, Is.EqualTo(expectedExtension));
            });
        }
        #endregion

        #region ContainsIllegalCharacters
        [TestCaseSource(nameof(TestInputs))]
        public void ContainsIllegalCharacters_ForInvalidInput_ReturnsExpectedResult_AndValue((string[] TextInputs, bool ExpectedResult, string ExpectedValue) test)
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
