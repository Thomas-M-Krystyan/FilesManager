using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Validation;
using FilesManager.UI.Common.Properties;
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
        /// <returns>
        ///   Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.
        /// </returns>
        public static PathNameExtensionDto GetPathNameExtension(string filePath)
        {
            // NOTE: Split the file path into path, name, and extension groups
            Match filePathMatch = RegexPatterns.FileComponentsPattern().Match(filePath);

            return filePathMatch.Success
                ? new PathNameExtensionDto(path: filePathMatch.Value(RegexPatterns.PathGroup),
                                           name: filePathMatch.Value(RegexPatterns.NameGroup),
                                           extension: filePathMatch.Value(RegexPatterns.ExtensionGroup))
                : throw new InvalidOperationException(Resources.ERROR_Internal_InvalidFilePathDto + $" \"{filePath}\"");
        }
        #endregion

        #region Path + Zeros + Digits + Name + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, zeros, digits, name, and extension.
        /// </summary>
        /// <returns>
        ///   Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.
        /// </returns>
        public static PathZerosDigitsExtensionDto GetPathZerosDigitsExtension(string filePath)
        {
            // NOTE: Split the file path into path, name, and extension groups
            Match fileComponentsMatch = RegexPatterns.FileComponentsPattern().Match(filePath);

            if (fileComponentsMatch.Success)
            {
                // NOTE: Split the file name into dedicated zeros, digits, and name groups
                Match digitsNameMatch = RegexPatterns.DigitsNamePattern().Match(
                    fileComponentsMatch.Value(RegexPatterns.NameGroup));

                if (digitsNameMatch.Success)
                {
                    return new PathZerosDigitsExtensionDto(
                        path: fileComponentsMatch.Value(RegexPatterns.PathGroup),
                        zeros: digitsNameMatch.Value(RegexPatterns.ZerosGroup),
                        digits: digitsNameMatch.Value(RegexPatterns.DigitsGroup),
                        name: digitsNameMatch.Value(RegexPatterns.NameGroup),
                        extension: fileComponentsMatch.Value(RegexPatterns.ExtensionGroup));
                }
            }
            
            throw new InvalidOperationException(Resources.ERROR_Internal_InvalidFilePathDto + $" \"{filePath}\"");
        }
        #endregion

        /// <summary>
        /// Converts path + name + extension back into a consolidated file's <see cref="Path"/>.
        /// </summary>
        public static string GetFilePath(string path, string name, string extension)
        {
            return Path.Combine(path, name + extension);
        }
    }
}
