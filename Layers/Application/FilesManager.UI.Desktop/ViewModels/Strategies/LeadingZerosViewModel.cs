using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.ObjectModel;

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
        private const string Zero = "0";
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
        /// <inheritdoc cref="StrategyBase.Process(ObservableCollection{FileData})"/>
        internal override sealed RenamingResultDto Process(ObservableCollection<FileData> loadedFiles)
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
            foreach (FileData file in loadedFiles)
            {
                FileZerosDigitsDto dto = this._converter.ConvertToDto(file.Dto);

                // Counting max length
                this.MaxDigitsLength = Math.Max(this.MaxDigitsLength, dto.Digits.Length);
            }

            // --------------------------------
            // 3. Process renaming of the files
            // --------------------------------
            this.MaxDigitsLength = default;  // Reset value

            RenamingResultDto result = TryUpdatingFiles(loadedFiles, GetNewFilePath);

            return result;
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
            (string zeros, string digits) = GetDigitsWithLeadingZeros(fileData.Dto);

            return this._converter.GetFilePath(new
            (
                path: fileData.Dto.Path,
                zeros: zeros,
                digits: digits,
                name: fileData.Dto.Name,
                extension: fileData.Dto.Extension
            ));
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
        private (string Zeros, string Digits) GetDigitsWithLeadingZeros(FileZerosDigitsDto fileDto)
        {
            // There is no point to process the DTO any further
            if (this.HasErrors)
            {
                return (fileDto.Zeros, fileDto.Digits);
            }

            // Removing zeros mode
            if (this.LeadingZeros is 0)
            {
                if (fileDto.Digits.Length is 0 &&
                    fileDto.Name.Length   is 0 &&
                    (this.IsAbsoluteModeOn || this.MaxDigitsLength is 0))
                {
                    return (Zero, string.Empty);  // Cases: "00.jpg" => "0.jpg" (try to reduce number of zeros to none, but prevent ".jpg")
                }

                if (this.IsAbsoluteModeOn)
                {
                    return (string.Empty, fileDto.Digits);  // Cases: "01.jpg" => "1.jpg" or "01a.png" => "1a.png" (trim zeros)
                }
            }

            return GetLeadingZeros(fileDto.Digits);
        }

        private (string Zeros, string Digits) GetLeadingZeros(string digits)
        {
            int zerosToAdd = digits.Length == this.MaxDigitsLength           // For 2n => MIN: "1" (Length: 1), MAX: "10" (Length: 2)
                ? this.LeadingZeros                                          // (Count: 1) => "0"
                : this.MaxDigitsLength + this.LeadingZeros - digits.Length;  // 2 (max len) + 1 (count) - 1 (min len) => "00" + "1"

            return (new string(char.Parse(Zero), zerosToAdd), digits);
        }
        #endregion
    }
}
