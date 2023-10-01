using FilesManager.Core.Models.DTOs.Results;

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
            try
            {
                string newFilePath = renameMethod();

                if (String.IsNullOrEmpty(newFilePath))
                {
                    throw new ArgumentException($"Cannot rename the file \"{oldFilePath}\"");
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
