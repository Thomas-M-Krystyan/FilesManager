using FilesManager.Core.Converters;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Services.Strategies
{
    internal static class PrependAppend
    {
        internal static string GetPrependedAndAppendedName(string oldFilePath, string textToPrepend, string textToAppend)
        {
            Match filePathMatch = Validate.IsFilePathValid(oldFilePath);

            return !filePathMatch.Success
                ? string.Empty
                : FilePathConverter.GetFilePath(
                    path: filePathMatch.Value(RegexPatterns.PathGroup),
                    name: $"{textToPrepend.GetValueOrEmpty()}" +
                          $"{filePathMatch.Value(RegexPatterns.NameGroup)}" +
                          $"{textToAppend.GetValueOrEmpty()}",
                    extension: filePathMatch.Value(RegexPatterns.ExtensionGroup));
        }
    }
}
