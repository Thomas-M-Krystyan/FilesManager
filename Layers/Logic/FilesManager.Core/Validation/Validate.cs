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
        /// <param name="textInputs">The text input to be validated.</param>
        /// <returns>
        ///   The answer whether the provided input contains illegal characters.
        /// </returns>
        public static bool HaveInvalidCharacters(string textInput)
        {
            return RegexPatterns.InvalidCharactersPattern.IsMatch(textInput);
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

        /// <summary>
        /// Reports the invalid usage of an event.
        /// <para>
        ///   Used for development purposes to monitor if an event from the XAML side was binded to a command
        ///   subscribing to a method which is using a received object parameter as a proper event argument.
        /// </para>
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <exception cref="InvalidOperationException">The event argument is invalid.</exception>
        public static void ReportInvalidCommandUsage(string methodName)
        {
            throw new InvalidOperationException($"The method is used with a wrong event: {methodName}");
        }
    }
}
