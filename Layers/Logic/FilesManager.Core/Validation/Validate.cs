﻿using FilesManager.Core.DTOs;
using FilesManager.Core.DTOs.Abstractions;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Validation
{
    /// <summary>
    /// Validation methods.
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Determines whether the provided file has a valid extension.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>
        ///   The answer whether provided file has an invalid extension.
        /// </returns>
        public static bool HasValidExtension(string filePath)
        {
            const int dotLength = 1;
            const int fileExtensionMinLength = 1;

            if (string.IsNullOrWhiteSpace(filePath) ||  // Invalid example: "" or " "
                !(filePath.Length > dotLength + fileExtensionMinLength))  // Invalid example: ".h"
            {
                return false;
            }

            string fileExtension = Path.GetExtension(filePath);

            const int fileExtensionMaxLength = 4;

            return fileExtension.Length is >= dotLength + fileExtensionMinLength    // Valid example: "a[.h]"
                                        and <= dotLength + fileExtensionMaxLength;  // Valid example: "a[.jpeg]"
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
                : RegexPatterns.FilePathPattern.Match(filePath);
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
                    if (RegexPatterns.InvalidCharactersPattern.IsMatch($"{text}"))
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
