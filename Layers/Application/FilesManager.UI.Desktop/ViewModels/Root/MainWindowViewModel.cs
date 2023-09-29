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
        #region View Models
        public IncrementNumberViewModel IncrementNumberViewModel { get; set; }

        public PrependAppendViewModel PrependAppendViewModel { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        internal MainWindowViewModel() : base()
        {
            this.IncrementNumberViewModel = new IncrementNumberViewModel();
            this.PrependAppendViewModel = new PrependAppendViewModel();
        }

        #region Polymorphism
        /// <summary>
        /// Deselects all strategies.
        /// </summary>
        protected override sealed void Deselect()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            // TODO: if (!this.IncrementStratefy.IsEnabled) => OnDeselected();
            this.IncrementNumberViewModel.OnDeselected();
            this.PrependAppendViewModel.OnDeselected();
        }

        /// <summary>
        /// Resets all strategies.
        /// </summary>
        protected override sealed void Reset()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            this.IncrementNumberViewModel.OnReset();
            this.PrependAppendViewModel.OnReset();
        }
        #endregion
    }
}
