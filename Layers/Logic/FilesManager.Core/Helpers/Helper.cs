namespace FilesManager.Core.Helpers
{
    /// <summary>
    /// Helper methods for common usage.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Gets the length of the longest text from the provided input.
        /// </summary>
        /// <param name="inputs">The strings to be examined.</param>
        /// <returns>
        ///   The size of the longest <see langword="string"/>.
        /// </returns>
        internal static int GetMaxLength(this IEnumerable<string> inputs)
        {
            return inputs.Max(input => input.Length);
        }
    }
}
