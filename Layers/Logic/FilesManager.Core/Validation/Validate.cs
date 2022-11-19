using FilesManager.Core.DTOs;
using FilesManager.Core.DTOs.Abstractions;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Validation
{
    /// <summary>
    /// Validation methods.
    /// </summary>
    public static class Validate
    {
        // Group names
        internal const string PathGroup = nameof(PathGroup);
        internal const string NameGroup = nameof(NameGroup);
        internal const string ExtensionGroup = nameof(ExtensionGroup);
        internal const string ZerosGroup = nameof(ZerosGroup);
        internal const string DigitsGroup = nameof(DigitsGroup);

        // Regex patterns
        internal const string InvalidCharactersPattern = @"[\\/:*?""<>|]";
        internal static readonly string FilePathPattern = $@"(?<{PathGroup}>.+\\)(?<{NameGroup}>.+)(?<{ExtensionGroup}>\.[aA-zZ0-9]\w+)";
        internal static readonly string DigitsNamePattern = $@"(?<{ZerosGroup}>0*)?(?<{DigitsGroup}>\d*)?(?<{NameGroup}>.*)";

        /// <summary>
        /// Determines whether the provided file has a valid extension.
        /// </summary>
        /// <param name="dropFilePath">The file path of dragged and dropped file.</param>
        /// <returns>
        ///   The answer whether provided file has an invalid extension.
        /// </returns>
        public static bool HasValidExtension(string dropFilePath)
        {
            if (string.IsNullOrWhiteSpace(dropFilePath) || dropFilePath.Length < 3)  // a.h
            {
                return false;
            }

            string fileExtension = Path.GetExtension(dropFilePath);

            const int fileExtensionMinLength = 1;
            const int fileExtensionMaxLength = 4;

            return fileExtension.Length > fileExtensionMinLength &&
                   fileExtension.Length <= (1 + fileExtensionMaxLength);  // File extension length + dot
        }

        /// <summary>
        /// Determines whether the given file path is valid.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="match">The matched result.</param>
        /// <returns>
        ///   The answer if file path is valid.
        /// </returns>
        internal static Match IsFilePathValid(string filePath)
        {
            return string.IsNullOrWhiteSpace(filePath)
                ? Match.Empty
                : Regex.Match(filePath, FilePathPattern);
        }

        /// <summary>
        /// Determines whether the given string contains illegal characters.
        /// </summary>
        /// <param name="invalidValue">The specific text which contains an invalid characters.</param>
        /// <param name="textInputs">The set of text inputs to be validated.</param>
        /// <returns>
        ///   The answer whether any of provided text values contains an illegal characters.
        /// </returns>
        public static bool ContainsIllegalCharacters(string[] textInputs, out string invalidValue)
        {
            invalidValue = string.Empty;

            if (textInputs != null)
            {
                foreach (string text in textInputs)
                {
                    if (Regex.IsMatch($"{text}", InvalidCharactersPattern))
                    {
                        invalidValue = text;

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the given DTO is valid.
        /// </summary>
        internal static RenamingResultDto IsPathDtoValid<T>(T pathDto, string previousFileName)
            where T : BasePathDto
        {
            return pathDto.IsEmpty()
                ? RenamingResultDto.Failure($"Internal (RegEx) error: The file \"{previousFileName}\" was't parsed properly")
                : pathDto switch
                  {
                      // Zeros-Digits type of DTO with missing Zeros and Digits values
                      PathZerosDigitsExtensionDto zerosDigitsDto when zerosDigitsDto.Zeros == string.Empty &&
                                                                      zerosDigitsDto.Digits == string.Empty
                        => RenamingResultDto.Failure($"The file name \"{previousFileName}\" does not contain preceeding numeric part"),
                      
                      // Default
                      _ => RenamingResultDto.Success(),
                  };
        }
    }
}
