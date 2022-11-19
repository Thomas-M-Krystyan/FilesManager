namespace FilesManager.Core.DTOs
{
    /// <summary>
    /// Extended components of a file path: path, name (zeros + digits), and extension.
    /// </summary>
    public class PathZerosDigitsExtensionDto : PathNameExtensionDto
    {
        /// <summary>
        /// Returns an empty instance of <see cref="PathZerosDigitsExtensionDto"/>.
        /// </summary>
        public static new readonly PathZerosDigitsExtensionDto Empty = new();

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
        public virtual string FullName => $"{this.Zeros}{this.Digits}{this.Name}";

        /// <summary>
        /// Initializes a new instance of the <see cref="PathZerosDigitsExtensionDto"/> class.
        /// </summary>
        private PathZerosDigitsExtensionDto() { }

        /// <inheritdoc cref="PathZerosDigitsExtensionDto()" />
        public PathZerosDigitsExtensionDto(string path, string zeros, string digits, string name, string extension) : base(path, name, extension)
        {
            this.Zeros = zeros;
            this.Digits = digits;
        }
    }
}
