using FilesManager.Core.DTOs;
using FilesManager.Core.Services.Strategies;
using FilesManager.Core.Validation;

namespace FileManager.Layers.Logic
{
    public class RenamingService
    {
        /// <summary>
        /// Changes the name of a given file by replacing it with incremented numbers (and optional postfix after number).
        /// </summary>
        public static RenamingResultDto ReplaceWithNumber(string oldFilePath, string prefix, ushort number, string postfix)
        {
            return RenameFile(oldFilePath, () => Increment.GetNumberIncrementedName(oldFilePath, prefix, number, postfix));
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
            return RenameFile(oldFilePath, () => PrependAppend.GetPrependedAndAppendedName(oldFilePath, textToPrepend, textToAppend));
        }

        /// <summary>
        /// Changes the name of a given file by setting a specified amount of leading zeros before the file name.
        /// </summary>
        public static RenamingResultDto SetLeadingZeros(string oldFilePath, PathZerosDigitsExtensionDto fileNameComponents, byte zerosCount, int maxNumberLength)
        {
            RenamingResultDto result = Validate.IsPathDtoValid(fileNameComponents, fileNameComponents.FullName);

            return result.IsSuccess
                ? RenameFile(oldFilePath, () => LeadingZeros.GetLeadedZerosName(fileNameComponents, zerosCount, maxNumberLength))
                : result;  // Failure
        }

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
