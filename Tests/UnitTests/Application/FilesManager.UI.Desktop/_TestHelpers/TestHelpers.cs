using FilesManager.Core.Converters;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Validation;

namespace FilesManager.UI.Desktop.UnitTests._TestHelpers
{
    internal static class TestHelpers
    {
        internal static PathZerosDigitsExtensionDto GetMockedDto(string testPath, string testName, string testExtension)
        {
            return RegexPatterns.FileComponentsPattern().Match($"{testPath}{testName}{testExtension}")
                                .GetPathZerosDigitsExtensionDto();
        }
    }
}
