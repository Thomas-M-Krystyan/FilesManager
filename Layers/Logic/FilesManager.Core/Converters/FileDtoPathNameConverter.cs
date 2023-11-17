using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Converters
{
    /// <inheritdoc cref="IFilePathConverter{TData, TFileDto}"/>
    internal sealed class FileDtoPathNameConverter : IFilePathConverter<Match, FilePathNameDto>
    {
        /// <inheritdoc cref="IFilePathConverter{TData, TFileDto}.ConvertToDto(TData)"/>
        FilePathNameDto IFilePathConverter<Match, FilePathNameDto>.ConvertToDto(Match data)
        {
            return new FilePathNameDto(
                path:      data.Value(RegexPatterns.PathGroup),
                name:      data.Value(RegexPatterns.NameGroup),
                extension: data.Value(RegexPatterns.ExtensionGroup));
        }

        /// <inheritdoc cref="IFilePathConverter{TData, TFileDto}.GetFilePath(TFileDto)"/>
        string IFilePathConverter<Match, FilePathNameDto>.GetFilePath(FilePathNameDto dto)
        {
            return dto.Name.IsEmptyOrWhiteSpaces()
                ? string.Empty
                : Path.Combine(dto.Path, dto.Name + dto.Extension);
        }
    }
}
