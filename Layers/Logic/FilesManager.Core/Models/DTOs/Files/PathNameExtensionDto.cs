using FilesManager.Core.Models.DTOs.Files.Abstractions;

namespace FilesManager.Core.Models.DTOs.Files
{
    /// <summary>
    /// The basic components of a file path: path, name, and extension.
    /// </summary>
    /// <seealso cref="BasePathDto"/>
    public record PathNameExtensionDto : BasePathDto
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathNameExtensionDto"/> record.
        /// </summary>
        public PathNameExtensionDto(string path, string name, string extension)
            : base(path, extension)
        {
            this.Name = name;
        }
    }
}
