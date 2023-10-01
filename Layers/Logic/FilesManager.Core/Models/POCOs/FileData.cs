namespace FilesManager.Core.Models.POCOs
{
    /// <summary>
    /// The POCO model representing <see cref="File"/> metadata.
    /// </summary>
    public record FileData
    {
        /// <summary>
        /// The full path to the file: <code>"C:\Users\JaneDoe\Desktop\Test.txt"</code>
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// The name of the file: <code>"Test"</code>
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The extension of the file: <code>".txt"</code>
        /// </summary>
        public string Extension { get; set; } = string.Empty;

        /// <summary>
        /// The file name with extension to be displayed on UI: <code>"Test.txt"</code>
        /// </summary>
        public string DisplayName => $"{this.Name}{this.Extension}";
    }
}
