using FilesManager.Core.DTOs;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FileManager.Layers.Logic
{
    public static class RenamingService
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
        public static RenamingResultDto SetLeadingZeros(string oldFilePath, GroupCollection? fileGroups, GroupCollection? numberGroups, int zerosCount, int maxNumberLength)
        {
            return RenameFile(oldFilePath, () => GetLeadedZerosName(fileGroups, numberGroups, zerosCount, maxNumberLength));
        }
        #endregion

        #region Rename methods
        internal static string GetNumberIncrementedName(string oldFilePath, string prefix, ushort startNumber, string postfix)
        {
            Match filePathMatch = Validate.IsFilePathValid(oldFilePath);

            return filePathMatch.Success
                ? Path.Combine(filePathMatch.Value(Validate.PathGroup),  // The original file path (ended with "/")
                    $"{prefix.GetValueOrEmpty()}" +                      // (!) Optional prefix before the incremented number
                    $"{startNumber++}" +                                 // (!) Number replacing the name of a given file
                    $"{postfix.GetValueOrEmpty()}" +                     // (!) Optional postfix after the incremented number
                    $"{filePathMatch.Value(Validate.ExtensionGroup)}")   // The original file extension (with dot)
                : string.Empty;
        }

        internal static string GetPrependedAndAppendedName(string oldFilePath, string textToPrepend, string textToAppend)
        {
            Match filePathMatch = Validate.IsFilePathValid(oldFilePath);

            return filePathMatch.Success
                ? Path.Combine(filePathMatch.Value(Validate.PathGroup),  // The original file path (ended with "/")
                    $"{textToPrepend.GetValueOrEmpty()}" +               // (!) The text to be prepended
                    $"{filePathMatch.Value(Validate.NameGroup)}" +       // The original file name
                    $"{textToAppend.GetValueOrEmpty()}" +                // (!) The text to be appended
                    $"{filePathMatch.Value(Validate.ExtensionGroup)}")   // The original file extension (with dot)
                : string.Empty;
        }

        internal static string GetLeadedZerosName(GroupCollection? fileGroups, GroupCollection? numberGroups, int zerosCount, int maxNumberLength)
        {
            bool hasValidData = fileGroups != null && fileGroups.Count > 0 &&
                                numberGroups != null && numberGroups.Count > 0;

            return hasValidData
                ? Path.Combine(fileGroups!.Value(Validate.PathGroup),                        // The original file path (ended with "/")
                    //$"{NameWithLeadingZeros(numberGroups!, zerosCount, maxNumberLength)}" +  // (!) Set the specific number of leading zeros
                    $"{fileGroups!.Value(Validate.ExtensionGroup)}")                         // The original file extension (with dot)
                : string.Empty;
        }
        #endregion

        #region Helper methods
        //internal static string[] GetNamesWithLeadingZeros(string[] originalNames, int zerosCount)
        //{
        //    //int maxLength = GetLongestFileName();

        //    return default;
        //}

        /// <summary>
        /// Gets the name of the longest file.
        /// </summary>
        /// <param name="fileNames">The file names:
        ///   <para>
        ///     <list type="bullet">
        ///       <item>collection cannot be null</item>
        ///       <item>items cannot be null or contain only whitespaces</item>
        ///       <item>items should have only names (without extension)</item>
        ///     </list>
        ///   </para>
        /// </param>
        /// <returns></returns>
        internal static int GetLongestFileName(string[] fileNames)
        {
            int count = 0;

            for (int index = 0; index < fileNames.Length; index++)
            {
                count = Math.Max(count, fileNames[index].Length);
            }

            return count;
        }

        /// <summary>
        /// Adds the leading zeros to the beginning of the file.
        /// </summary>
        /// <param name="numberComponent">The extracted numeric part of the file name.</param>
        /// <param name="zerosCount">The amount of zeros to be added. Cannot be 0.</param>
        /// <param name="maxNumberLength">The lenght of the longest numeric component.</param>
        /// <returns>New collection of file names preceeded by zeros.</returns>
        internal static string[] AddLeadingZeros(string[] numberComponent, byte zerosCount, ushort maxNumberLength)
        {
            if (zerosCount == 0 || maxNumberLength == 0)
            {
                return numberComponent;
            }

            List<string> renamedComponent = new();

            for (int index = 0; index < numberComponent.Length; index++)
            {
                string newName;

                if (numberComponent[index].Length == 0)
                {
                    newName = numberComponent[index];
                }
                else
                {
                    int zerosToAdd = numberComponent[index].Length == maxNumberLength
                    ? zerosCount
                    : maxNumberLength - numberComponent[index].Length + zerosCount;

                    newName = $"{new string('0', zerosToAdd)}{numberComponent[index]}";
                }

                renamedComponent.Add(newName);
            }

            return renamedComponent.ToArray();
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

                return new RenamingResultDto(true, "Success", newFilePath);
            }
            catch (Exception exception)
            {
                return new RenamingResultDto(false, exception.Message, string.Empty);
            }
        }
        #endregion
    }
}
