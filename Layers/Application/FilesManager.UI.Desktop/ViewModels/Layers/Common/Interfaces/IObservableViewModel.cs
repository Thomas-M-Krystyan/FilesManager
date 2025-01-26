using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Common.Interfaces
{
    /// <summary>
    /// <inheritdoc cref="INotifyPropertyChanged"/>
    /// <para>
    /// NOTE: Extends the <see cref="INotifyPropertyChanged"/> interface
    /// with handler method notifying which view model property was changed.
    /// </para>
    /// </summary>
    internal interface IObservableViewModel : INotifyPropertyChanged
    {
        #region Events
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add    => this.PropertyChanged += value;
            remove => this.PropertyChanged -= value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Notifies a specific view model whether its property was changed from XAML code.
        /// </summary>
        internal void OnPropertyChanged(string propertyName);
        #endregion
    }
}
