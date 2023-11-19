using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.Generic;
using System.Linq;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    /// <summary>
    /// The strategy to update the file name by appending to it leading zeroes.
    /// </summary>
    /// <seealso cref="StrategyBase"/>
    internal sealed class LeadingZerosViewModel : StrategyBase
    {
        private readonly IFilePathConverter<FilePathNameDto, FileZerosDigitsDto> _converter;

        #region Texts
        public static readonly string Method_Header = Resources.Header_Method_LeadingZeros;
        public static readonly string Method_Tooltip = Resources.Tooltip_Method_LeadingZeros;

        public static readonly string Set_Label = Resources.Label_Set;
        public static readonly string Set_Tooltip = Resources.Tooltip_Set;

        public static readonly string ClearZeros_Tooltip = Resources.Tooltip_ClearZeros;
        #endregion

        #region Const
        private const bool DefaultAbsoluteModeOn = false;
        private const bool DefaultAbsoluteModeEnabled = true;
        private const string SingleZero = "0";
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        private byte _leadingZeros;
        public byte LeadingZeros
        {
            get => this._leadingZeros;
            set
            {
                this._leadingZeros = value;
                ValidateNumMaxLimit(nameof(this.LeadingZeros), value, maxLimit: 7);
                OnPropertyChanged(nameof(this.LeadingZeros));

                // NOTE: The checkbox should be enabled only for 0 value
                this.IsAbsoluteModeEnabled = this._leadingZeros == ushort.MinValue;
            }
        }

        private bool _isAbsoluteModeOn = DefaultAbsoluteModeOn;
        public bool IsAbsoluteModeOn
        {
            get => this._isAbsoluteModeOn;
            set
            {
                this._isAbsoluteModeOn = value;
                OnPropertyChanged(nameof(this.IsAbsoluteModeOn));
            }
        }

        private bool _isAbsoluteModeEnabled = DefaultAbsoluteModeEnabled;

        /// <summary>
        /// Determines whether <see cref="IsAbsoluteModeOn"/> checkbox is enabled.
        /// </summary>
        public bool IsAbsoluteModeEnabled
        {
            get => this._isAbsoluteModeEnabled;
            set
            {
                this._isAbsoluteModeEnabled = value;
                OnPropertyChanged(nameof(this.IsAbsoluteModeEnabled));

                // NOTE: The disabled checkbox should have false value
                if (!this._isAbsoluteModeEnabled)
                {
                    this.IsAbsoluteModeOn = DefaultAbsoluteModeOn;
                }
            }
        }
        #endregion

        #region Properties
        internal int MaxDigitsLength { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LeadingZerosViewModel"/> class.
        /// </summary>
        internal LeadingZerosViewModel() : base()
        {
            this._converter = new FileDtoZerosDigitsConverter();
        }

        #region Polymorphism
        /// <inheritdoc cref="StrategyBase.Process(IList{FileData})"/>
        internal override sealed RenamingResultDto Process(IList<FileData> loadedFiles)
        {
            // -----------------------------------------
            // 1. Validate if there are any input errors
            // -----------------------------------------
            if (this.HasErrors)
            {
                return RenamingResultDto.Failure(GetAllErrors());
            }

            // --------------------------------
            // 2. Preparing the processing data
            // --------------------------------
            this.MaxDigitsLength = default;  // Reset value

            for (int index = 0; index < loadedFiles.Count; index++)
            {
                // Counting max length
                this.MaxDigitsLength = Math.Max(this.MaxDigitsLength,
                    this._converter.ConvertToDto(loadedFiles[index].Dto).Digits.Length);
            }

            // --------------------------------
            // 3. Process renaming of the files
            // --------------------------------
            return TryUpdatingFiles(loadedFiles, GetNewFilePath);
        }

        /// <inheritdoc cref="StrategyBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.LeadingZeros = default;
            this.IsAbsoluteModeOn = DefaultAbsoluteModeOn;

            base.Reset();
        }

        /// <inheritdoc cref="StrategyBase.GetNewFilePath(FileData)"/>
        protected internal override sealed string GetNewFilePath(FileData fileData)
        {
            // Conversion (split the "Name" into: "Zeros" + "Digits" + "Name")
            FileZerosDigitsDto zerosFileDataDto = this._converter.ConvertToDto(fileData.Dto);

            // Regulate the amount of leading "Zeros" + unchanged "Digits"
            zerosFileDataDto = GetDigitsWithLeadingZeros(zerosFileDataDto);

            // Retrieve the full filename from DTO
            return this._converter.GetFilePath(zerosFileDataDto);
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Adds the leading zeros to the beginning of the file.
        /// </summary>
        /// <param name="file">The file DTO with extracted parts of the file name (zeros or digits).</param>
        /// <returns>
        ///   The new file name preceeded by zeros.
        /// </returns>
        private FileZerosDigitsDto GetDigitsWithLeadingZeros(FileZerosDigitsDto fileDto)
        {
            // Removing "Zeros" mode
            if (this.LeadingZeros is 0)
            {
                if (fileDto.Digits.Length is 0 &&
                    fileDto.Name.Length is 0 &&
                    (this.IsAbsoluteModeOn || this.MaxDigitsLength is 0))
                {
                    // Cases: "00.jpg" => "0.jpg" (try to reduce number of "Zeros" to none, but prevent ".jpg")
                    return new FileZerosDigitsDto(fileDto, SingleZero, string.Empty);
                }

                if (this.IsAbsoluteModeOn)
                {
                    // Cases: "01.jpg" => "1.jpg" or "01a.png" => "1a.png" (trim "Zeros")
                    return new FileZerosDigitsDto(fileDto, string.Empty, fileDto.Digits);
                }
            }

            // Adding leading "Zeros" mode
            return GetLeadingZeros(fileDto);
        }

        private FileZerosDigitsDto GetLeadingZeros(FileZerosDigitsDto fileDto)
        {
            // For case "add one 0" where the shortest file name is "1" (min len) and the longest is "10" (max len)
            int zerosToAdd = fileDto.Digits.Length == this.MaxDigitsLength // NOTE: digits length can be never greater than max digits length
                // For the name "10" (from the above case): all additional zeros will be prepended => "010"
                ? this.LeadingZeros
                // For the name "1" (from the above case): first the zeros alignment should happen "1" => "01" (max len 2 from "10")
                //                                         and only after that the additional zeros can be prepended => "001"
                : this.MaxDigitsLength - fileDto.Digits.Length + this.LeadingZeros;
            
            string newZeros = new(char.Parse(SingleZero), zerosToAdd);

            return new FileZerosDigitsDto(fileDto, newZeros, fileDto.Digits);
        }
        #endregion
    }
}
