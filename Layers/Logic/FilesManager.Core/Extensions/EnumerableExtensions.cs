namespace FilesManager.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>s.
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether the given collection is null or empty.
        /// </summary>
        /// <param name="collection">The collection to be validated.</param>
        /// <returns>
        ///   <see langword="true"/> if the collection is invalid; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection?.Count() == 0;
        }

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
