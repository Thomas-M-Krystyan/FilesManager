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

            return FilePathConverter.GetFilePath(
                path: filePathMatch.Value(Validate.PathGroup),
                name: $"{textToPrepend.GetValueOrEmpty()}" +
                      $"{filePathMatch.Value(Validate.NameGroup)}" +
                      $"{textToAppend.GetValueOrEmpty()}",
                extension: filePathMatch.Value(Validate.ExtensionGroup));
        }
    }
}
