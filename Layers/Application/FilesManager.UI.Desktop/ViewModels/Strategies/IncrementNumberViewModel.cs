using FilesManager.UI.Desktop.Properties;
using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;

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

        #region Properties
        private string _namePrefix = string.Empty;
        public string NamePrefix
        {
            get => this._namePrefix;
            set
            {
                this._namePrefix = value;
                OnPropertyChanged(nameof(this.NamePrefix));
            }
        }

        private string _startingNumber = string.Empty;
        public string StartingNumber
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

        /// <summary>
        /// Renames given files using incremented start number and optional postfix.
        /// </summary>
        //private void RenameWithIncrementedNumber()
        //{
        //    // Validate strings which are going to be used in file name
        //    RenamingResultDto result = ValidateIllegalCharacters(this.NamePrefix, this.NamePostfix);

        //    if (result.IsSuccess)
        //    {
        //        // Validate null or empty input
        //        if (String.IsNullOrWhiteSpace(this.StartingNumber))
        //        {
        //            result = RenamingResultDto.Failure("Provide \"Start number\".");
        //        }
        //        else
        //        {
        //            // Validate input value (cannot be converted or it's too large)
        //            if (UInt16.TryParse(this.StartingNumber, out ushort startNumber))
        //            {
        //                // Validate input value (consecutive numbers would exceed the allowed maximum)
        //                if (startNumber + this.FilesList.Items.Count - 1 <= UInt16.MaxValue)  // NOTE: "startNumber" will be the first to use, that's why Count - 1
        //                {
        //                    // Process renaming of the file
        //                    foreach (ListBoxItem fileItem in this.FilesList.Items)
        //                    {
        //                        result = RenamingService.ReplaceWithNumber((string)fileItem.ToolTip, this.NamePrefix, startNumber++, this.NamePostfix);

        //                        // Validate renaming result
        //                        if (!result.IsSuccess)
        //                        {
        //                            ClearFilesList();

        //                            break;
        //                        }

        //                        UpdateNameOnList(fileItem, result.NewFilePath);
        //                    }

        //                    if (result.IsSuccess)
        //                    {
        //                        // Set the last number as the new start number
        //                        this.StartingNumber = startNumber.Equals(0)
        //                            ? (--startNumber).ToString()  // Revert the effect of "startNumber" value overflow (UInt16.MaxValue + 1 => 0)
        //                            : startNumber.ToString(CultureInfo.InvariantCulture);
        //                    }
        //                }
        //                else
        //                {
        //                    result = RenamingResultDto.Failure($"Some numbers would exceed the max value for \"Start number\" ({UInt16.MaxValue}) if the renaming continue incrementally.");
        //                }
        //            }
        //            else
        //            {
        //                result = RenamingResultDto.Failure($"Invalid \"Start number\" value: {this.StartingNumber}.");
        //            }
        //        }
        //    }

        //    DisplayPopup(result);
        //}

        #region Polymorphism
        /// <inheritdoc cref="StrategyBase.Process()"/>
        internal override sealed void Process()
        {
        }

        /// <inheritdoc cref="ViewModelBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            Deselect();

            this.NamePrefix = string.Empty;
            this.StartingNumber = string.Empty;
            this.NamePostfix = string.Empty;
        }
        #endregion
    }
}
