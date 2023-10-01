﻿namespace FilesManager.Core.Models.DTOs.Results
{
    /// <summary>
    /// The renaming operation result DTO.
    /// </summary>
    public sealed record RenamingResultDto
    {
        /// <summary>
        /// Gets the result of renaming operation.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the renaming result message (information, warning, error).
        /// </summary>
        public string Message { get; } = string.Empty;

        /// <summary>
        /// Gets new renamed file path.
        /// </summary>
        public string NewFilePath { get; } = string.Empty;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RenamingResultDto"/> record.
        /// </summary>
        private RenamingResultDto(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        /// <inheritdoc cref="RenamingResultDto(bool)" />
        private RenamingResultDto(bool isSuccess, string message)
            : this(isSuccess)
        {
            this.Message = message;
        }

        /// <inheritdoc cref="RenamingResultDto(bool)" />
        private RenamingResultDto(bool isSuccess, string message, string newFilePath)
            : this(isSuccess, message)
        {
            this.NewFilePath = newFilePath;
        }
        #endregion

        #region Results
        /// <summary>
        /// Renaming operation was successful.
        /// </summary>
        public static RenamingResultDto Success()
        {
            return new RenamingResultDto(true);
        }

        /// <inheritdoc cref="Success" />
        public static RenamingResultDto Success(string newFilePath)
        {
            return new RenamingResultDto(true, "Success", newFilePath);
        }

        /// <summary>
        /// Renaming operation was unsuccessful.
        /// </summary>
        public static RenamingResultDto Failure()
        {
            return new RenamingResultDto(false, "Failure");
        }

        /// <inheritdoc cref="Failure" />
        public static RenamingResultDto Failure(string message)
        {
            return new RenamingResultDto(false, message);
        }
        #endregion
    }
}