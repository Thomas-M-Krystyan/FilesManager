using FilesManager.UI.Desktop.Properties;
using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels
{
    /// <summary>
    /// Base class for all view models in MVVM architecture.
    /// </summary>
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Texts
        public static readonly string Content_NonEmptyField_Tooltip = Resources.Tooltip_Tip_Content_NonEmptyField;
        public static readonly string Content_OnlyPositives_Tooltip = Resources.Tooltip_Tip_Content_OnlyPositives;

        // Events
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies a specific view model whether its property was changed from XAML code.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
