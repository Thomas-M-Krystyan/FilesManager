using System.Text.RegularExpressions;

namespace FilesManager.Core.Validation
{
    /// <summary>
    /// A set of handful Regular Expression patterns.
    /// </summary>
    public static class RegexPatterns
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
        /// Captures anything that is not a digit.
        /// </summary>
        public static readonly Regex NotDigit = new("[^0-9]+", RegexOptions.Compiled);

        /// <summary>
        /// Captures invalid characters in files names.
        /// </summary>
        internal static readonly Regex InvalidCharactersPattern = new("[" +
            $"{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}" +
            "]", RegexOptions.Compiled);

        /// <summary>
        /// Captures path, name, and extensions components of the file path.
        /// </summary>
        internal static readonly Regex FilePathPattern = new($@"^(?<{PathGroup}>[A-Z]{{1}}\:\\.+\\)(?<{NameGroup}>[a-zA-Z0-9 ._-]+)(?<{ExtensionGroup}>\.[aA-zZ0-9]{{1,6}})$", RegexOptions.Compiled);

        /// <summary>
        /// Captures zeroes, digits, and non-numeric components of the file name.
        /// </summary>
        internal static readonly Regex DigitsNamePattern = new($@"(?<{ZerosGroup}>0*)?(?<{DigitsGroup}>\d*)?(?<{NameGroup}>.*)", RegexOptions.Compiled);
    }
}
