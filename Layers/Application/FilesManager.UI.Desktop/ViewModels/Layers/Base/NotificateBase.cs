using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Base
{
    /// <inheritdoc cref="INotifyPropertyChanged"/>   => Notification
    internal abstract class NotificateBase : INotifyPropertyChanged
    {
        #region Events
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificateBase"/> class.
        /// </summary>
        protected NotificateBase() : base()
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
