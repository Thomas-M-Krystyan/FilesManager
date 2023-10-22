using FilePath = System.IO.Path;

namespace FilesManager.Core.Models.POCOs
{
    /// <summary>
    /// The POCO model representing <see cref="File"/> metadata.
    /// </summary>
    internal record FileData
    {
        /// <summary>
        /// The full path to the file: <code>"C:\Users\JaneDoe\Desktop\Test.txt"</code>
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// The file name with extension to be displayed on UI: <code>"Test.txt"</code>
        /// </summary>
        public string DisplayName => $"{FilePath.GetFileName(this.Path)}";
    }
}
