using FileManager.Layers.Logic;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using NUnit.Framework;

namespace FilesManager.Core.UnitTests.Services
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
        public void SetLeadingZeros_ForInvalidPath_ReturnsFailureDto(TestData test)
        {
            // Act
            RenamingResultDto actualResult = RenamingService.SetLeadingZeros(
                test.OriginalPath,
                new PathZerosDigitsExtensionDto(test.File.Path, test.File.Zeros, test.File.Digits, test.File.Name, test.File.Extension),
                5,
                9);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult.IsSuccess, Is.False);
                Assert.That(actualResult.Message, Is.EqualTo(test.Exception));
                Assert.That(actualResult.NewFilePath, Is.Empty);
            });
        }

        private static IEnumerable<TestData> LeadingZerosTestCases(string filePath)
        {
            // DTO without name or extension
            yield return new($"{filePath}", new("", "00", "1", "Test", ""), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");
            yield return new($"{filePath}", new("C:/Users/", "00", "1", "Test", ""), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");
            yield return new($"{filePath}", new("", "00", "1", "Test", ".zip"), "Internal (RegEx) error: The file \"001Test\" was't parsed properly");

            // DTO without zeroes or digits
            yield return new($"{filePath}", new("C:/Users/", "", "", "", ".jpg"), "The file name \"\" does not contain preceeding numeric part");
            yield return new($"{filePath}", new("C:/Users/", "", "", "Test", ".jpg"), "The file name \"Test\" does not contain preceeding numeric part");

            // Invalid source path
            yield return new($"{filePath}", new("C:/Users/", "00", "1", "Test", ".jpg"), $"Cannot rename the file \"{filePath}\"");
        }

        internal sealed record TestData(string OriginalPath, TestFileData File, string Exception);

        internal sealed record TestFileData(string Path, string Zeros, string Digits, string Name, string Extension);
    }
}
