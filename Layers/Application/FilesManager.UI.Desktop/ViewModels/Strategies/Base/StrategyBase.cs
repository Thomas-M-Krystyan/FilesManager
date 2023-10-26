using FilesManager.Core.Models.DTOs.Files.Abstractions;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.Utilities;
using FilesManager.UI.Desktop.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace FilesManager.UI.Desktop.ViewModels.Strategies.Base
{
    /// <summary>
    /// Base class for all file manipulation strategies.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    internal abstract class StrategyBase<TFileDto> : ViewModelBase
        where TFileDto : BasePathDto
    {
        #region Texts
        public static readonly string RadioButton_Tooltip = Resources.Tooltip_RadioButton;

        public static readonly string Content_NonEmptyField_Tooltip = Resources.Tooltip_Tip_Content_NonEmptyField;
        public static readonly string Content_OnlyPositives_Tooltip = Resources.Tooltip_Tip_Content_OnlyPositives;
        public static readonly string Content_OnlyVerySmallPositives_Tooltip = Resources.Tooltip_Tip_Content_OnlyVerySmallPositives;
        #endregion

        #region Const
        protected const string DefaultStartingNumber = "0";
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        private bool _isEnabled = false;

        /// <summary>
        /// Determines whether this strategy is activated / enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                OnPropertyChanged(nameof(this.IsEnabled));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs BEFORE the <see cref="Select()"/> - when it was requested on this view model.
        /// </summary>
        public event Action? BeforeOnSelected;

        /// <summary>
        /// Occurs AFTER the <see cref="Select()"/> - when it was requested on this view model.
        /// </summary>
        public event Action<object>? AfterOnSelected;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyBase"/> class.
        /// </summary>
        protected StrategyBase() : base()
        {
        }

        #region Polymorphism
        /// <inheritdoc cref="ViewModelBase.Select(object)"/>
        protected override sealed void Select(object parameter)  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            if (!this.IsEnabled)
            {
                this.BeforeOnSelected?.Invoke();  // NOTE: Subscribed action to be invoked BEFORE the selection
                this.IsEnabled = true;
                this.AfterOnSelected?.Invoke(parameter);  // NOTE: Subscribed action to be invoked AFTER the selection
            }
        }

        /// <inheritdoc cref="ViewModelBase.Deselect()"/>
        protected override sealed void Deselect()  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            if (this.IsEnabled)
            {
                this.IsEnabled = false;
            }
        }

        /// <inheritdoc cref="ViewModelBase.Reset()"/>
        protected override void Reset()
        {
            Deselect();
            ClearAllErrors();
        }
        #endregion

        #region Abstract
        /// <summary>
        /// Executes logic related to this specific strategy.
        /// </summary>
        /// <param name="loadedFiles">
        ///   The collection of loaded files, dragged and dropped into dedicated UI section.
        ///   <para>
        ///     It will never be null or empty.
        ///   </para>
        /// </param>
        /// <returns>
        ///   <inheritdoc cref="RenamingResultDto" path="/summary"/>
        /// </returns>
        internal abstract RenamingResultDto Process(ObservableCollection<FileData> loadedFiles);

        /// <summary>
        /// Gets the new file path for a file to be renamed.
        /// </summary>
        /// <param name="fileDto">The file DTO composed from RegEx <see cref="Match"/>.</param>
        /// <returns>
        ///   The new file path (changed by the current renaming strategy).
        /// </returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        protected internal abstract string GetNewFilePath(TFileDto fileDto);
        #endregion

        #region Virtual
        /// <summary>
        /// Displays a proper <see cref="MessageBoxResult"/> popup with feedback information.
        /// </summary>
        /// <param name="result">The result of processing operation.</param>
        internal virtual void DisplayPopup(RenamingResultDto result)
        {
            _ = result.IsSuccess
                ? Message.InfoOk(Resources.RESULT_Operation_Success_Header, Resources.RESULT_Operation_Success_Text)
                : Message.ErrorOk(Resources.RESULT_Operation_Failure, result.Message);
        }
        #endregion

        #region Concrete (Validation)
        /// <summary>
        /// Validates if the value of the specified property contains any illegal characters.
        /// <para>
        ///   A specific <see cref="Action"/>s will be invoked in case of success or failure.
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        protected void ValidateIllegalChars(string propertyName, string propertyValue)
        {
            _ = Validate.ContainInvalidCharacters(propertyValue,
                () => ClearErrors(propertyName),
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ContainsIllegalCharacter, propertyValue));
        }

        /// <summary>
        /// Validates if the value of the specified property contains only numbers (optional: within a specified maximum range).
        /// <para>
        ///   A specific <see cref="Action"/>s will be invoked in case of success or failure.
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        /// <param name="number">The result of string to <paramref name="TNumber"/> conversion.</param>
        /// <param name="maxLimit">The maximum limit of the output number.</param>
        protected void ValidateOnlyNumbers<TNumber>(string propertyName, string propertyValue, out TNumber number, TNumber? maxLimit = null)
            where TNumber : struct, IComparable, IComparable<TNumber>, IConvertible, IEquatable<TNumber>, IFormattable  // NOTE: Numeric type
        {
            bool isSuccess = Validate.Is(propertyValue, out number,
                () => ClearErrors(propertyName),
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ContainsNotDigits, propertyValue));

            if (isSuccess && maxLimit != null)
            {
                _ = Validate.WithinLimit(number, maxLimit.Value,
                () => ClearErrors(propertyName),
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ExceedsMaxLimit, maxLimit));
            }
        }
        #endregion

        #region Concrete (Logic)
        /// <summary>
        /// Updates the <see cref="ObservableCollection{T}"/> of files.
        /// </summary>
        /// <param name="loadedFiles">The list of files to by updated.</param>
        /// <param name="index">The index of the element to be modified.</param>
        /// <param name="updateLogic">The specific logic how to update selected element.</param>
        protected internal static void UpdateFilesList(ObservableCollection<FileData> loadedFiles, int index, Func<FileData> updateLogic)
        {
            loadedFiles.RemoveAt(index);  // NOTE: Triggers OnCollectionChanged event

            // Update the file
            FileData file = updateLogic.Invoke();

            loadedFiles.Insert(index, file);  // NOTE: Triggers OnCollectionChanged event
        }
        #endregion
    }
}
