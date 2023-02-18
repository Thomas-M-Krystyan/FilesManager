using System.Text.RegularExpressions;

namespace FilesManager.Core.Validation
{
    internal static class RegexPatterns
    {
        // -----------
        // Group names
        // -----------
        internal const string PathGroup = nameof(PathGroup);
        internal const string NameGroup = nameof(NameGroup);
        internal const string ExtensionGroup = nameof(ExtensionGroup);
        internal const string ZerosGroup = nameof(ZerosGroup);
        internal const string DigitsGroup = nameof(DigitsGroup);

        // --------------
        // Regex patterns
        // --------------

        /// <summary>
        /// Captures invalid characters in files names.
        /// </summary>
        internal static readonly Regex InvalidCharactersPattern = new($@"[{string.Join(string.Empty, Path.GetInvalidFileNameChars())}]", RegexOptions.Compiled);

        /// <summary>
        /// Captures path, name, and extensions components of the file path.
        /// </summary>
        internal static readonly Regex FilePathPattern = new($@"(?<{PathGroup}>.+\\)(?<{NameGroup}>.+)(?<{ExtensionGroup}>\.[aA-zZ0-9]\w+)", RegexOptions.Compiled);

        /// <summary>
        /// Captures zeroes, digits, and non-numeric components of the file name.
        /// </summary>
        internal static readonly Regex DigitsNamePattern = new($@"(?<{ZerosGroup}>0*)?(?<{DigitsGroup}>\d*)?(?<{NameGroup}>.*)", RegexOptions.Compiled);
    }
}
