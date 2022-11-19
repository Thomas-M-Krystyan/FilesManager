namespace FilesManager.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>s.
    /// </summary>
    internal static class EnumerableExtensions
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection?.Count() == 0;
        }
    }
}
