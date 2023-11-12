using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Files.Abstractions;
using FilesManager.Core.Validation;
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
        internal static PathNameExtensionDto GetPathNameExtensionDto(this Match filePathMatch)
        {
            return new PathNameExtensionDto(path:      filePathMatch.Value(RegexPatterns.PathGroup),
                                            name:      filePathMatch.Value(RegexPatterns.NameGroup),
                                            extension: filePathMatch.Value(RegexPatterns.ExtensionGroup));
        }
        #endregion

        #region Path + Zeros + Digits + Name + Extension
        /// <summary>
        /// Converts file path into dedicated groups of values: path, zeros, digits, name, and extension.
        /// </summary>
        /// <param name="dto">The DTO with basic file components.</param>
        /// <returns>
        ///   Empty <see cref="PathZerosDigitsExtensionDto"/> if the provided file path is invalid.
        /// </returns>
        internal static PathZerosDigitsExtensionDto GetPathZerosDigitsExtensionDto(this PathNameExtensionDto dto)
        {
            // NOTE: Split the file name into dedicated zeros, digits, and name groups
            Match digitsNameMatch = RegexPatterns.DigitsNamePattern().Match(dto.Name);

            return new PathZerosDigitsExtensionDto(path:      dto.Path,
                                                   zeros:     digitsNameMatch.Value(RegexPatterns.ZerosGroup),
                                                   digits:    digitsNameMatch.Value(RegexPatterns.DigitsGroup),
                                                   name:      digitsNameMatch.Value(RegexPatterns.NameGroup),
                                                   extension: dto.Extension);
        }
        #endregion

        /// <summary>
        /// Converts path + name + extension back into a consolidated file's <see cref="Path"/>.
        /// </summary>
        internal static string GetFilePath(this BasePathDto fileDto, string name)
        {
            return name.IsEmptyOrWhiteSpaces()
                ? string.Empty
                : Path.Combine(fileDto.Path, name + fileDto.Extension);
        }
    }
}
