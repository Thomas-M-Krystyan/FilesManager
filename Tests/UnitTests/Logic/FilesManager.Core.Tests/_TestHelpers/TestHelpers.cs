using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Validation;

namespace FilesManager.Core.UnitTests._TestHelpers
{
    internal static class TestHelpers
    {
        internal static Match GetTestMatch(string fullFilePath)
        {
            return RegexPatterns.FileComponentsPattern().Match(fullFilePath);
        }

        internal static FilePathNameDto GetTestDto(string testPath, string testName, string testExtension)
        {
            return new FilePathNameDto(testPath, testName, testExtension);
        }
    }
}
