using System.Diagnostics;

namespace FilesManager.Core.Models.DTOs.Files
{
    /// <summary>
    /// The extended file components: path, name (zeros + digits), and extension.
    /// </summary>
    /// <seealso cref="FilePathNameDto"/>
    [DebuggerDisplay("Path: {Path}, Zeros: {Zeros}, Digits: {Digits}, Name: {Name}, Extension: {Extension}")]
    internal sealed record FileZerosDigitsDto : FilePathNameDto
    {
        /// <summary>
        /// Gets the zeros part of the name of the file.
        /// </summary>
        internal string Zeros { get; init; } = string.Empty;

        /// <summary>
        /// Gets the digits part of the name of the file.
        /// </summary>
        internal string Digits { get; init; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileZerosDigitsDto"/> record.
        /// </summary>
        internal FileZerosDigitsDto(string path, string zeros, string digits, string name, string extension)
            : base(path, name, extension)
        {
            this.Zeros = zeros;
            this.Digits = digits;
        }

        /// <inheritdoc cref="FileZerosDigitsDto(string, string, string, string, string)"/>
        internal FileZerosDigitsDto(FileZerosDigitsDto dto, string zeros, string digits)
            : this(dto.Path, zeros, digits, dto.Name, dto.Extension)
        {
        }
    }
}
