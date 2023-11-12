using FilePath = System.IO.Path;
using RegexMatch = System.Text.RegularExpressions.Match;

namespace FilesManager.Core.Models.POCOs
{
    /// <summary>
    /// The POCO model representing <see cref="File"/> metadata.
    /// </summary>
    internal sealed record FileData
    {
        /// <summary>
        /// The full path to the file: <code>"C:\Users\JaneDoe\Desktop\Test.txt"</code>
        /// </summary>
        internal string Path { get; set; } = string.Empty;

        /// <summary>
        /// The file name with extension to be displayed on UI: <code>"Test.txt"</code>
        /// </summary>
        public string DisplayName => $"{FilePath.GetFileName(this.Path)}";

        /// <inheritdoc cref="RegexMatch"/>
        internal RegexMatch Match { get; set; } = RegexMatch.Empty;
    }
}
