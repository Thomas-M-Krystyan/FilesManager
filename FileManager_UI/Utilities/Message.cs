using System.Windows;

namespace FileManager_UI.Utilities
{
    public static class Message
    {
        /// <summary>
        /// Displays ERROR (red) popup with a single "OK" acknowledge button.
        /// </summary>
        public static MessageBoxResult ErrorOk(string messageBoxText, string messageBoxHeader)
        {
            return MessageBox.Show(messageBoxText, messageBoxHeader, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays WARNING (yellow) popup with a single "OK" acknowledge button.
        /// </summary>
        public static MessageBoxResult WarningOk(string messageBoxText, string messageBoxHeader)
        {
            return MessageBox.Show(messageBoxText, messageBoxHeader, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Displays INFORMATION (blue) popup with a single "OK" acknowledge button.
        /// </summary>
        public static MessageBoxResult InfoOk(string messageBoxText, string messageBoxHeader)
        {
            return MessageBox.Show(messageBoxText, messageBoxHeader, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
