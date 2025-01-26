using FilesManager.UI.Desktop.ViewModels.Layers.Common.Interfaces;
using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Common.Base
{
    /// <summary>
    /// <inheritdoc cref="IObservableViewModel"/>
    /// </summary>
    /// <remarks>
    /// NOTE: Interfaces cannot access their own events from implemented
    /// methods, so this class was introduced to omit such restriction.
    /// </remarks>
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
