using FilesManager.UI.Desktop.ViewModels.Base;
using Microsoft.Xaml.Behaviors.Core;
using System.Windows.Input;

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

        #region Commands
        /// <summary>
        /// Occurs when a click on some members of this view model was requested.
        /// </summary>
        public ICommand OnSelected => new ActionCommand(Select);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyBase"/> class.
        /// </summary>
        protected StrategyBase() : base()
        {
        }

        #region Abstract
        /// <summary>
        /// Selects some elements related to this strategy based on the click event from the XAML side.
        /// </summary>
        protected abstract void Select();
        #endregion
    }
}
