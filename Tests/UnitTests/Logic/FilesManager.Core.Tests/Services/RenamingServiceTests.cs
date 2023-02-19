using FileManager.Layers.Logic;
using FilesManager.Core.DTOs;
using NUnit.Framework;

namespace FilesManager.Core.Tests.Services
{
    [TestFixture]
    internal class RenamingServiceTests
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void TestMethod_ReplaceWithNumber_ForInvalidPath_ReturnsFailureDto(string invalidPath)
        {
            // Act
            RenamingResultDto actualResult = RenamingService.ReplaceWithNumber(invalidPath, "Pre", 9, "Post");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.IsSuccess, Is.False);
                Assert.That(actualResult.Message.StartsWith("Cannot rename the file"), Is.True);
                Assert.That(actualResult.NewFilePath, Is.Empty);
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void TestMethod_EnrichWithPrependAndAppend_ForInvalidPath_ReturnsFailureDto(string invalidPath)
        {
            // Act
            RenamingResultDto actualResult = RenamingService.EnrichWithPrependAndAppend(invalidPath, "Prepend", "Append");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.IsSuccess, Is.False);
                Assert.That(actualResult.Message.StartsWith("Cannot rename the file"), Is.True);
                Assert.That(actualResult.NewFilePath, Is.Empty);
            });
        }

        [TestCaseSource(nameof(LeadingZerosTestCases), new object[] { "C:/Users/Test" })]
        [TestCaseSource(nameof(LeadingZerosTestCases), new object[] { "" })]
        [TestCaseSource(nameof(LeadingZerosTestCases), new object[] { " " })]
        [TestCaseSource(nameof(LeadingZerosTestCases), new object?[] { null })]
        public void TestMethod_SetLeadingZeros_ForInvalidPath_ReturnsFailureDto((string OriginalPath, (string Path, string Zeros, string Digits, string Name, string Extension) Dto, string ExpectedException) test)
        {
            // Act
            RenamingResultDto actualResult = RenamingService.SetLeadingZeros(
                test.OriginalPath,
                new PathZerosDigitsExtensionDto(test.Dto.Path, test.Dto.Zeros, test.Dto.Digits, test.Dto.Name, test.Dto.Extension),
                5,
                9);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.IsSuccess, Is.False);
                Assert.That(actualResult.Message, Is.EqualTo(test.ExpectedException));
                Assert.That(actualResult.NewFilePath, Is.Empty);
            });
        }

        public static IEnumerable<(string originalPath, (string path, string zeros, string digits, string name, string extension), string exception)> LeadingZerosTestCases(string originalPath)
        {
            yield return ($"{originalPath}", ("", "00", "1", "Test", ""), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");
            yield return ($"{originalPath}", ("C:/Users/", "00", "1", "Test", ""), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");
            yield return ($"{originalPath}", ("", "00", "1", "Test", ".zip"), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");

            yield return ($"{originalPath}", ("C:/Users/", "", "", "", ".jpg"), "The file name \"\" does not contain preceeding numeric part");
            yield return ($"{originalPath}", ("C:/Users/", "", "", "Test", ".jpg"), "The file name \"Test\" does not contain preceeding numeric part");

            yield return ($"{originalPath}", ("C:/Users/", "00", "1", "Test", ".jpg"), $"Cannot rename the file \"{originalPath}\"");
        }
    }
}
