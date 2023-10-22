using FilesManager.Core.Validation;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Tests.Validation
{
    [TestFixture]
    internal class RegexPatternsTests
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
            Match match = RegexPatterns.InvalidCharactersPattern.Match(fileName);

            // Arrange
            Assert.That(match.Success, Is.EqualTo(isSuccess));
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
            Match match = RegexPatterns.DigitsNamePattern().Match(fileName);

            // Arrange
            Assert.Multiple(() =>
            {
                Assert.That(match.Success, Is.EqualTo(isSuccess));
                Assert.That(match.Groups[RegexPatterns.ZerosGroup].Value, Is.EqualTo(expectedZeros));
                Assert.That(match.Groups[RegexPatterns.DigitsGroup].Value, Is.EqualTo(expectedDigits));
                Assert.That(match.Groups[RegexPatterns.NameGroup].Value, Is.EqualTo(exetedName));
            });
        }
        #endregion
    }
}
