using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Models.POCOs
{
    /// <summary>
    /// The POCO model representing <see cref="File"/> metadata.
    /// </summary>
    internal sealed record FileData
    {
        private static readonly IFilePathConverter<Match, FilePathNameDto> PathNameConverter
            = new FileDtoPathNameConverter();

        /// <inheritdoc cref="FilePathNameDto"/>
        internal FilePathNameDto Dto { get; }

        /// <summary>
        /// The full path to the file: <code>"C:\Users\JaneDoe\Desktop\Test.txt"</code>
        /// </summary>
        internal string FullPath => $"{this.Dto.Path}{this.Dto.Name}{this.Dto.Extension}";

        /// <summary>
        /// The file name with extension to be displayed on UI: <code>"Test.txt"</code>
        /// </summary>
        public string DisplayName => $"{this.Dto.Name}{this.Dto.Extension}";  // Used on the UI

        /// <summary>
        /// Initializes a new instance of the <see cref="FileData"/> class.
        /// </summary>
        internal FileData(FilePathNameDto dto)
        {
            this.Dto = dto;
        }

        /// <inheritdoc cref="FileData(FilePathNameDto)"/>
        internal FileData(Match match)
            : this(PathNameConverter.ConvertToDto(match))
        {
        }

        /// <inheritdoc cref="FileData(FilePathNameDto)"/>
        internal FileData(string newFilePath)
            : this(RegexPatterns.FileComponentsPattern().Match(newFilePath))
        {
        }
    }
}
