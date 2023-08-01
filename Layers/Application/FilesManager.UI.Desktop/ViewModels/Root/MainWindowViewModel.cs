using FilesManager.UI.Desktop.ViewModels.Strategies;

namespace FilesManager.UI.Desktop.ViewModels.Root
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        public IncrementNumberViewModel IncrementStrategy { get; set; }

        internal MainWindowViewModel()
        {
            this.IncrementStrategy = new IncrementNumberViewModel();
        }
    }
}
