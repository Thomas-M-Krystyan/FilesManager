using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies;

namespace FilesManager.UI.Desktop.ViewModels.Root
{
    /// <summary>
    /// View model for the main window of the application.
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        // View Models
        public IncrementNumberViewModel IncrementStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        internal MainWindowViewModel() : base()
        {
            this.IncrementStrategy = new IncrementNumberViewModel();
        }

        #region Polymorphism
        /// <summary>
        /// Deselects all strategies.
        /// </summary>
        protected override sealed void Deselect()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            this.IncrementStrategy.OnDeselected();
        }

        /// <summary>
        /// Resets all strategies.
        /// </summary>
        protected override sealed void Reset()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            this.IncrementStrategy.OnReset();
        }
        #endregion
    }
}
