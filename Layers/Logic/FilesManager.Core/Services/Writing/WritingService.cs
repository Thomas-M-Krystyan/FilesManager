using FilesManager.Core.Models.DTOs.Results;

namespace FilesManager.Core.Services.Writing
{
    internal static class WritingService
    {
        /// <summary>
        /// Renames the given file using a specified renaming method.
        /// </summary>
        /// <param name="oldFilePath">The original file path.</param>
        /// <param name="newFilePath">The new file path to be used.</param>
        /// <returns>Result of renamed file.</returns>
        internal static RenamingResultDto RenameFile(string oldFilePath, string newFilePath)
        {
            // TODO: Introduce backups for files
            try
            {
                File.Move(oldFilePath, newFilePath);

                return RenamingResultDto.Success(newFilePath);
            }
            catch (Exception exception)  // 8 types of exceptions
            {
                return RenamingResultDto.Failure(exception.Message);
            }
        }
    }
}
