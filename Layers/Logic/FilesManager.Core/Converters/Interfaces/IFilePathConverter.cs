using FilesManager.Core.Models.DTOs.Files;

namespace FilesManager.Core.Converters.Interfaces
{
    /// <summary>
    /// The interface handling conversion from <typeparamref name="TData"/> to <typeparamref name="TFileDto"/> and back.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TFileDto">The type of the file DTO.</typeparam>
    internal interface IFilePathConverter<TData, TFileDto>
        where TData    : class
        where TFileDto : FilePathNameDto
    {
        /// <summary>
        /// Converts the given data into a specific <typeparamref name="TFileDto"/>.
        /// </summary>
        /// <param name="data">The specific type of data to be converted into <typeparamref name="TFileDto"/>.</param>
        /// <returns>
        ///   The dedicated <typeparamref name="TFileDto"/>.
        /// </returns>
        internal TFileDto ConvertToDto(TData data);

        /// <summary>
        /// Converts path + name + extension back into a consolidated file's <see cref="Path"/>.
        /// </summary>
        /// <param name="dto">The specific <typeparamref name="TFileDto"/> to be used for data retrieval.</param>
        /// <returns>
        ///   The absolute file path: "C:\Folder\Subfolder\File.txt".
        /// </returns>
        internal string GetFilePath(TFileDto dto);
    }
}
