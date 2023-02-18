using FilesManager.Core.DTOs;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Converters
{
    /// <summary>
    /// Converts back and forth data associated with file <see cref="Path"/>.
    /// </summary>
    public static class FilePathConverter
    {
        #region Path + Name + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, name, and extension.
        /// </summary>
        /// <returns>Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.</returns>
        public static PathNameExtensionDto GetPathNameExtension(string filePath)
        {
            Match filePathMatch = RegexPatterns.FilePathPattern.Match(filePath);

            return filePathMatch.Success
                ? new PathNameExtensionDto(path: filePathMatch.Value(RegexPatterns.PathGroup),
                                           name: filePathMatch.Value(RegexPatterns.NameGroup),
                                           extension: filePathMatch.Value(RegexPatterns.ExtensionGroup))
                : PathNameExtensionDto.Empty;
        }
        #endregion

        #region Path + Zeros + Digits + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, zeros, digits, and extension.
        /// </summary>
        /// <returns>Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.</returns>
        public static PathZerosDigitsExtensionDto GetPathZerosDigitsExtension(string filePath)
        {
            Match filePathMatch = RegexPatterns.FilePathPattern.Match(filePath);

            if (filePathMatch.Success)
            {
                string path = filePathMatch.Value(RegexPatterns.PathGroup);
                string name = filePathMatch.Value(RegexPatterns.NameGroup);
                string extension = filePathMatch.Value(RegexPatterns.ExtensionGroup);

                Match digitsNameMatch = RegexPatterns.DigitsNamePattern.Match(name);

                if (digitsNameMatch.Success)
                {
                    string zeros = digitsNameMatch.Value(RegexPatterns.ZerosGroup);
                    string digits = digitsNameMatch.Value(RegexPatterns.DigitsGroup);
                    name = digitsNameMatch.Value(RegexPatterns.NameGroup);

                    return new PathZerosDigitsExtensionDto(path, zeros, digits, name, extension);
                }
            }

            return PathZerosDigitsExtensionDto.Empty;
        }
        #endregion

        /// <summary>
        /// Converts path + name + extension back into a consolidated file <see cref="Path"/>.
        /// </summary>
        public static string GetFilePath(string path, string name, string extension)
        {
            return Path.Combine(path, name + extension);
        }
    }
}
