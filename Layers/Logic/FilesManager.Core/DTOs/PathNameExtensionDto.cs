using FilesManager.Core.DTOs.Abstractions;

namespace FilesManager.Core.DTOs
{
    /// <summary>
    /// All basic components of a file path: path, name, and extension.
    /// </summary>
    public class PathNameExtensionDto : BasePathDto
    {
        /// <summary>
        /// Returns an empty instance of <see cref="PathNameExtensionDto"/>.
        /// </summary>
        public static readonly PathNameExtensionDto Empty = new();

        /// <summary>
        /// Gets the name of a file.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathNameExtensionDto"/> class.
        /// </summary>
        private protected PathNameExtensionDto() { }

        /// <inheritdoc cref="PathNameExtensionDto()" />
        public PathNameExtensionDto(string path, string name, string extension) : base(path, extension)
        {
            this.Name = name;
        }
    }
}
