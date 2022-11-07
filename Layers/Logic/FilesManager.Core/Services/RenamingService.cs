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
        public static (bool IsSuccess, string Message, string NewFilePath) ReplaceWithNumber(string oldFilePath, string prefix, ushort number, string postfix)
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
        public static (bool IsSuccess, string Message, string NewFilePath) EnrichWithPrependAndAppend(string oldFilePath, string textToPrepend, string textToAppend)
        {
            return RenameFile(oldFilePath, () => GetPrependedAndAppendedName(oldFilePath, textToPrepend, textToAppend));
        }

        /// <summary>
        /// Changes the name of a given file by setting a specified amount of leading zeros before the file name.
        /// </summary>
        public static (bool IsSuccess, string Message, string NewFilePath) SetLeadingZeros(string oldFilePath, GroupCollection? fileGroups, GroupCollection? numberGroups, int zerosCount, int maxNumberLength)
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
            bool hasValidData = (fileGroups != null && fileGroups.Count > 0) &&
                                (numberGroups != null && numberGroups.Count > 0);

            return hasValidData
                ? Path.Combine(fileGroups!.Value(Validate.PathGroup),                        // The original file path (ended with "/")
                    $"{NameWithLeadingZeros(numberGroups!, zerosCount, maxNumberLength)}" +  // (!) Set the specific number of leading zeros
                    $"{fileGroups!.Value(Validate.ExtensionGroup)}")                         // The original file extension (with dot)
                : string.Empty;
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Set enough number of zeros before numeric part of the name
        /// </summary>
        private static string NameWithLeadingZeros(GroupCollection nameGroups, int zerosCount, int maxNumberLength)
        {
            if (zerosCount > 0)
            {
                string zeros = nameGroups.Value(Validate.ZerosGroup);
                string digits = nameGroups.Value(Validate.DigitsGroup);
                int numberLength = zeros.Length + digits.Length;

                /* Add more zeroes to shorter number to make all of them the same size:

                   REQUEST:
                     "Add 1 more zero to numbers 10 & 100"

                   NOTE: Zero will be added to number 100 since this is the longest number in the given
                         sequence => 0100, and number 10 will be compensated => 0010 to have equal length

                   RESULT:
                     0010 & 0100
                */
                if (numberLength < maxNumberLength)
                {
                    int difference = maxNumberLength - numberLength;
                    zerosCount += difference;
                }

                string leadingZeros = $"{new string('0', zerosCount)}{digits}";

                return $"{leadingZeros}{nameGroups.Value(Validate.NameGroup)}";
            }

            return string.Empty;
        }
        #endregion

        #region System.IO.File
        /// <summary>
        /// Renames the given file using a specified renaming method.
        /// </summary>
        /// <param name="oldFilePath">The original file path.</param>
        /// <param name="renameMethod">The rename method to be used.</param>
        /// <returns>Result of renamed file.</returns>
        private static (bool IsSuccess, string Message, string NewFilePath) RenameFile(string oldFilePath, Func<string> renameMethod)
        {
            try
            {
                string newFilePath = renameMethod();

                File.Move(oldFilePath, newFilePath);

                return (true, "Success", newFilePath);
            }
            catch (Exception exception)
            {
                return (false, exception.Message, string.Empty);
            }
        }
        #endregion
    }
}
