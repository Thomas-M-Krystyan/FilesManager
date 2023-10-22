using FileManager.Layers.Logic;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.Generic;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    /// <summary>
    /// The strategy to update the file name by using prefix, incremented number, and postfix.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    internal sealed class IncrementNumberViewModel : StrategyBase
    {
        private const string DefaultStartingNumber = "0";

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

        private ushort _validStartingNumber;
        private string _startingNumber = DefaultStartingNumber;
        public string StartingNumber
        {
            get => this._startingNumber;
            set
            {
                this._startingNumber = value;
                ValidateOnlyNumbers(nameof(this.StartingNumber), value, out this._validStartingNumber);
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

            // ---------------------------------
            // 2. Validate additional conditions
            // ---------------------------------
            if (this._validStartingNumber + loadedFiles.Count - 1 > ushort.MaxValue)  // Exceeding the maximum possible value
            {
                // EXAMPLE: "startNumber" is 65530 and there is 6 files on the list. The result of ++ would be 65536 => which is more than maximum for ushort
                return RenamingResultDto.Failure(Resources.ERROR_Validation_Field_ValueWillExceedUshortMax);
            }

            // --------------------------------
            // 3. Process renaming of the files
            // --------------------------------
            var result = RenamingResultDto.Failure();

            for (int index = 0; index < loadedFiles.Count; index++)
            {
                FileData file = loadedFiles[index];
                result = RenamingService.ReplaceWithNumber(file.Path, this.NamePrefix, this._validStartingNumber++, this.NamePostfix);

                if (result.IsSuccess)
                {
                    // NOTE: Removing the old and inserting the modified item is necessary to notify ObservableCollection<T> about item updates
                    loadedFiles.RemoveAt(index);
                    file.Path = result.NewFilePath;
                    loadedFiles.Insert(index, file);
                }
                else
                {
                    loadedFiles.Clear();
                    break;
                }
            }

            if (result.IsSuccess)
            {
                // Set the last number as the new start number
                this._validStartingNumber = this._validStartingNumber is 0
                    ? --this._validStartingNumber  // Revert the effect of value overflow (ushort.MaxValue + 1 => 0). Keep the ushort.MaxValue
                    : this._validStartingNumber;
            }

            return result;
        }

        /// <inheritdoc cref="StrategyBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.NamePrefix = string.Empty;
            this.StartingNumber = DefaultStartingNumber;
            this._validStartingNumber = default;
            this.NamePostfix = string.Empty;

            base.Reset();
        }
        #endregion

        #region Validation
        private void ValidateIllegalChars(string propertyName, string value)
        {
            _ = Validate.ContainInvalidCharacters(value,
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ContainsIllegalCharacter, value),
                () => ClearErrors(propertyName));
        }

        private void ValidateOnlyNumbers(string propertyName, string value, out ushort validStartingNumber)
        {
            _ = Validate.IsUshort(value, out validStartingNumber,
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ContainsNotDigits, value),
                () => ClearErrors(propertyName));
        }
        #endregion
    }
}
