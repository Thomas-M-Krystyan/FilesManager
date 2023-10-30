using FilesManager.Core.Validation;

namespace FilesManager.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see langword="string"/>s.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Gets the value of the given <see langword="string"/>.
        /// </summary>
        /// <param name="value">The value to be guard checked.</param>
        /// <returns>
        ///   The value of the given <see langword="string"/> or <see cref="string.Empty"/> if it is empty or contains only white spaces.
        /// </returns>
        internal static string TrimOnlyWhiteSpaces(this string value)
        {
            return value.IsOnlyWhiteSpaces() ? string.Empty : value;
        }

        /// <summary>
        /// Determines whether the given <see langword="string"/> is empty or it contains only white spaces.
        /// </summary>
        /// <param name="value">The string to be examined.</param>
        /// <returns>
        ///   <see langword="true"/> if the string is empty or contains only white spaces; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool IsEmptyOrWhiteSpaces(this string value)
        {
            return value.Length == 0 ||
                   value.IsOnlyWhiteSpaces();
        }

        /// <summary>
        /// Determines whether the given <see langword="string"/> contains only white spaces.
        /// </summary>
        /// <param name="value">The string to be examined.</param>
        /// <returns>
        ///   <see langword="true"/> if the string is empty or contains only white spaces; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool IsOnlyWhiteSpaces(this string value)
        {
            return RegexPatterns.WhiteSpacePattern().IsMatch(value);
        }
    }
}
