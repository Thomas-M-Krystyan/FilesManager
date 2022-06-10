using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace FileManager_Logic
{
    public static class FilesManager
    {
        // Group names
        private const string PathGroup = nameof(PathGroup);
        private const string NameGroup = nameof(NameGroup);
        private const string ExtensionGroup = nameof(ExtensionGroup);

        // Regex pattern
        private static readonly string FilePathPattern = $@"^(?<{PathGroup}>.+\\)(?<{NameGroup}>\w+)(?<{ExtensionGroup}>\.[aA-zZ0-9]{{1,4}})$";

        /// <summary>
        /// Determines whether the provided file has a valid extension.
        /// </summary>
        public static bool IsValidFileType(string dropFilePath)
        {
            string filePath = Path.GetExtension(dropFilePath);

            return filePath.Contains(".jpg") || filePath.Contains(".jpeg") || filePath.Contains(".png");
        }

        /// <summary>
        /// Change the name of a given file by replacing it with incremented numbers.
        /// </summary>
        public static (bool IsSuccess, string Message, string NewFilePath) ReplaceFile(ListBoxItem fileItem, ushort number)
        {
            try
            {
                string oldFilePath = fileItem.ToolTip as string;
                string newFilePath = RenameFile(oldFilePath, number);

                File.Move(oldFilePath, newFilePath);

                return (true, "Success", newFilePath);
            }
            catch (Exception exception)
            {
                return (false, exception.Message, String.Empty);
            }
        }

        internal static string RenameFile(string filePath, ushort startNumber)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                return String.Empty;
            }

            Match match = Regex.Match(filePath, FilePathPattern);

            return match.Success
                ? Path.Combine(match.Groups[PathGroup].Value, $"{startNumber++}{match.Groups[ExtensionGroup].Value}")
                : String.Empty;
        }
    }
}
