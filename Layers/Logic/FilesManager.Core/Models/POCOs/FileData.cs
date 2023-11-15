using FilesManager.Core.Models.DTOs.Files;

namespace FilesManager.Core.Models.POCOs
{
    /// <summary>
    /// The POCO model representing <see cref="File"/> metadata.
    /// </summary>
    internal sealed record FileData
    {
        /// <inheritdoc cref="PathNameExtensionDto"/>
        internal PathNameExtensionDto Dto { get; }

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
        internal FileData(PathNameExtensionDto dto)
        {
            this.Dto = dto;
        }
    }
}
