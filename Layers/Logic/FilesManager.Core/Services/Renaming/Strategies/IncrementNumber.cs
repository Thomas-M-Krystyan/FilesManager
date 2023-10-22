using FilesManager.Core.Converters;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Services.Renaming.Strategies
{
    internal static class IncrementNumber
    {
        internal static string GetNumberIncrementedName(string oldFilePath, string prefix, ushort startNumber, string postfix)
        {
            Match filePathMatch = Validate.IsFilePathValid(oldFilePath);

            return !filePathMatch.Success
                ? string.Empty
                : FilePathConverter.GetFilePath(
                    path: filePathMatch.Value(RegexPatterns.PathGroup),
                    name: $"{prefix.GetValueOrEmpty()}" +
                          $"{startNumber++}" +
                          $"{postfix.GetValueOrEmpty()}",
                    extension: filePathMatch.Value(RegexPatterns.ExtensionGroup));
        }
    }
}
