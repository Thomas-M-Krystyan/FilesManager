using FilesManager.UI.Desktop.ViewModels.Base;

namespace FilesManager.UI.Desktop.ViewModels.Strategies.Base
{
    /// <summary>
    /// Base class for all file manipulation strategies.
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    internal abstract class StrategyBase : ViewModelBase
    {
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

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyBase"/> class.
        /// </summary>
        protected StrategyBase() : base()
        {
        }

        #region Event handlers
        /// <inheritdoc cref="ViewModelBase.OnReset"/>
        public new void OnSelected()
        {
            if (base.OnSelected.CanExecute(null))
            {
                base.OnSelected.Execute(null);
            }
        }

        /// <inheritdoc cref="ViewModelBase.OnReset"/>
        public new void OnDeselected()
        {
            if (base.OnDeselected.CanExecute(null))
            {
                base.OnDeselected.Execute(null);
            }
        }
        #endregion

        #region Polymorphism
        /// <inheritdoc cref="ViewModelBase.Select()"/>
        protected override sealed void Select()  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            this.IsEnabled = true;
        }

        /// <inheritdoc cref="ViewModelBase.Deselect()"/>
        protected override sealed void Deselect()  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            this.IsEnabled = false;
        }
        #endregion
    }
}
