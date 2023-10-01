using FilesManager.UI.Desktop.Properties;
using FilesManager.UI.Desktop.ViewModels.Base;
using System;

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

        #region Properties
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
        internal abstract void Process();
        #endregion
    }
}
