using FilesManager.Core.Converters;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Services.Renaming.Strategies
{
    internal static class LeadingZeros
    {
        internal static string GetLeadedZerosName(PathZerosDigitsExtensionDto fileNameComponents, byte zerosCount, int maxNumberLength)
        {
            RenamingResultDto result = Validate.IsPathDtoValid(fileNameComponents, fileNameComponents.FullName);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(result.Message);
            }

            Match filePathMatch = Validate.IsFilePathValid(fileNameComponents.Path);

            return !filePathMatch.Success
                ? string.Empty
                : FilePathConverter.GetFilePath(
                    path: fileNameComponents.Path,
                    name: $"{GetDigitsWithLeadingZeros(fileNameComponents.Digits, zerosCount, maxNumberLength)}" +
                          $"{fileNameComponents.Name}",
                    extension: fileNameComponents.Extension);
        }

        #region Helper methods
        /// <summary>
        /// Adds the leading zeros to the beginning of the file.
        /// </summary>
        /// <param name="initialDigits">The extracted digits part of the file name.</param>
        /// <param name="zerosCount">The amount of zeros to be added. Cannot be 0.</param>
        /// <param name="maxNumberLength">The lenght of the longest numeric component.</param>
        /// <returns>New file name preceeded by zeros.</returns>
        internal static string GetDigitsWithLeadingZeros(string initialDigits, byte zerosCount, int maxNumberLength)
        {
            if (zerosCount == 0 || maxNumberLength <= 0)
            {
                return initialDigits;
            }

            int zerosToAdd = initialDigits.Length == maxNumberLength
                ? zerosCount
                : maxNumberLength - initialDigits.Length + zerosCount;

            return $"{new string('0', zerosToAdd)}{initialDigits}";
        }
        #endregion
    }
}
