namespace FilesManager.Core.ExtensionMethods
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Gets the value or empty string (to avoid <see langword="null"/>).
        /// </summary>
        internal static string GetValueOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
    }
}
