using System.Text.RegularExpressions;

namespace FilesManager.Core.Validation
{
    /// <summary>
    /// A set of handful Regular Expression patterns.
    /// </summary>
    internal static partial class RegexPatterns
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
        /// <exception cref="ArgumentNullException"/>
        internal static readonly Regex InvalidCharactersPattern = new("[" +
            $"{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}" +
            "]", RegexOptions.Compiled);

        /// <summary>
        /// Captures path, name, and extensions components of the file path.
        /// </summary>
        [GeneratedRegex($@"^(?<{PathGroup}>[A-Z]{{1}}\:\\.+\\)(?<{NameGroup}>.+)(?<{ExtensionGroup}>\.[aA-zZ0-9]{{1,6}})$", RegexOptions.Compiled)]
        internal static partial Regex FileComponentsPattern();

        /// <summary>
        /// Captures zeroes, digits, and non-numeric components of the file name.
        /// </summary>
        [GeneratedRegex($@"^(?<{ZerosGroup}>0*)(?<{DigitsGroup}>\d*)(?<{NameGroup}>.*)$", RegexOptions.Compiled)]
        internal static partial Regex DigitsNamePattern();

        /// <summary>
        /// Captures one or many white spaces.
        /// </summary>
        [GeneratedRegex(@"^\s+$", RegexOptions.Compiled)]
        internal static partial Regex WhiteSpacePattern();
    }
}
