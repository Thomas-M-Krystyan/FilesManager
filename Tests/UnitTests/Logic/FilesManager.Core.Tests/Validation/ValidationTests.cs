using FilesManager.Core.Validation;
using Moq;
using NUnit.Framework;
using Match = System.Text.RegularExpressions.Match;

namespace FilesManager.Core.Tests.Validation
{
    [TestFixture]
    public class ValidationTests
    {
        #region IsFilePathValid
        // Missing path
        [TestCase("File", false, "", "", "")]
        [TestCase("File.", false, "", "", "")]
        [TestCase("File.h", false, "", "", "")]
        [TestCase("File.7z", false, "", "", "")]
        [TestCase("File.exe", false, "", "", "")]
        [TestCase("File.jpeg", false, "", "", "")]
        [TestCase("File.xhtml", false, "", "", "")]
        [TestCase("File.cshtml", false, "", "", "")]
        [TestCase("File.extreme", false, "", "", "")]
        // Missing name
        [TestCase(@"C:\Folder", false, "", "", "")]
        [TestCase(@"C:\Folder\", false, "", "", "")]
        [TestCase(@"C:\Folder\.", false, "", "", "")]
        [TestCase(@"C:\Folder\.h", false, "", "", "")]
        [TestCase(@"C:\Folder\.7z", false, "", "", "")]
        [TestCase(@"C:\Folder\.exe", false, "", "", "")]
        [TestCase(@"C:\Folder\.jpeg", false, "", "", "")]
        [TestCase(@"C:\Folder\.xhtml", false, "", "", "")]
        [TestCase(@"C:\Folder\.cshtml", false, "", "", "")]
        [TestCase(@"C:\Folder\.extreme", false, "", "", "")]
        // Full path
        [TestCase(@"C:\Folder\File", false, "", "", "")]  // Without extension
        [TestCase(@"C:\Folder\File.", false, "", "", "")]  // Without extension
        [TestCase(@"C:\Folder\File.h", true, "C:\\Folder\\", "File", ".h")]
        [TestCase(@"C:\Folder\File.7z", true, "C:\\Folder\\", "File", ".7z")]
        [TestCase(@"C:\Folder\File.exe", true, "C:\\Folder\\", "File", ".exe")]
        [TestCase(@"C:\Folder\File.jpeg", true, "C:\\Folder\\", "File", ".jpeg")]
        [TestCase(@"C:\Folder\File.xhtml", true, "C:\\Folder\\", "File", ".xhtml")]
        [TestCase(@"C:\Folder\File.cshtml", true, "C:\\Folder\\", "File", ".cshtml")]
        [TestCase(@"C:\Folder\File.extreme", false, "", "", "")]  // Too long
        [TestCase(@"C:\Folder\Is Me.jpeg", true, "C:\\Folder\\", "Is Me", ".jpeg")]
        [TestCase(@"C:\Folder\Is-Me.jpeg", true, "C:\\Folder\\", "Is-Me", ".jpeg")]
        [TestCase(@"C:\Folder\Is_Me.jpeg", true, "C:\\Folder\\", "Is_Me", ".jpeg")]
        [TestCase(@"C:\Folder\Is.Me.jpeg", true, "C:\\Folder\\", "Is.Me", ".jpeg")]
        [TestCase(@"C:\Folder\num1.jpeg", true, "C:\\Folder\\", "num1", ".jpeg")]
        [TestCase(@"C:\Folder\Is.png.jpeg", true, "C:\\Folder\\", "Is.png", ".jpeg")]
        [TestCase(@"C:\Folder\Is.Gr3go_r-20.jpeg", true, "C:\\Folder\\", "Is.Gr3go_r-20", ".jpeg")]
        public void IsFilePathValid_ForGivenInput_ReturnsExpectedMatch(string testPath, bool isSuccess, string expectedPath, string expectedName, string expectedExtension)
        {
            // Act
            Match actualResult = Validate.IsFilePathValid(testPath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.Success, Is.EqualTo(isSuccess));
                
                if (actualResult.Success)
                {
                    Assert.That(actualResult.Groups[RegexPatterns.PathGroup].Value, Is.EqualTo(expectedPath));
                    Assert.That(actualResult.Groups[RegexPatterns.NameGroup].Value, Is.EqualTo(expectedName));
                    Assert.That(actualResult.Groups[RegexPatterns.ExtensionGroup].Value, Is.EqualTo(expectedExtension));
                }
                else
                {
                    Assert.That(actualResult, Is.EqualTo(Match.Empty));
                }
            });
        }
        #endregion

        #region ContainInvalidCharacters
        [TestCaseSource(nameof(TestInputs))]
        public void ContainInvalidCharacters_ForGivenInput_ReturnsExpectedResult_AndValue((string TextInput, bool ExpectedResult) test)
        {
            // Act
            bool actualResult = Validate.ContainInvalidCharacters(test.TextInput);

            // Assert
            Assert.That(actualResult, Is.EqualTo(test.ExpectedResult));
        }

        private static IEnumerable<(string, bool)> TestInputs()
        {
            yield return ("", false);
            yield return ("All good", false);

            // Invalid characters:  \/:*?""<>|
            yield return (@"n\ame", true);
            yield return (@"name/", true);
            yield return (@"nam:e", true);
            yield return (@"*name", true);
            yield return (@"name?", true);
            yield return (@"""name", true);
            yield return (@"<name", true);
            yield return (@"na>me", true);
            yield return (@"na|me", true);
        }

        internal abstract class TestClass
        {
            public abstract void SuccessAction();

            public abstract void FailureAction();
        }

        [Test]
        public void ContainInvalidCharacters_ForValidationFailure_PassedFailureMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.ContainInvalidCharacters(">Invalid<", failureAction: mockedTestClass.Object.FailureAction);

            // Assert
            mockedTestClass.Verify(mock => mock.FailureAction(), Times.Once);
        }

        [Test]
        public void ContainInvalidCharacters_ForValidationSuccess_PassedSuccessMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.ContainInvalidCharacters("Valid", successAction: mockedTestClass.Object.SuccessAction);

            // Assert
            mockedTestClass.Verify(mock => mock.SuccessAction(), Times.Once);
        }
        #endregion
    }
}
