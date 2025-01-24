using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Base
{
    /// <inheritdoc cref="INotifyPropertyChanged"/>
    internal abstract class Observable : INotifyPropertyChanged
    {
        #region Events
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Observable"/> class.
        /// </summary>
        protected Observable()
        {
        }

        #region Protected
        /// <summary>
        /// Notifies a specific view model whether its property was changed from XAML code.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
