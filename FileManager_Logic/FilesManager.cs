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
        private const string PathGroup = nameof(PathGroup);
        private const string NameGroup = nameof(NameGroup);
        private const string ExtensionGroup = nameof(ExtensionGroup);

        // Regex patterns
        private const string InvalidCharactersPatter = "[\\/:*?\"<>|]";
        internal static readonly string FilePathPattern = $@"(?<{PathGroup}>.+\\)(?<{NameGroup}>.+)(?<{ExtensionGroup}>\.[aA-zZ0-9]\w+)";
        #endregion

        /// <summary>
        /// Determines whether the provided file has a valid extension.
        /// </summary>
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
            return RenameFile(oldFilePath, () => GetPrpendedAndAppendedName(oldFilePath, textToPrepend, textToAppend));
        }

        #region Rename methods
        internal static string GetNumberIncrementedName(string filePath, string prefix, ushort startNumber, string postfix)
        {
            bool isSuccess = IsFilePathValid(filePath, out Match match);

            return isSuccess
                ? Path.Combine(match.Groups[PathGroup].Value,                             // The original file path (ended with "/")
                    $"{(String.IsNullOrWhiteSpace(prefix) ? String.Empty : prefix)}" +    // (!) Optional prefix before the incremented number
                    $"{startNumber++}" +                                                  // (!) Number replacing the name of a given file
                    $"{(String.IsNullOrWhiteSpace(postfix) ? String.Empty : postfix)}" +  // (!) Optional postfix after the incremented number
                    $"{match.Groups[ExtensionGroup].Value}")                              // The original file extension (with dot)
                : String.Empty;
        }

        internal static string GetPrpendedAndAppendedName(string filePath, string textToPrepend, string textToAppend)
        {
            bool isSuccess = IsFilePathValid(filePath, out Match match);

            return isSuccess
                ? Path.Combine(match.Groups[PathGroup].Value,                                         // The original file path (ended with "/")
                    $"{(String.IsNullOrWhiteSpace(textToPrepend) ? String.Empty : textToPrepend)}" +  // (!) The text to be prepended
                    $"{match.Groups[NameGroup].Value}" +                                              // The original file name
                    $"{(String.IsNullOrWhiteSpace(textToAppend) ? String.Empty : textToAppend)}" +    // (!) The text to be appended
                    $"{match.Groups[ExtensionGroup].Value}")                                          // The original file extension (with dot)
                : String.Empty;
        }
        #endregion

        #region System.IO.File
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

        public static bool ContainsIllegalCharacters(out string invalidValue, params string[] textInputs)
        {
            invalidValue = String.Empty;

            foreach (string text in textInputs)
            {
                if (Regex.IsMatch(text, InvalidCharactersPatter))
                {
                    invalidValue = text;

                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
