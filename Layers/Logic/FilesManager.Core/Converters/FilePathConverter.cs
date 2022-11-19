using FilesManager.Core.DTOs;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Converters
{
    /// <summary>
    /// Converts back and forth data associated with file <see cref="Path"/>.
    /// </summary>
    public class FilePathConverter
    {
        #region Path + Name + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, name, and extension.
        /// </summary>
        /// <returns>Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.</returns>
        public static PathNameExtensionDto GetPathNameExtension(string filePath)
        {
            Match filePathMatch = Regex.Match(filePath, Validate.FilePathPattern);

            return filePathMatch.Success
                ? new PathNameExtensionDto(path: filePathMatch.Value(Validate.PathGroup),
                                           name: filePathMatch.Value(Validate.NameGroup),
                                           extension: filePathMatch.Value(Validate.ExtensionGroup))
                : PathNameExtensionDto.Empty;
        }

        /// <summary>
        /// Converts path + name + extension back into a consolidated file <see cref="Path"/>.
        /// </summary>
        public static string GetFilePath(string path, string name, string extension)
        {
            return Path.Combine(path, name + extension);
        }
        #endregion

        #region Path + Zeros + Digits + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, zeros, digits, and extension.
        /// </summary>
        /// <returns>Empty <see cref="PathNameExtensionDto"/> if the provided file path is invalid.</returns>
        public static PathZerosDigitsExtensionDto GetPathZerosDigitsExtension(string filePath)
        {
            Match filePathMatch = Regex.Match(filePath, Validate.FilePathPattern);

            if (filePathMatch.Success)
            {
                string path = filePathMatch.Value(Validate.PathGroup);
                string name = filePathMatch.Value(Validate.NameGroup);
                string extension = filePathMatch.Value(Validate.ExtensionGroup);

                Match digitsNameMatch = Regex.Match(name, Validate.DigitsNamePattern);

                if (digitsNameMatch.Success)
                {
                    string zeros = digitsNameMatch.Value(Validate.ZerosGroup);
                    string digits = digitsNameMatch.Value(Validate.DigitsGroup);
                    name = digitsNameMatch.Value(Validate.NameGroup);

                    return new PathZerosDigitsExtensionDto(path, zeros, digits, name, extension);
                }
            }

            return PathZerosDigitsExtensionDto.Empty;
        }

        /// <summary>
        /// Converts path + digits + name + extension back into a consolidated file <see cref="Path"/>.
        /// </summary>
        public static string GetFilePath(string path, string digits, string name, string extension)
        {
            return Path.Combine(path, digits + name + extension);
        }
        #endregion
    }
}
