namespace FilesManager.Core.UnitTests.Services.Renaming
{
    [TestFixture]
    internal sealed class RenamingServiceTests
    {
        //[TestCase("")]
        //[TestCase(" ")]
        //public void ReplaceWithNumber_ForInvalidPath_ReturnsFailureDto(string invalidPath)
        //{
        //    // Arrange
        //    StrategyBase strategy = new IncrementNumberViewModel();

        //    // Act
        //    RenamingResultDto actualResult = RenamingService.ReplaceWithNumber(invalidPath, "Pre", 9, "Post");

        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(actualResult.IsSuccess, Is.False);
        //        Assert.That(actualResult.Message, Does.StartWith(Resources.ERROR_Validation_File_NotRenamed));
        //        Assert.That(actualResult.NewFilePath, Is.Empty);
        //    });
        //}

        //[TestCase("")]
        //[TestCase(" ")]
        //public void UpdateWithPrependAndAppend_ForInvalidPath_ReturnsFailureDto(string invalidPath)
        //{
        //    // Act
        //    RenamingResultDto actualResult = RenamingService.UpdateWithPrependAndAppend(invalidPath, "Prepend", "Append");

        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(actualResult.IsSuccess, Is.False);
        //        Assert.That(actualResult.Message, Does.StartWith(Resources.ERROR_Validation_File_NotRenamed));
        //        Assert.That(actualResult.NewFilePath, Is.Empty);
        //    });
        //}

        //[TestCaseSource(nameof(LeadingZerosTestCases), new object[] { "C:/Users/Test" })]
        //[TestCaseSource(nameof(LeadingZerosTestCases), new object[] { "" })]
        //[TestCaseSource(nameof(LeadingZerosTestCases), new object[] { " " })]
        //[TestCaseSource(nameof(LeadingZerosTestCases), new object?[] { null })]
        //public void SetLeadingZeros_ForInvalidPath_ReturnsFailureDto(TestData test)
        //{
        //    // Act
        //    RenamingResultDto actualResult = RenamingService.SetLeadingZeros(
        //        test.OriginalPath,
        //        new PathZerosDigitsExtensionDto(test.File.Path, test.File.Zeros, test.File.Digits, test.File.Name, test.File.Extension),
        //        5,
        //        9);

        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(actualResult.IsSuccess, Is.False);
        //        Assert.That(actualResult.Message, Does.StartWith(test.Exception));
        //        Assert.That(actualResult.NewFilePath, Is.Empty);
        //    });
        //}

        //private static IEnumerable<TestData> LeadingZerosTestCases(string filePath)
        //{
        //    // DTO without name or extension
        //    yield return new($"{filePath}", new("", "00", "1", "Test", ""), Resources.ERROR_Validation_File_NotRenamed);
        //    yield return new($"{filePath}", new("C:/Users/", "00", "1", "Test", ""), Resources.ERROR_Validation_File_NotRenamed);
        //    yield return new($"{filePath}", new("", "00", "1", "Test", ".zip"), Resources.ERROR_Validation_File_NotRenamed);

        //    // DTO without zeroes or digits
        //    yield return new($"{filePath}", new("C:/Users/", "", "", "", ".jpg"), Resources.ERROR_Validation_FileName_HasNoPreceedingNumber);
        //    yield return new($"{filePath}", new("C:/Users/", "", "", "Test", ".jpg"), Resources.ERROR_Validation_FileName_HasNoPreceedingNumber);

        //    // Invalid source path
        //    yield return new($"{filePath}", new("C:/Users/", "00", "1", "Test", ".jpg"), Resources.ERROR_Validation_File_NotRenamed);
        //}

        //internal sealed record TestData(string OriginalPath, TestFileData File, string Exception);

        //internal sealed record TestFileData(string Path, string Zeros, string Digits, string Name, string Extension);
    }
}
