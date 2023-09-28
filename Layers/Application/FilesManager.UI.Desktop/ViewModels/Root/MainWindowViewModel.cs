using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies;

namespace FilesManager.UI.Desktop.ViewModels.Root
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        // View Models
        public IncrementNumberViewModel IncrementStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        internal MainWindowViewModel()
        {
            this.IncrementStrategy = new IncrementNumberViewModel();
        }
    }
}
