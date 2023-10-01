using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Files.Abstractions;
using FilesManager.Core.Models.DTOs.Results;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Validation
{
    /// <summary>
    /// Validation methods.
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Determines whether the given file path is valid.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="match">The matched result.</param>
        /// <returns>
        ///   The answer if file path is valid.
        /// </returns>
        public static Match IsFilePathValid(string filePath)
        {
            return string.IsNullOrWhiteSpace(filePath)
                ? Match.Empty
                : RegexPatterns.FilePathPattern.Match(filePath);
        }

        /// <summary>
        /// Determines whether the given <see langword="string"/> contains illegal characters.
        /// </summary>
        /// <param name="textInputs">The probe of text inputs to be validated.</param>
        /// <param name="invalidInput">The specific input that contains invalid characters.</param>
        /// <returns>
        ///   The answer whether any of the provided inputs contain illegal characters.
        /// </returns>
        public static (bool ContainsInvalid, string InvalidInput) HaveInvalidCharacters(params string[] textInputs)
        {
            for (int index = 0; index < textInputs.Length; index++)
            {
                string currentInput = textInputs[index];

                if (RegexPatterns.InvalidCharactersPattern.IsMatch(currentInput))
                {
                    return (true, currentInput);
                }
            }

            return (false, string.Empty);
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
