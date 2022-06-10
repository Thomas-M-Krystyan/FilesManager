using FileManager_Logic;
using FileManager_UI.ExtensionMethods;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
{
    public partial class MainWindow
    {
        /// <summary>
        /// Renames given files using incremented start number.
        /// </summary>
        private void RenameWithIncrementedNumber()
        {
            (bool IsSuccess, string Message, string NewFilePath) result = (false, String.Empty, String.Empty);

            // Validate null or empty input
            if (String.IsNullOrWhiteSpace(this.StartingNumber.Text))
            {
                result = (false, "Provide \"Start number\".", String.Empty);
            }
            else
            {
                // Validate input value (cannot be converted or it's too large)
                if (UInt16.TryParse(this.StartingNumber.Text, out ushort startNumber) &&
                    startNumber - this.FilesList.Items.Count <= UInt16.MaxValue)
                {
                    foreach (ListBoxItem fileItem in this.FilesList.Items)
                    {
                        // Process renaming of the file
                        result = FilesManager.ReplaceFile(fileItem, startNumber++, this.NamePostfix.Text);

                        // Validate renaming result
                        if (!result.IsSuccess)
                        {
                            ClearFilesList();

                            break;
                        }

                        // Update names and paths of the files on the list
                        fileItem.Content = Path.GetFileName(result.NewFilePath);
                        fileItem.ToolTip = result.NewFilePath;
                    }

                    if (result.IsSuccess)
                    {
                        // Set the last number as the new start number
                        this.StartingNumber.Text = startNumber.ToString(CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    result = (false, $"Invalid \"Start number\": {this.StartingNumber.Text}.", String.Empty);
                }
            }

            // Display popup message
            _ = result.IsSuccess
                ? MessageBox.Show("All files were renamed!", result.Message, MessageBoxButton.OK, MessageBoxImage.Information)
                : MessageBox.Show(result.Message, "Renaming error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Activates this method <see cref="RadioButton"/>.
        /// </summary>
        private void StartNumberRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.StartNumberRadioButton.Activate();
            this._isAnyMethodSelected = true;
        }

        /// <summary>
        /// Selects the radio button when this label was clicked.
        /// </summary>
        private void StartNumberLabel_Clicked(object sender, RoutedEventArgs @event)
        {
            StartNumberRadioButton_Checked(sender, @event);
        }

        /// <summary>
        /// Selects the radio button when this input text field was activated (on focus).
        /// </summary>
        private void StartNumberTextBox_Focus(object sender, RoutedEventArgs @event)
        {
            StartNumberRadioButton_Checked(sender, @event);
        }

        /// <summary>
        /// Clears radio button and input text field.
        /// </summary>
        private void ClearIncrementedNumber()
        {
            this.StartNumberRadioButton.Deactivate();
            this.StartingNumber.Text = String.Empty;
        }
    }
}
