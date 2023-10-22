using System.Text.RegularExpressions;

namespace FilesManager.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="GroupCollection"/>s.
    /// </summary>
    internal static class GroupCollectionExtensions
    {
        /// <summary>
        /// Gets the content (value) of a specified <see cref="Group"/> from <see cref="GroupCollection"/>.
        /// </summary>
        internal static string Value(this GroupCollection groupCollection, string groupName)
        {
            return groupCollection[groupName].Value;
        }

        /// <summary>
        /// Gets the content (value) of a specified <see cref="Group"/> from <see cref="Match"/>.
        /// </summary>
        internal static string Value(this Match match, string groupName)
        {
            return match.Groups[groupName].Value;
        }
    }
}
