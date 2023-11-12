namespace FilesManager.Core.Models.DTOs.Files
{
    /// <summary>
    /// The extended file components: path, name (zeros + digits), and extension.
    /// </summary>
    /// <seealso cref="PathNameExtensionDto"/>
    internal sealed record PathZerosDigitsExtensionDto : PathNameExtensionDto
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
        /// Gets the complete name of the file.
        /// </summary>
        internal string FullName => $"{this.Zeros}{this.Digits}{this.Name}";

        /// <summary>
        /// Initializes a new instance of the <see cref="PathZerosDigitsExtensionDto"/> record.
        /// </summary>
        internal PathZerosDigitsExtensionDto(string path, string zeros, string digits, string name, string extension)
            : base(path, name, extension)
        {
            this.Zeros = zeros;
            this.Digits = digits;
        }
    }
}
