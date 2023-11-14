using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Converters
{
    /// <inheritdoc cref="IFilePathConverter{TData, TFileDto}"/>
    internal sealed class PathZerosDigitsExtensionConverter : IFilePathConverter<PathNameExtensionDto, PathZerosDigitsExtensionDto>
    {
        /// <inheritdoc cref="IFilePathConverter{TData, TFileDto}.ConvertToDto(TData)"/>
        PathZerosDigitsExtensionDto IFilePathConverter<PathNameExtensionDto, PathZerosDigitsExtensionDto>
            .ConvertToDto(PathNameExtensionDto data)
        {
            // Split the file name into dedicated zeros, digits, and name groups
            Match digitsNameMatch = RegexPatterns.DigitsNamePattern().Match(data.Name);

            return new PathZerosDigitsExtensionDto(
                path:      data.Path,
                zeros:     digitsNameMatch.Value(RegexPatterns.ZerosGroup),
                digits:    digitsNameMatch.Value(RegexPatterns.DigitsGroup),
                name:      digitsNameMatch.Value(RegexPatterns.NameGroup),
                extension: data.Extension);
        }

        /// <inheritdoc cref="IFilePathConverter{TData, TFileDto}.GetFilePath(TFileDto)"/>
        string IFilePathConverter<PathNameExtensionDto, PathZerosDigitsExtensionDto>.GetFilePath(PathZerosDigitsExtensionDto dto)
        {
            return (dto.Zeros + dto.Digits + dto.Name).IsEmptyOrWhiteSpaces()
                ? string.Empty
                : Path.Combine(dto.Path, dto.Zeros + dto.Digits + dto.Name + dto.Extension);
        }
    }
}
