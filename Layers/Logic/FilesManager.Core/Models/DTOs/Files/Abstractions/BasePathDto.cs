namespace FilesManager.Core.Models.DTOs.Files.Abstractions
{
    /// <summary>
    /// Parent class for all file-related Data Transfer Objects (DTOs).
    /// </summary>
    internal abstract record BasePathDto
    {
        /// <summary>
        /// Gets the path of the file.
        /// </summary>
        internal string Path { get; init; } = string.Empty;

        /// <summary>
        /// Gets the extension of the file.
        /// </summary>
        internal string Extension { get; init; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePathDto"/> record.
        /// </summary>
        protected BasePathDto()  // NOTE: Required for "new TDto()"
        {
        }

        /// <inheritdoc cref="BasePathDto()"/>
        protected BasePathDto(string path, string extension)
        {
            this.Path = path;
            this.Extension = extension;
        }
    }
}
