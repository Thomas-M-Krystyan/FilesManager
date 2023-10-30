using FilesManager.Core.Extensions;

namespace FilesManager.Core.UnitTests.ExtensionMethods
{
    [TestFixture]
    internal sealed class StringExtensionsTests
    {
        [TestCase("",       "")]
        [TestCase(" ",      "")]
        [TestCase("   ",    "")]
        [TestCase("  *  ",  "  *  ")]
        [TestCase("test",   "test")]
        [TestCase(" test",  " test")]
        [TestCase("te  st", "te  st")]
        [TestCase("test ",  "test ")]
        public void TrimOnlyWhiteSpaces_ReturnsExpectedResult(string testValue, string expectedResult)
        {
            // Act
            string actualResult = testValue.TrimOnlyWhiteSpaces();

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase("",       true)]
        [TestCase(" ",      true)]
        [TestCase("   ",    true)]
        [TestCase("  *  ",  false)]
        [TestCase("test",   false)]
        [TestCase(" test",  false)]
        [TestCase("te  st", false)]
        [TestCase("test ",  false)]
        public void IsEmptyOrWhiteSpaces_ReturnsExpectedResult(string testValue, bool expectedResult)
        {
            // Act
            bool actualResult = testValue.IsEmptyOrWhiteSpaces();

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase("",       false)]
        [TestCase(" ",      true)]
        [TestCase("   ",    true)]
        [TestCase("  *  ",  false)]
        [TestCase("test",   false)]
        [TestCase(" test",  false)]
        [TestCase("te  st", false)]
        [TestCase("test ",  false)]
        public void IsOnlyWhiteSpaces_ReturnsExpectedResult(string testValue, bool expectedResult)
        {
            // Act
            bool actualResult = testValue.IsOnlyWhiteSpaces();

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
