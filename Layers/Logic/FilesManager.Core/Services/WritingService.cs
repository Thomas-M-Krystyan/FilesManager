using FilesManager.Core.Models.DTOs.Results;
using FilesManager.UI.Common.Properties;

namespace FilesManager.Core.Services
{
    internal static class WritingService
    {
        /// <summary>
        /// Renames the given file using a specified renaming method.
        /// </summary>
        /// <param name="oldFilePath">The original file path.</param>
        /// <param name="renameMethod">The rename method to be used.</param>
        /// <returns>Result of renamed file.</returns>
        internal static RenamingResultDto RenameFile(string oldFilePath, Func<string> renameMethod)
        {
            // TODO: Introduce backups for files
            try
            {
                string newFilePath = renameMethod();

                if (string.IsNullOrWhiteSpace(newFilePath))
                {
                    return RenamingResultDto.Failure(Resources.ERROR_Validation_File_NotRenamed + $" \"{oldFilePath}\"");
                }

                File.Move(oldFilePath, newFilePath);

                return RenamingResultDto.Success(newFilePath);
            }
            catch (Exception exception)
            {
                return RenamingResultDto.Failure(exception.Message);
            }
        }
    }
}
