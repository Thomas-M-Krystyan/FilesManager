using FilesManager.Core.Converters;
using FilesManager.Core.DTOs;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FileManager.Layers.Logic
{
    public class RenamingService
    {
        #region API
        /// <summary>
        /// Changes the name of a given file by replacing it with incremented numbers (and optional postfix after number).
        /// </summary>
        public static RenamingResultDto ReplaceWithNumber(string oldFilePath, string prefix, ushort number, string postfix)
        {
            return RenameFile(oldFilePath, () => GetNumberIncrementedName(oldFilePath, prefix, number, postfix));
        }

        /// <summary>
        /// Changes the name of a given file by:
        /// <para>
        ///   - prepending a text before the original file name and / or
        /// </para>
        /// <para>
        ///   - appending a text after the original file name but before its extension.
        /// </para>
        /// </summary>
        public static RenamingResultDto EnrichWithPrependAndAppend(string oldFilePath, string textToPrepend, string textToAppend)
        {
            return RenameFile(oldFilePath, () => GetPrependedAndAppendedName(oldFilePath, textToPrepend, textToAppend));
        }

        /// <summary>
        /// Changes the name of a given file by setting a specified amount of leading zeros before the file name.
        /// </summary>
        public static RenamingResultDto SetLeadingZeros(string oldFilePath, PathZerosDigitsExtensionDto fileNameComponents, byte zerosCount, int maxNumberLength)
        {
            RenamingResultDto result = Validate.IsPathDtoValid(fileNameComponents, fileNameComponents.FullName);

            return result.IsSuccess
                ? RenameFile(oldFilePath, () => GetLeadedZerosName(fileNameComponents, zerosCount, maxNumberLength))
                : result;  // Failure
        }
        #endregion

        #region Rename methods
        internal static string GetNumberIncrementedName(string oldFilePath, string prefix, ushort startNumber, string postfix)
        {
            Match filePathMatch = Validate.IsFilePathValid(oldFilePath);

            return FilePathConverter.GetFilePath(
                path: filePathMatch.Value(Validate.PathGroup),
                name: $"{prefix.GetValueOrEmpty()}" +
                      $"{startNumber++}" +
                      $"{postfix.GetValueOrEmpty()}",
                extension: filePathMatch.Value(Validate.ExtensionGroup));
        }

        internal static string GetPrependedAndAppendedName(string oldFilePath, string textToPrepend, string textToAppend)
        {
            Match filePathMatch = Validate.IsFilePathValid(oldFilePath);

            return FilePathConverter.GetFilePath(
                path: filePathMatch.Value(Validate.PathGroup),
                name: $"{textToPrepend.GetValueOrEmpty()}" +
                      $"{filePathMatch.Value(Validate.NameGroup)}" +
                      $"{textToAppend.GetValueOrEmpty()}",
                extension: filePathMatch.Value(Validate.ExtensionGroup));
        }

        internal static string GetLeadedZerosName(PathZerosDigitsExtensionDto fileNameComponents, byte zerosCount, int maxNumberLength)
        {
            return FilePathConverter.GetFilePath(
                path: fileNameComponents.Path,
                name: $"{GetDigitsWithLeadingZeros(fileNameComponents.Digits, zerosCount, maxNumberLength)}" +
                      $"{fileNameComponents.Name}",
                extension: fileNameComponents.Extension);
        }
        #endregion

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

        #region System.IO.File
        /// <summary>
        /// Renames the given file using a specified renaming method.
        /// </summary>
        /// <param name="oldFilePath">The original file path.</param>
        /// <param name="renameMethod">The rename method to be used.</param>
        /// <returns>Result of renamed file.</returns>
        private static RenamingResultDto RenameFile(string oldFilePath, Func<string> renameMethod)
        {
            try
            {
                string newFilePath = renameMethod();

                File.Move(oldFilePath, newFilePath);

                return RenamingResultDto.Success(newFilePath);
            }
            catch (Exception exception)
            {
                return RenamingResultDto.Failure(exception.Message);
            }
        }
        #endregion
    }
}
