using System.Diagnostics;

namespace FilesManager.Core.Models.DTOs.Results
{
    /// <summary>
    /// The renaming operation result DTO.
    /// </summary>
    [DebuggerDisplay("Success: {IsSuccess}, {ValueName,nq}: {Value}")]
    internal sealed record RenamingResultDto
    {
        #region Defaults
        private static readonly RenamingResultDto DefaultFailure = new(isSuccess: false);
        #endregion

        /// <summary>
        /// Gets the result of renaming operation.
        /// </summary>
        internal bool IsSuccess { get; }

        /// <summary>
        /// Gets the result value:
        /// <list type="bullet">
        ///   <item>Success: New file path</item>
        ///   <item>Failure: Error message(s)</item>
        /// </list>
        /// </summary>
        internal string Value { get; } = string.Empty;

        // NOTE: Used implicitly by [DebuggerDisplay]
        private string ValueName => this.IsSuccess ? "New path" : "Error";
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RenamingResultDto"/> record.
        /// </summary>
        private RenamingResultDto(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        /// <inheritdoc cref="RenamingResultDto(bool)" />
        private RenamingResultDto(bool isSuccess, string value)
            : this(isSuccess)
        {
            this.Value = value;
        }
        #endregion

        #region Results
        /// <summary>
        /// Renaming operation was successful.
        /// </summary>
        internal static RenamingResultDto Success(string newFilePath)
        {
            return new RenamingResultDto(isSuccess: true, value: newFilePath);
        }

        /// <summary>
        /// Renaming operation was unsuccessful.
        /// </summary>
        internal static RenamingResultDto Failure()
        {
            return DefaultFailure;
        }

        /// <inheritdoc cref="Failure" />
        internal static RenamingResultDto Failure(string errorMessage)
        {
            return new RenamingResultDto(isSuccess: false, value: errorMessage);
        }
        #endregion
    }
}
