using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Validation;
using FilesManager.UI.Common.Properties;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Converters
{
    /// <summary>
    /// Converts back and forth data associated with file <see cref="Path"/>.
    /// </summary>
    internal static class FilePathConverter
    {
        #region Path + Name + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, name, and extension.
        /// </summary>
        /// <param name="filePathMatch">The file path RegEx match.</param>
        /// <returns>
        ///   Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.
        /// </returns>
        /// <exception cref="InvalidOperationException"/>
        internal static PathNameExtensionDto GetPathNameExtension(Match filePathMatch)
        {
            return filePathMatch.Success
                ? new PathNameExtensionDto(path: filePathMatch.Value(RegexPatterns.PathGroup),
                                           name: filePathMatch.Value(RegexPatterns.NameGroup),
                                           extension: filePathMatch.Value(RegexPatterns.ExtensionGroup))
                : throw new InvalidOperationException(Resources.ERROR_Internal_InvalidFilePathDto);
        }
        #endregion

        #region Path + Zeros + Digits + Name + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, zeros, digits, name, and extension.
        /// </summary>
        /// <param name="filePathMatch">The file path RegEx match.</param>
        /// <returns>
        ///   Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.
        /// </returns>
        /// <exception cref="InvalidOperationException"/>
        internal static PathZerosDigitsExtensionDto GetPathZerosDigitsExtension(Match filePathMatch)
        {
            // NOTE: Split the file name into dedicated zeros, digits, and name groups
            Match digitsNameMatch = RegexPatterns.DigitsNamePattern().Match(
                filePathMatch.Value(RegexPatterns.NameGroup));

            return digitsNameMatch.Success
                ? new PathZerosDigitsExtensionDto(
                    path: filePathMatch.Value(RegexPatterns.PathGroup),
                    zeros: digitsNameMatch.Value(RegexPatterns.ZerosGroup),
                    digits: digitsNameMatch.Value(RegexPatterns.DigitsGroup),
                    name: digitsNameMatch.Value(RegexPatterns.NameGroup),
                    extension: filePathMatch.Value(RegexPatterns.ExtensionGroup))
                : throw new InvalidOperationException(Resources.ERROR_Internal_InvalidFilePathDto);
        }
        #endregion

        /// <summary>
        /// Converts path + name + extension back into a consolidated file's <see cref="Path"/>.
        /// </summary>
        internal static string GetFilePath(string path, string name, string extension)
        {
            if (string.IsNullOrEmpty(path) ||
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(extension))
            {
                return string.Empty;
            }

            return Path.Combine(path, name + extension);
        }
    }
}
