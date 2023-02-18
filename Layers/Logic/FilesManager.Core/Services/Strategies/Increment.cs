using FilesManager.Core.Converters;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Services.Strategies
{
    internal static class Increment
    {
        internal static string GetNumberIncrementedName(string oldFilePath, string prefix, ushort startNumber, string postfix)
        {
            Match filePathMatch = Validate.IsFilePathValid(oldFilePath);

            return FilePathConverter.GetFilePath(
                path: filePathMatch.Value(Validate.PathGroup),
                name: $"{prefix.GetValueOrEmpty()}" +
                      $"{startNumber++}" +
                      $"{postfix.GetValueOrEmpty()}",
                extension: filePathMatch.Value(Validate.ExtensionGroup));
        }
    }
}
