using FilesManager.Core.Models.DTOs.Files.Abstractions;
using System.Diagnostics;

namespace FilesManager.Core.Models.DTOs.Files
{
    /// <summary>
    /// The basic file components: path, name, and extension.
    /// </summary>
    /// <seealso cref="BasePathDto"/>
    [DebuggerDisplay("Path: {Path}, Name: {Name}, Extension: {Extension}")]
    internal record FilePathNameDto : BasePathDto
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        internal string Name { get; init; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePathNameDto"/> record.
        /// </summary>
        internal FilePathNameDto(string path, string name, string extension)
            : base(path, extension)
        {
            this.Name = name;
        }
    }
}
