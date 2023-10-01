using System.Windows;

namespace FilesManager.UI.Desktop.Utilities
{
    internal static class Message
    {
        /// <summary>
        /// Displays ERROR (red) popup with a single "OK" acknowledge button.
        /// </summary>
        internal static MessageBoxResult ErrorOk(string messageBoxHeader, string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, messageBoxHeader, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays WARNING (yellow) popup with a single "OK" acknowledge button.
        /// </summary>
        internal static MessageBoxResult WarningOk(string messageBoxHeader, string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, messageBoxHeader, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Displays INFORMATION (blue) popup with a single "OK" acknowledge button.
        /// </summary>
        internal static MessageBoxResult InfoOk(string messageBoxHeader, string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, messageBoxHeader, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
