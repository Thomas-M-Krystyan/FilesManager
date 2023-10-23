using FilesManager.Core.Validation;

namespace FilesManager.UI.Desktop.UnitTests._TestHelpers
{
    internal static class TestHelpers
    {
        internal static Match GetMockedMatch(string testPath, string testName, string testExtension)
        {
            return RegexPatterns.FileComponentsPattern().Match($"{testPath}{testName}{testExtension}");
        }
    }
}
