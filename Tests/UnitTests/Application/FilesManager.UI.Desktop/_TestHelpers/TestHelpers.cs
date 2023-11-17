using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;

namespace FilesManager.UI.Desktop.UnitTests._TestHelpers
{
    internal static class TestHelpers
    {
        private static readonly IFilePathConverter<FilePathNameDto, FileZerosDigitsDto> Converter
            = new FileDtoZerosDigitsConverter();

        internal static FileZerosDigitsDto GetTestDto(string testPath, string testName, string testExtension)
        {
            return Converter.ConvertToDto(new FilePathNameDto(testPath, testName, testExtension));
        }
    }
}
