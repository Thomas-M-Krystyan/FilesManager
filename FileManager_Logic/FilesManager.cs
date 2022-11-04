using System;
using System.IO;
using System.Text.RegularExpressions;

#pragma warning disable CA1707   // Allow underscores in namespaces
#pragma warning disable IDE0130  // Allow underscores in namespaces

namespace FileManager_Logic
{
    public static class FilesManager
    {
        #region Fields
        // Group names
        internal const string PathGroup = nameof(PathGroup);
        internal const string NameGroup = nameof(NameGroup);
        internal const string ExtensionGroup = nameof(ExtensionGroup);
        internal const string ZerosGroup = nameof(ZerosGroup);
        internal const string DigitsGroup = nameof(DigitsGroup);

        // Regex patterns
        internal const string InvalidCharactersPattern = @"[\\/:*?""<>|]";
        internal static readonly string FilePathPattern = $@"(?<{PathGroup}>.+\\)(?<{NameGroup}>.+)(?<{ExtensionGroup}>\.[aA-zZ0-9]\w+)";
        internal static readonly string LeadingZerosPattern = $@"(?<{ZerosGroup}>0+)?(?<{DigitsGroup}>\d+)?(?<{ExtensionGroup}>\.[aA-zZ0-9]\w+)";
        #endregion

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
        public static (bool IsSuccess, string Message, string NewFilePath) SetLeadingZeros(string oldFilePath, GroupCollection fileGroups, GroupCollection numberGroups, int zerosCount, int maxNumberLength)
        {
            return RenameFile(oldFilePath, () => GetLeadedZerosName(fileGroups, numberGroups, zerosCount, maxNumberLength));
        }
        #endregion

        #region Rename methods
        internal static string GetNumberIncrementedName(string oldFilePath, string prefix, ushort startNumber, string postfix)
        {
            bool isSuccess = IsFilePathValid(oldFilePath, out Match match);

            return isSuccess
                ? Path.Combine(match.Groups[PathGroup].Value,                             // The original file path (ended with "/")
                    $"{(String.IsNullOrWhiteSpace(prefix) ? String.Empty : prefix)}" +    // (!) Optional prefix before the incremented number
                    $"{startNumber++}" +                                                  // (!) Number replacing the name of a given file
                    $"{(String.IsNullOrWhiteSpace(postfix) ? String.Empty : postfix)}" +  // (!) Optional postfix after the incremented number
                    $"{match.Groups[ExtensionGroup].Value}")                              // The original file extension (with dot)
                : String.Empty;
        }

        internal static string GetPrependedAndAppendedName(string oldFilePath, string textToPrepend, string textToAppend)
        {
            bool isSuccess = IsFilePathValid(oldFilePath, out Match match);

            return isSuccess
                ? Path.Combine(match.Groups[PathGroup].Value,                                         // The original file path (ended with "/")
                    $"{(String.IsNullOrWhiteSpace(textToPrepend) ? String.Empty : textToPrepend)}" +  // (!) The text to be prepended
                    $"{match.Groups[NameGroup].Value}" +                                              // The original file name
                    $"{(String.IsNullOrWhiteSpace(textToAppend) ? String.Empty : textToAppend)}" +    // (!) The text to be appended
                    $"{match.Groups[ExtensionGroup].Value}")                                          // The original file extension (with dot)
                : String.Empty;
        }

        internal static string GetLeadedZerosName(GroupCollection fileGroups, GroupCollection numberGroups, int zerosCount, int maxNumberLength)
        {
            return numberGroups?.Count != 0
                ? Path.Combine(fileGroups[PathGroup].Value,                                 // The original file path (ended with "/")
                    $"{NameWithLeadingZeros(numberGroups, zerosCount, maxNumberLength)}" +  // (!) Set the specific number of leading zeros
                    $"{fileGroups[ExtensionGroup].Value}")                                  // The original file extension (with dot)
                : String.Empty;

            // Set enough number of zeros before numeric part of the name
            static string NameWithLeadingZeros(GroupCollection groups, int zerosCount, int maxNumberLength)
            {
                if (zerosCount > 0)
                {
                    string zeros = groups[ZerosGroup].Value;
                    string digits = groups[DigitsGroup].Value;
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

                    return $"{leadingZeros}{groups[NameGroup].Value}";
                }

                return String.Empty;
            }
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
                return (false, exception.Message, String.Empty);
            }
        }
        #endregion

        #region Validation
        /// <summary>
        /// Determines whether the provided file has a valid extension.
        /// </summary>
        /// <param name="dropFilePath">The file path of dragged and dropped file.</param>
        /// <returns>
        ///   The answer whether provided file has an invalid extension.
        /// </returns>
        public static bool HasValidExtension(string dropFilePath)
        {
            string fileExtension = Path.GetExtension(dropFilePath);

            const int fileExtensionMinLength = 1;
            const int fileExtensionMaxLength = 4;

            return fileExtension.Contains(".") &&
                   fileExtension.Length > fileExtensionMinLength &&
                   fileExtension.Length <= 1 + fileExtensionMaxLength;  // File extension length + dot
        }

        /// <summary>
        /// Determines whether the given file path is valid.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="match">The matched result.</param>
        /// <returns>
        ///   The answer if file path is valid.
        /// </returns>
        internal static bool IsFilePathValid(string filePath, out Match match)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                match = null;

                return false;
            }

            match = Regex.Match(filePath, FilePathPattern);

            return match.Success;
        }

        /// <summary>
        /// Determines whether the given string contains illegal characters.
        /// </summary>
        /// <param name="invalidValue">The specific text which contains an invalid characters.</param>
        /// <param name="textInputs">The set of text inputs to be validated.</param>
        /// <returns>
        ///   The answer whether any of provided text values contains an illegal characters.
        /// </returns>
        public static bool ContainsIllegalCharacters(out string invalidValue, params string[] textInputs)
        {
            invalidValue = String.Empty;

            foreach (string text in textInputs)
            {
                if (Regex.IsMatch($@"{text}", InvalidCharactersPattern))
                {
                    invalidValue = text;

                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Helper methods
        public static (int NumberLength, GroupCollection FileGroups, GroupCollection NumberGroups) GetNumberLength(string oldFilePath)
        {
            const int NotFound = 0;  // There are no digits in the file name

            if (IsFilePathValid(oldFilePath, out Match pathMatch))
            {
                string fileName = pathMatch.Groups[NameGroup].Value;

                Match numberMatch = Regex.Match(fileName, LeadingZerosPattern);
                GroupCollection numberGroups = numberMatch.Groups;

                return numberMatch.Success
                    ? (numberGroups[ZerosGroup].Value.Length + numberGroups[DigitsGroup].Value.Length, pathMatch.Groups, numberGroups)
                    : (NotFound, pathMatch.Groups, numberGroups);
            }
            else
            {
                return (NotFound, pathMatch.Groups, null);
            }
        }
        #endregion
    }
}
