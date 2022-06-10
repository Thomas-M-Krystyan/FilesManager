using System.Windows.Controls;

namespace FileManager_UI.ExtensionMethods
{
    public static class RadioButtonExtensions
    {
        /// <summary>
        /// Checks given <see cref="RadioButton"/>.
        /// </summary>
        public static void Activate(this RadioButton radioButton)
        {
            radioButton.IsChecked = true;
        }

        /// <summary>
        /// Unchecks given <see cref="RadioButton"/>.
        /// </summary>
        public static void Deactivate(this RadioButton radioButton)
        {
            radioButton.IsChecked = false;
        }
    }
}
