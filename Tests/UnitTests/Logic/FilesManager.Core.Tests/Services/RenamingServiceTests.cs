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
        public void ReplaceWithNumber_ForInvalidPath_ReturnsFailureDto(string invalidPath)
        {
            // Act
            RenamingResultDto actualResult = RenamingService.ReplaceWithNumber(invalidPath, "Pre", 9, "Post");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.IsSuccess, Is.False);
                Assert.That(actualResult.Message, Does.StartWith("Cannot rename the file"));
                Assert.That(actualResult.NewFilePath, Is.Empty);
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        public void EnrichWithPrependAndAppend_ForInvalidPath_ReturnsFailureDto(string invalidPath)
        {
            // Act
            RenamingResultDto actualResult = RenamingService.EnrichWithPrependAndAppend(invalidPath, "Prepend", "Append");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.IsSuccess, Is.False);
                Assert.That(actualResult.Message, Does.StartWith("Cannot rename the file"));
                Assert.That(actualResult.NewFilePath, Is.Empty);
            });
        }

        [TestCaseSource(nameof(LeadingZerosTestCases), new object[] { "C:/Users/Test" })]
        [TestCaseSource(nameof(LeadingZerosTestCases), new object[] { "" })]
        [TestCaseSource(nameof(LeadingZerosTestCases), new object[] { " " })]
        [TestCaseSource(nameof(LeadingZerosTestCases), new object?[] { null })]
        public void SetLeadingZeros_ForInvalidPath_ReturnsFailureDto((string OriginalPath, (string Path, string Zeros, string Digits, string Name, string Extension) Dto, string ExpectedException) test)
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

        private static IEnumerable<(string originalPath, (string path, string zeros, string digits, string name, string extension), string exception)> LeadingZerosTestCases(string filePath)
        {
            // DTO without name or extension
            yield return ($"{filePath}", ("", "00", "1", "Test", ""), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");
            yield return ($"{filePath}", ("C:/Users/", "00", "1", "Test", ""), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");
            yield return ($"{filePath}", ("", "00", "1", "Test", ".zip"), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");

            // DTO without zeroes or digits
            yield return ($"{filePath}", ("C:/Users/", "", "", "", ".jpg"), "The file name \"\" does not contain preceeding numeric part");
            yield return ($"{filePath}", ("C:/Users/", "", "", "Test", ".jpg"), "The file name \"Test\" does not contain preceeding numeric part");

            // Invalid source path
            yield return ($"{filePath}", ("C:/Users/", "00", "1", "Test", ".jpg"), $"Cannot rename the file \"{filePath}\"");
        }
    }
}
