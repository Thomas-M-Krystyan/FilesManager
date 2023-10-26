using FilesManager.Core.Converters;
using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Files.Abstractions;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Services.Writing;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.ObjectModel;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    /// <summary>
    /// The strategy to update the file name by using prefix, incremented number, and postfix.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    internal sealed class IncrementNumberViewModel : StrategyBase<BasePathDto>
    {
        #region Texts
        public static readonly string Method_Header = Resources.Header_Method_IncrementNumber;
        public static readonly string Method_Tooltip = Resources.Tooltip_Method_IncrementNumber;

        public static readonly string Prefix_Label = Resources.Label_Prefix;
        public static readonly string Prefix_Tooltip = Resources.Tooltip_Prefix;
        public static readonly string StartNumber_Label = Resources.Label_StartNumber;
        public static readonly string StartNumber_Tooltip = Resources.Tooltip_StartNumber;
        public static readonly string Postfix_Label = Resources.Label_Postfix;
        public static readonly string Postfix_Tooltip = Resources.Tooltip_Postfix;
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        private string _namePrefix = string.Empty;
        public string NamePrefix
        {
            get => this._namePrefix;
            set
            {
                this._namePrefix = value;
                ValidateIllegalChars(nameof(this.NamePrefix), value);
                OnPropertyChanged(nameof(this.NamePrefix));
            }
        }

        private ushort _currentStartingNumber;                   // NOTE: Logic
        private string _startingNumber = DefaultStartingNumber;  // NOTE: UI
        public string StartingNumber
        {
            get => this._startingNumber;
            set
            {
                this._startingNumber = value;
                ValidateOnlyNumbers(nameof(this.StartingNumber), value, out this._currentStartingNumber);
                OnPropertyChanged(nameof(this.StartingNumber));
            }
        }

        private string _namePostfix = string.Empty;
        public string NamePostfix
        {
            get => this._namePostfix;
            set
            {
                this._namePostfix = value;
                ValidateIllegalChars(nameof(this.NamePostfix), value);
                OnPropertyChanged(nameof(this.NamePostfix));
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementNumberViewModel"/> class.
        /// </summary>
        internal IncrementNumberViewModel() : base()
        {
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

            // ---------------------------------
            // 2. Validate additional conditions
            // ---------------------------------
            if (this._currentStartingNumber + loadedFiles.Count - 1 > ushort.MaxValue)  // Exceeding the maximum possible value
            {
                // EXAMPLE: "startNumber" is 65530 and there is 6 files on the list. The result of ++ would be 65536 => which is more than maximum for ushort
                return RenamingResultDto.Failure(Resources.ERROR_Validation_Field_ValueWillExceedUshortMax);
            }

            // --------------------------------
            // 3. Process renaming of the files
            // --------------------------------
            var result = RenamingResultDto.Failure();
            FileData file;
            PathNameExtensionDto dto;

            for (int index = 0; index < loadedFiles.Count; index++)
            {
                file = loadedFiles[index];
                dto = FilePathConverter.GetPathNameExtension(file.Match);

                result = WritingService.RenameFile(file.Path, GetNewFilePath(dto));

                if (result.IsSuccess)
                {
                    UpdateFilesList(loadedFiles, index, () =>
                    {
                        file.Path = result.NewFilePath;
                        return file;
                    });
                }
                else
                {
                    loadedFiles.Clear();
                    break;
                }
            }

            // ------------------------------
            // 4. Finalization of the process
            // ------------------------------
            if (result.IsSuccess)
            {
                // Set the last number as the new start number
                this._currentStartingNumber = this._currentStartingNumber is ushort.MinValue
                    ? --this._currentStartingNumber  // Revert the effect of value overflow (ushort.MaxValue + 1 => 0). Keep the ushort.MaxValue
                    : this._currentStartingNumber;
            }

            return result;
        }

        /// <inheritdoc cref="StrategyBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.NamePrefix = string.Empty;
            this.StartingNumber = DefaultStartingNumber;
            this.NamePostfix = string.Empty;

            base.Reset();
        }

        /// <inheritdoc cref="StrategyBase{TFileDto}.GetNewFilePath(TFileDto)"/>
        protected internal override sealed string GetNewFilePath(BasePathDto fileDto)
        {
            return FilePathConverter.GetFilePath(
                path: fileDto.Path,
                name: $"{this.NamePrefix.GetValueOrEmpty()}" +
                      $"{this._currentStartingNumber++}" +      // NOTE: Very important! Keep incrementing the current number
                      $"{this.NamePostfix.GetValueOrEmpty()}",
                extension: fileDto.Extension);
        }
        #endregion
    }
}
