using FilesManager.Core.Validation;

namespace FilesManager.Core.UnitTests.Validation
{
    [TestFixture]
    internal sealed class ValidationTests
    {
        internal abstract class TestClass
        {
            public abstract void SuccessAction();

            public abstract void FailureAction();
        }

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
        public void IsFilePathValid_ForGivenInput_ReturnsExpectedMatch(string testPath, bool expectedIsSuccess, string expectedPath, string expectedName, string expectedExtension)
        {
            // Act
            bool actualIsSuccess = Validate.IsFilePathValid(testPath, out Match actualResult);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualIsSuccess, Is.EqualTo(expectedIsSuccess));
                
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

        #region IsUshort
        [Test]
        public void IsUshort_ForGivenValidNumber_ReturnsTrue_AndValue()
        {
            // Act
            bool actualResult = Validate.IsUshort("5", out ushort actualNumber);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.True);
                Assert.That(actualNumber, Is.EqualTo(5));
            });
        }

        [TestCase("-1", 0)]     // Negative
        [TestCase("99999", 0)]  // Too large
        public void IsUshort_ForGivenInvalidNumber_ReturnsFalse_AndValue(string textInput, int expectedNumber)
        {
            // Act
            bool actualResult = Validate.IsUshort(textInput, out ushort actualNumber);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(actualNumber, Is.EqualTo((ushort)expectedNumber));
            });
        }

        [TestCase("a", 0)]  // Lowercase letter
        [TestCase("Z", 0)]  // Uppercase letter
        [TestCase("+", 0)]  // Special character
        public void IsUshort_ForGivenInvalidValue_ReturnsFalse_AndValue(string textInput, int expectedNumber)
        {
            // Act
            bool actualResult = Validate.IsUshort(textInput, out ushort actualNumber);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(actualNumber, Is.EqualTo((ushort)expectedNumber));
            });
        }

        [Test]
        public void IsUshort_ForValidationSuccess_PassedSuccessMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.IsUshort("1", out _, successAction: mockedTestClass.Object.SuccessAction);

            // Assert
            mockedTestClass.Verify(mock => mock.SuccessAction(), Times.Once);
        }

        [Test]
        public void IsUshort_ForValidationFailure_PassedFailureMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.IsUshort("-1", out _, failureAction: mockedTestClass.Object.FailureAction);

            // Assert
            mockedTestClass.Verify(mock => mock.FailureAction(), Times.Once);
        }
        #endregion

        #region WithinLimit
        // sbyte + short
        [TestCase(1, (sbyte)1, (short)2, true)]
        [TestCase(2, (sbyte)2, (short)2, true)]
        [TestCase(3, (sbyte)3, (short)2, false)]
        // sbyte + int
        [TestCase(4, (sbyte)1, (int)2, true)]
        [TestCase(5, (sbyte)2, (int)2, true)]
        [TestCase(6, (sbyte)3, (int)2, false)]
        // sbyte + float
        [TestCase(7, (sbyte)1, (float)2, true)]
        [TestCase(8, (sbyte)2, (float)2, true)]
        [TestCase(9, (sbyte)3, (float)2, false)]
        // sbyte + double
        [TestCase(10, (sbyte)1, (double)2, true)]
        [TestCase(11, (sbyte)2, (double)2, true)]
        [TestCase(12, (sbyte)3, (double)2, false)]
        // byte + short
        [TestCase(13, (byte)1, (short)2, true)]
        [TestCase(14, (byte)2, (short)2, true)]
        [TestCase(15, (byte)3, (short)2, false)]
        // byte + ushort
        [TestCase(16, (byte)1, (ushort)2, true)]
        [TestCase(17, (byte)2, (ushort)2, true)]
        [TestCase(18, (byte)3, (ushort)2, false)]
        // byte + int
        [TestCase(19, (byte)1, (int)2, true)]
        [TestCase(20, (byte)2, (int)2, true)]
        [TestCase(21, (byte)3, (int)2, false)]
        // byte + uint
        [TestCase(22, (byte)1, (uint)2, true)]
        [TestCase(23, (byte)2, (uint)2, true)]
        [TestCase(24, (byte)3, (uint)2, false)]
        // byte + float
        [TestCase(25, (byte)1, (float)2, true)]
        [TestCase(26, (byte)2, (float)2, true)]
        [TestCase(27, (byte)3, (float)2, false)]
        // byte + double
        [TestCase(28, (byte)1, (double)2, true)]
        [TestCase(29, (byte)2, (double)2, true)]
        [TestCase(30, (byte)3, (double)2, false)]
        // short + int
        [TestCase(31, (short)1, (int)2, true)]
        [TestCase(32, (short)2, (int)2, true)]
        [TestCase(33, (short)3, (int)2, false)]
        // short + float
        [TestCase(34, (short)1, (float)2, true)]
        [TestCase(35, (short)2, (float)2, true)]
        [TestCase(36, (short)3, (float)2, false)]
        // short + double
        [TestCase(37, (short)1, (double)2, true)]
        [TestCase(38, (short)2, (double)2, true)]
        [TestCase(39, (short)3, (double)2, false)]
        // ushort + int
        [TestCase(40, (ushort)1, (int)2, true)]
        [TestCase(41, (ushort)2, (int)2, true)]
        [TestCase(42, (ushort)3, (int)2, false)]
        // ushort + uint
        [TestCase(43, (ushort)1, (uint)2, true)]
        [TestCase(44, (ushort)2, (uint)2, true)]
        [TestCase(45, (ushort)3, (uint)2, false)]
        // ushort + float
        [TestCase(46, (ushort)1, (float)2, true)]
        [TestCase(47, (ushort)2, (float)2, true)]
        [TestCase(48, (ushort)3, (float)2, false)]
        // ushort + double
        [TestCase(49, (ushort)1, (double)2, true)]
        [TestCase(50, (ushort)2, (double)2, true)]
        [TestCase(51, (ushort)3, (double)2, false)]
        // int + float
        [TestCase(52, (int)1, (float)2, true)]
        [TestCase(53, (int)2, (float)2, true)]
        [TestCase(54, (int)3, (float)2, false)]
        // int + double
        [TestCase(55, (int)1, (double)2, true)]
        [TestCase(56, (int)2, (double)2, true)]
        [TestCase(57, (int)3, (double)2, false)]
        // uint + float
        [TestCase(58, (uint)1, (float)2, true)]
        [TestCase(59, (uint)2, (float)2, true)]
        [TestCase(60, (uint)3, (float)2, false)]
        // uint + double
        [TestCase(61, (uint)1, (double)2, true)]
        [TestCase(62, (uint)2, (double)2, true)]
        [TestCase(63, (uint)3, (double)2, false)]
        // float + double
        [TestCase(64, (float)1, (double)2, true)]
        [TestCase(65, (float)2, (double)2, true)]
        [TestCase(66, (float)3, (double)2, false)]
        // Forbidden: sbyte + byte, sbyte + ushort, sbyte + uint, short + ushort, ushort + uint, int + uint
        public void WithinLimit_ForGivenValidNumbers_ReturnsTrue<TNumber>(int testId, TNumber testNumber1, TNumber testNumber2, bool expectedResult)
            where TNumber : struct, IComparable, IComparable<TNumber>, IConvertible, IEquatable<TNumber>, IFormattable  // NOTE: Numeric type
        {
            // Act
            bool actualResult = Validate.WithinLimit(testNumber1, testNumber2);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult), message: $"Test with id failed: {testId}");
        }

        [Test]
        public void WithinLimit_ForValidationSuccess_PassedSuccessMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.WithinLimit(1, 2, successAction: mockedTestClass.Object.SuccessAction);

            // Assert
            mockedTestClass.Verify(mock => mock.SuccessAction(), Times.Once);
        }

        [Test]
        public void WithinLimit_ForValidationFailure_PassedFailureMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.WithinLimit(2, 1, failureAction: mockedTestClass.Object.FailureAction);

            // Assert
            mockedTestClass.Verify(mock => mock.FailureAction(), Times.Once);
        }
        #endregion
    }
}
