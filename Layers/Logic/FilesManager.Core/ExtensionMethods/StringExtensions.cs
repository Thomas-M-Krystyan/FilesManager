namespace FilesManager.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see langword="string"/>s.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Gets the value or empty string (to avoid <see langword="null"/>).
        /// </summary>
        internal static string GetValueOrEmpty(this string value)
        {
            return String.IsNullOrWhiteSpace(value) ? String.Empty : value;
        }
    }
}
