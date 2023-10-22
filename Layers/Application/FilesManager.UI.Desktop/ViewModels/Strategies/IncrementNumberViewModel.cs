using FileManager.Layers.Logic;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Desktop.Properties;
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
                ValidateProperty(nameof(this.NamePrefix), value);
                OnPropertyChanged(nameof(this.NamePrefix));
            }
        }

        private ushort _startingNumber;
        public ushort StartingNumber
        {
            get => this._startingNumber;
            set
            {
                this._startingNumber = value;
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
                ValidateProperty(nameof(this.NamePostfix), value);
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
            // ----------------------------------
            // 1. Validate mandatory input fields
            // ----------------------------------
            if (this.StartingNumber + loadedFiles.Count - 1 > ushort.MaxValue)  // Exceeding the maximum possible value
            {
                // EXAMPLE: "startNumber" is 65530 and there is 6 files on the list. The result of ++ would be 65536 => which is more than maximum for ushort
                return RenamingResultDto.Failure($"Some numbers would eventually exceed the max value for \"Start number\" (65535) if the renaming continue.");
            }

            // -----------------------------------------
            // 2. Validate if there are any input errors
            // -----------------------------------------
            if (this.HasErrors)
            {
                return RenamingResultDto.Failure(GetAllErrors());
            }

            // --------------------------------
            // 3. Process renaming of the files
            // --------------------------------
            var result = RenamingResultDto.Failure();

            for (int index = 0; index < loadedFiles.Count; index++)
            {
                FileData file = loadedFiles[index];
                result = RenamingService.ReplaceWithNumber(file.Path, this.NamePrefix, this.StartingNumber++, this.NamePostfix);

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
                this.StartingNumber = this.StartingNumber is 0
                    ? --this.StartingNumber  // Revert the effect of value overflow (ushort.MaxValue + 1 => 0)
                    : this.StartingNumber;
            }

            return result;
        }

        /// <inheritdoc cref="StrategyBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.NamePrefix = string.Empty;
            this.StartingNumber = default;
            this.NamePostfix = string.Empty;

            base.Reset();
        }
        #endregion

        #region Helper methods
        private void ValidateProperty(string propertyName, string value)
        {
            if (Validate.HaveInvalidCharacters(value))
            {
                AddError(propertyName, Resources.ERROR_Validation_IllegalCharacter + $" {value}");
            }
            else
            {
                ClearErrors(propertyName);
            }
        }
        #endregion
    }
}
