using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Services.Strategies;
using FilesManager.Core.Services.Writing;

namespace FileManager.Layers.Logic
{
    public static class RenamingService
    {
        /// <summary>
        /// Changes the name of a given file by replacing it with incremented numbers (and optional postfix after number).
        /// </summary>
        public static RenamingResultDto ReplaceWithNumber(string oldFilePath, string prefix, ushort number, string postfix)
        {
            return WritingService.RenameFile(oldFilePath, () => IncrementNumber.GetNumberIncrementedName(oldFilePath, prefix, number, postfix));
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
        public static RenamingResultDto UpdateWithPrependAndAppend(string oldFilePath, string textToPrepend, string textToAppend)
        {
            return WritingService.RenameFile(oldFilePath, () => PrependAppend.GetPrependedAndAppendedName(oldFilePath, textToPrepend, textToAppend));
        }

        /// <summary>
        /// Changes the name of a given file by setting a specified amount of leading zeros before the file name.
        /// </summary>
        public static RenamingResultDto SetLeadingZeros(string oldFilePath, PathZerosDigitsExtensionDto fileNameComponents, byte zerosCount, int maxNumberLength)
        {
            return WritingService.RenameFile(oldFilePath, () => LeadingZeros.GetLeadedZerosName(fileNameComponents, zerosCount, maxNumberLength));
        }
    }
}
