using FilesManager.Core.ExtensionMethods;

namespace FilesManager.Core.UnitTests.ExtensionMethods
{
    [TestFixture]
    internal class StringExtensionsTests
    {
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("test", "test")]
        public void GetValueOrEmpty_ReturnsExpectedResult(string testValue, string expectedResult)
        {
            // Act
            string actualResult = testValue.GetValueOrEmpty();

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
