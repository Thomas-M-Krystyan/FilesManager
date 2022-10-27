using System.Windows.Controls;

namespace FileManager_UI.ExtensionMethods
{
    public static class RadioButtonExtensions
    {
        /// <summary>
        /// Checks given <see cref="RadioButton"/>.
        /// </summary>
        public static void Activate(this RadioButton radioButton, MainWindow mainWindow)
        {
            radioButton.IsChecked = true;

            mainWindow.IsAnyMethodSelected = true;
        }

        /// <summary>
        /// Unchecks given <see cref="RadioButton"/>.
        /// </summary>
        public static void Deactivate(this RadioButton radioButton)
        {
            radioButton.IsChecked = false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="RadioButton"/> is checked.
        /// </summary>
        public static bool IsChecked(this RadioButton radioButton)
        {
            return radioButton.IsChecked ?? false;
        }
    }
}
