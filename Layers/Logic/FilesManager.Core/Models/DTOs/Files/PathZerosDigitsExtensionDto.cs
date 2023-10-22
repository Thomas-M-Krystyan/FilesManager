namespace FilesManager.Core.Models.DTOs.Files
{
    /// <summary>
    /// The extended components of a file path: path, name (zeros + digits), and extension.
    /// </summary>
    /// <seealso cref="PathNameExtensionDto"/>
    public sealed record PathZerosDigitsExtensionDto : PathNameExtensionDto
    {
        /// <summary>
        /// Gets the zeros part of the name of the file.
        /// </summary>
        public string Zeros { get; } = string.Empty;

        /// <summary>
        /// Gets the digits part of the name of the file.
        /// </summary>
        public string Digits { get; } = string.Empty;

        /// <summary>
        /// Gets the complete name of the file.
        /// </summary>
        public string FullName => $"{this.Zeros}{this.Digits}{this.Name}";

        /// <summary>
        /// Initializes a new instance of the <see cref="PathZerosDigitsExtensionDto"/> record.
        /// </summary>
        public PathZerosDigitsExtensionDto(string path, string zeros, string digits, string name, string extension)
            : base(path, name, extension)
        {
            this.Zeros = zeros;
            this.Digits = digits;
        }
    }
}
