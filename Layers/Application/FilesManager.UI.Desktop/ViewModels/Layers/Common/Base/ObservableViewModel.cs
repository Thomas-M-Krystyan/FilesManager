using FilesManager.UI.Desktop.ViewModels.Layers.Common.Interfaces;
using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Common.Base
{
    /// <inheritdoc cref="IObservableViewModel"/>
    internal abstract class ObservableViewModel : IObservableViewModel
    {
        /// <inheritdoc cref="IObservableViewModel.PropertyChanged"/>
        private event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc cref="IObservableViewModel.OnPropertyChanged(string)"/>
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
