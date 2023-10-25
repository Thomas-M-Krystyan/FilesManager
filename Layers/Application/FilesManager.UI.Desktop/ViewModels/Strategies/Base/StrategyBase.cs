using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.Utilities;
using FilesManager.UI.Desktop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace FilesManager.UI.Desktop.ViewModels.Strategies.Base
{
    /// <summary>
    /// Base class for all file manipulation strategies.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    internal abstract class StrategyBase : ViewModelBase
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
        internal abstract RenamingResultDto Process(IList<FileData> loadedFiles);

        /// <summary>
        /// Gets the new file path for a file to be renamed.
        /// </summary>
        /// <param name="filePathMatch">The file path RegEx match.</param>
        /// <returns>
        ///   The new file path (changed by the current renaming strategy).
        /// </returns>
        protected internal abstract string GetNewFilePath(Match filePathMatch);
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
        protected void ValidateIllegalChars(string propertyName, string value)
        {
            _ = Validate.ContainInvalidCharacters(value,
                () => ClearErrors(propertyName),
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ContainsIllegalCharacter, value));
        }

        protected void ValidateOnlyNumbers(string propertyName, string value, out ushort validStartingNumber, int? maxLimit = null)
        {
            bool isSuccess = Validate.IsUshort(value, out validStartingNumber,
                () => ClearErrors(propertyName),
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ContainsNotDigits, value));

            if (isSuccess && maxLimit != null)
            {
                _ = Validate.WithinLimit(validStartingNumber, maxLimit.Value,
                () => ClearErrors(propertyName),
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ExceedsMaxLimit, maxLimit));
            }
        }
        #endregion
    }
}
