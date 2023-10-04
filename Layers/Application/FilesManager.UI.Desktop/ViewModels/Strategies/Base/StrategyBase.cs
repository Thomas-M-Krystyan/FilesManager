using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Desktop.Properties;
using FilesManager.UI.Desktop.Utilities;
using FilesManager.UI.Desktop.ViewModels.Base;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

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
        public event Action? AfterOnSelected;
        #endregion

        #region Commands (binding)
        /// <summary>
        /// Handles subscribed <see cref="ValidateNumericInput(object)"/> action.
        /// </summary>
        public ICommand ValidNumInputCommand => new ActionCommand(ValidateNumericInput);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyBase"/> class.
        /// </summary>
        protected StrategyBase() : base()
        {
        }

        #region Polymorphism
        /// <inheritdoc cref="ViewModelBase.Select()"/>
        protected override sealed void Select()  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            if (!this.IsEnabled)
            {
                this.BeforeOnSelected?.Invoke();  // NOTE: Subscribed action to be invoked BEFORE the selection
                this.IsEnabled = true;
                this.AfterOnSelected?.Invoke();  // NOTE: Subscribed action to be invoked AFTER the selection
            }
        }

        /// <inheritdoc cref="ViewModelBase.Deselect()"/>
        protected override sealed void Deselect()  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            this.IsEnabled = false;
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
        internal abstract RenamingResultDto Process(ObservableCollection<FileData> loadedFiles);
        #endregion

        #region Virtual
        /// <summary>
        /// Displays a proper <see cref="MessageBoxResult"/> popup with feedback information.
        /// </summary>
        /// <param name="result">The result of processing operation.</param>
        internal virtual void DisplayPopup(RenamingResultDto result)
        {
            _ = result.IsSuccess
                ? Message.InfoOk("Operation successful", "All files were renamed!")
                : Message.ErrorOk("Operation failed", result.Message);
        }
        #endregion

        #region Private
        /// <summary>
        /// Validates whether the provided input contains only numeric values.
        /// </summary>
        /// <param name="parameter">
        ///   The event's argument (<see cref="TextCompositionEventArgs"/>) passed
        ///   when the specific trigger was activated on the XAML side.
        /// </param>
        private void ValidateNumericInput(object parameter)
        {
            if (parameter is TextCompositionEventArgs textInputEvent)
            {
                // Prevents providing even a single non-digit character in the guarded text box field
                textInputEvent.Handled = RegexPatterns.NotDigit.IsMatch(textInputEvent.Text);
            }
            else
            {
                ReportInvalidCommandUsage(nameof(ValidateNumericInput));
            }
        }
        #endregion
    }
}
