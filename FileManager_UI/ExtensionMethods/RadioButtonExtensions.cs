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
        /// Checks given <see cref="RadioButton"/>.
        /// </summary>
        public static void Activate(this RadioButton radioButton, ref bool isSelected)
        {
            radioButton.Activate();
            isSelected = true;
        }

        /// <summary>
        /// Unchecks given <see cref="RadioButton"/>.
        /// </summary>
        public static void Deactivate(this RadioButton radioButton)
        {
            radioButton.IsChecked = false;
        }

        /// <summary>
        /// Unchecks given <see cref="RadioButton"/>.
        /// </summary>
        public static void Deactivate(this RadioButton radioButton, ref bool isSelected)
        {
            radioButton.Deactivate();
            isSelected = false;
        }
    }
}
