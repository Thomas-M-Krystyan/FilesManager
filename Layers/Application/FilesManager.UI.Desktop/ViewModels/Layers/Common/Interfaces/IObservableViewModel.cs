using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Common.Interfaces
{
    /// <inheritdoc cref="INotifyPropertyChanged"/>
    /// <remarks>
    /// Extends the interface with handler method notifying which view model property was changed.
    /// </remarks>
    internal interface IObservableViewModel : INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add => this.PropertyChanged += value;
            remove => this.PropertyChanged -= value;
        }

        /// <summary>
        /// Notifies a specific view model whether its property was changed from XAML code.
        /// </summary>
        internal void OnPropertyChanged(string propertyName);
    }
}
