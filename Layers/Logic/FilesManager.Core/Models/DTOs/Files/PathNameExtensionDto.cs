using FilesManager.Core.Models.DTOs.Files.Abstractions;

namespace FilesManager.Core.Models.DTOs.Files
{
    /// <summary>
    /// The basic file components: path, name, and extension.
    /// </summary>
    /// <seealso cref="BasePathDto"/>
    internal record PathNameExtensionDto : BasePathDto
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        internal string Name { get; init; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathNameExtensionDto"/> record.
        /// </summary>
        internal PathNameExtensionDto(string path, string name, string extension)
            : base(path, extension)
        {
            this.Name = name;
        }
    }
}
