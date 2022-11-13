namespace FilesManager.Core.DTOs
{
    /// <summary>
    /// The renaming operation result DTO.
    /// </summary>
    public class RenamingResultDto
    {
        /// <summary>
        /// Gets or sets the result of renaming operation.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets or sets the renaming result message (information, warning, error).
        /// </summary>
        public string Message { get; } = string.Empty;

        /// <summary>
        /// Gets or sets new renamed file path.
        /// </summary>
        public string NewFilePath { get; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenamingResultDto"/> class.
        /// </summary>
        public RenamingResultDto(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenamingResultDto"/> class.
        /// </summary>
        public RenamingResultDto(bool isSuccess, string message, string newFilePath) : this(isSuccess)
        {
            this.Message = message;
            this.NewFilePath = newFilePath;
        }
    }
}
