using FilesManager.UI.Desktop.ViewModels.Strategies;

namespace FilesManager.UI.Desktop.ViewModels.Root
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        #region View Models
        public IncrementNumberViewModel IncrementStrategy { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        internal MainWindowViewModel()
        {
            this.IncrementStrategy = new IncrementNumberViewModel();
        }
    }
}
