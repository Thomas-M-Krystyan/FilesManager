﻿using FilesManager.Core.Extensions;

namespace FilesManager.Core.UnitTests.Helpers
{
    [TestFixture]
    internal sealed class HelperTests
    {
        [TestCaseSource(nameof(GetTestFileNames))]
        public void GetMaxLength_ForValidInput_ReturnsExpectedMaxFileLength((string[] Names, int ExpectedCount) testData)
        {
            // Act
            int result = testData.Names.GetMaxLength();

            // Assert
            Assert.That(result, Is.EqualTo(testData.ExpectedCount));
        }

        private static IEnumerable<(string[] Names, int ExpectedCount)> GetTestFileNames()
        {
            yield return (new[] { string.Empty                 }, 0);
            yield return (new[] { "test1"                      }, 5);
            yield return (new[] { "test1", "test25"            }, 6);
            yield return (new[] { "test1", "test25", "test300" }, 7);
        }
    }
}
