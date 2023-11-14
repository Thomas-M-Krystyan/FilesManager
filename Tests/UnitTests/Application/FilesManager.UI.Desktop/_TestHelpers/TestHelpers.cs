using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;

namespace FilesManager.UI.Desktop.UnitTests._TestHelpers
{
    internal static class TestHelpers
    {
        private static readonly IFilePathConverter<PathNameExtensionDto, PathZerosDigitsExtensionDto> Converter
            = new PathZerosDigitsExtensionConverter();

        internal static PathZerosDigitsExtensionDto GetTestDto(string testPath, string testName, string testExtension)
        {
            return Converter.ConvertToDto(new PathNameExtensionDto(testPath, testName, testExtension));
        }
    }
}
