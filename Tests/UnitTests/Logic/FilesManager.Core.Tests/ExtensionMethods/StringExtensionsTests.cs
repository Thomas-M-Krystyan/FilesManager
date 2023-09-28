using FilesManager.Core.ExtensionMethods;
using NUnit.Framework;

namespace FilesManager.Core.Tests.ExtensionMethods
{
    [TestFixture]
    internal class StringExtensionsTests
    {
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("test", "test")]
        public void GetValueOrEmpty_ReturnsExpectedResult(string testText, string expectedResult)
        {
            // Act
            string actualResult = testText.GetValueOrEmpty();

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
