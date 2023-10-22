namespace FilesManager.Core.Models.DTOs.Files.Abstractions
{
    /// <summary>
    /// Parent class for all file-related Data Transfer Objects (DTOs).
    /// </summary>
    public abstract record BasePathDto
    {
        /// <summary>
        /// Gets the path of the file.
        /// </summary>
        public string Path { get; } = string.Empty;

        /// <summary>
        /// Gets the extension of the file.
        /// </summary>
        public string Extension { get; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePathDto"/> record.
        /// </summary>
        protected BasePathDto(string path, string extension)
        {
            this.Path = path;
            this.Extension = extension;
        }
    }
}
