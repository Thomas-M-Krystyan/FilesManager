using FilesManager.Core.Validation;

namespace FilesManager.Core.UnitTests.Validation
{
    [TestFixture]
    internal sealed class ValidationTests
    {
        internal abstract class TestClass
        {
            internal abstract void SuccessAction();

            internal abstract void FailureAction();
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

        #region Is<ushort>(out ushort)
        [Test]
        public void Is_Ushort_ForGivenValidNumber_ReturnsTrue_AndValue()
        {
            // Act
            bool actualResult = Validate.Is("5", out ushort actualNumber);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.True);
                Assert.That(actualNumber, Is.EqualTo(5));
            });
        }

        [TestCase("-1", 0)]     // Negative
        [TestCase("99999", 0)]  // Too large
        public void Is_Ushort_ForGivenInvalidNumber_ReturnsFalse_AndValue(string textInput, int expectedNumber)
        {
            // Act
            bool actualResult = Validate.Is(textInput, out ushort actualNumber);

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
        public void Is_Ushort_ForGivenInvalidValue_ReturnsFalse_AndValue(string textInput, int expectedNumber)
        {
            // Act
            bool actualResult = Validate.Is(textInput, out ushort actualNumber);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(actualNumber, Is.EqualTo((ushort)expectedNumber));
            });
        }

        [Test]
        public void Is_Ushort_ForValidationSuccess_PassedSuccessMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.Is("1", out ushort _, successAction: mockedTestClass.Object.SuccessAction);

            // Assert
            mockedTestClass.Verify(mock => mock.SuccessAction(), Times.Once);
        }

        [Test]
        public void Is_Ushort_ForValidationFailure_PassedFailureMethodIsExecuted()
        {
            // Arrange
            var mockedTestClass = new Mock<TestClass>();

            // Act
            _ = Validate.Is("-1", out ushort _, failureAction: mockedTestClass.Object.FailureAction);

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
        // sbyte + long
        [TestCase(7, (sbyte)1, (long)2, true)]
        [TestCase(8, (sbyte)2, (long)2, true)]
        [TestCase(9, (sbyte)3, (long)2, false)]
        // sbyte + float
        [TestCase(10, (sbyte)1, (float)2, true)]
        [TestCase(11, (sbyte)2, (float)2, true)]
        [TestCase(12, (sbyte)3, (float)2, false)]
        // sbyte + double
        [TestCase(13, (sbyte)1, (double)2, true)]
        [TestCase(14, (sbyte)2, (double)2, true)]
        [TestCase(15, (sbyte)3, (double)2, false)]
        // byte + short
        [TestCase(16, (byte)1, (short)2, true)]
        [TestCase(17, (byte)2, (short)2, true)]
        [TestCase(18, (byte)3, (short)2, false)]
        // byte + ushort
        [TestCase(19, (byte)1, (ushort)2, true)]
        [TestCase(20, (byte)2, (ushort)2, true)]
        [TestCase(21, (byte)3, (ushort)2, false)]
        // byte + int
        [TestCase(22, (byte)1, (int)2, true)]
        [TestCase(23, (byte)2, (int)2, true)]
        [TestCase(24, (byte)3, (int)2, false)]
        // byte + uint
        [TestCase(25, (byte)1, (uint)2, true)]
        [TestCase(26, (byte)2, (uint)2, true)]
        [TestCase(27, (byte)3, (uint)2, false)]
        // byte + long
        [TestCase(28, (byte)1, (long)2, true)]
        [TestCase(29, (byte)2, (long)2, true)]
        [TestCase(30, (byte)3, (long)2, false)]
        // byte + ulong
        [TestCase(31, (byte)1, (ulong)2, true)]
        [TestCase(32, (byte)2, (ulong)2, true)]
        [TestCase(33, (byte)3, (ulong)2, false)]
        // byte + float
        [TestCase(34, (byte)1, (float)2, true)]
        [TestCase(35, (byte)2, (float)2, true)]
        [TestCase(36, (byte)3, (float)2, false)]
        // byte + double
        [TestCase(37, (byte)1, (double)2, true)]
        [TestCase(38, (byte)2, (double)2, true)]
        [TestCase(39, (byte)3, (double)2, false)]
        // short + int
        [TestCase(40, (short)1, (int)2, true)]
        [TestCase(41, (short)2, (int)2, true)]
        [TestCase(42, (short)3, (int)2, false)]
        // short + long
        [TestCase(43, (short)1, (long)2, true)]
        [TestCase(44, (short)2, (long)2, true)]
        [TestCase(45, (short)3, (long)2, false)]
        // short + float
        [TestCase(46, (short)1, (float)2, true)]
        [TestCase(47, (short)2, (float)2, true)]
        [TestCase(48, (short)3, (float)2, false)]
        // short + double
        [TestCase(49, (short)1, (double)2, true)]
        [TestCase(50, (short)2, (double)2, true)]
        [TestCase(51, (short)3, (double)2, false)]
        // ushort + int
        [TestCase(52, (ushort)1, (int)2, true)]
        [TestCase(53, (ushort)2, (int)2, true)]
        [TestCase(54, (ushort)3, (int)2, false)]
        // ushort + uint
        [TestCase(55, (ushort)1, (uint)2, true)]
        [TestCase(56, (ushort)2, (uint)2, true)]
        [TestCase(57, (ushort)3, (uint)2, false)]
        // ushort + long
        [TestCase(58, (ushort)1, (long)2, true)]
        [TestCase(59, (ushort)2, (long)2, true)]
        [TestCase(60, (ushort)3, (long)2, false)]
        // ushort + ulong
        [TestCase(61, (ushort)1, (ulong)2, true)]
        [TestCase(62, (ushort)2, (ulong)2, true)]
        [TestCase(63, (ushort)3, (ulong)2, false)]
        // ushort + float
        [TestCase(64, (ushort)1, (float)2, true)]
        [TestCase(65, (ushort)2, (float)2, true)]
        [TestCase(66, (ushort)3, (float)2, false)]
        // ushort + double
        [TestCase(67, (ushort)1, (double)2, true)]
        [TestCase(68, (ushort)2, (double)2, true)]
        [TestCase(69, (ushort)3, (double)2, false)]
        // int + long
        [TestCase(70, (int)1, (long)2, true)]
        [TestCase(71, (int)2, (long)2, true)]
        [TestCase(72, (int)3, (long)2, false)]
        // int + float
        [TestCase(73, (int)1, (float)2, true)]
        [TestCase(74, (int)2, (float)2, true)]
        [TestCase(75, (int)3, (float)2, false)]
        // int + double
        [TestCase(76, (int)1, (double)2, true)]
        [TestCase(77, (int)2, (double)2, true)]
        [TestCase(78, (int)3, (double)2, false)]
        // uint + long
        [TestCase(79, (uint)1, (long)2, true)]
        [TestCase(80, (uint)2, (long)2, true)]
        [TestCase(81, (uint)3, (long)2, false)]
        // uint + ulong
        [TestCase(82, (uint)1, (ulong)2, true)]
        [TestCase(83, (uint)2, (ulong)2, true)]
        [TestCase(84, (uint)3, (ulong)2, false)]
        // uint + float
        [TestCase(85, (uint)1, (float)2, true)]
        [TestCase(86, (uint)2, (float)2, true)]
        [TestCase(87, (uint)3, (float)2, false)]
        // uint + double
        [TestCase(88, (uint)1, (double)2, true)]
        [TestCase(89, (uint)2, (double)2, true)]
        [TestCase(90, (uint)3, (double)2, false)]
        // long + float
        [TestCase(91, (long)1, (float)2, true)]
        [TestCase(92, (long)2, (float)2, true)]
        [TestCase(93, (long)3, (float)2, false)]
        // long + double
        [TestCase(94, (long)1, (double)2, true)]
        [TestCase(95, (long)2, (double)2, true)]
        [TestCase(96, (long)3, (double)2, false)]
        // float + double
        [TestCase(97, (float)1, (double)2, true)]
        [TestCase(98, (float)2, (double)2, true)]
        [TestCase(99, (float)3, (double)2, false)]
        // Forbidden: sbyte + byte, sbyte + ushort, sbyte + uint, sbyte + ulong, short + ushort, short + ulong,
        //            ushort + uint, int + uint, int + ulong, long + ulong, anything with decimal
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
