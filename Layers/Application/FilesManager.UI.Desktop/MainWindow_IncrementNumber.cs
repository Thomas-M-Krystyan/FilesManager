using FileManager.Layers.Logic;
using FilesManager.UI.Desktop.ExtensionMethods;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace FilesManager.UI.Desktop
{
    public partial class MainWindow
    {
        /// <summary>
        /// Renames given files using incremented start number and optional postfix.
        /// </summary>
        private void RenameWithIncrementedNumber()
        {
            // Validate strings which are going to be used in file name
            (bool IsSuccess, string Message, string NewFilePath) result =
                ValidateIllegalCharacters(this.NamePrefix.Text, this.NamePostfix.Text);

            if (result.IsSuccess)
            {
                // Validate null or empty input
                if (String.IsNullOrWhiteSpace(this.StartingNumber.Text))
                {
                    result = (false, "Provide \"Start number\".", string.Empty);
                }
                else
                {
                    // Validate input value (cannot be converted or it's too large)
                    if (UInt16.TryParse(this.StartingNumber.Text, out ushort startNumber) &&
                        startNumber - this.FilesList.Items.Count <= UInt16.MaxValue)
                    {
                        // Process renaming of the file
                        foreach (ListBoxItem fileItem in this.FilesList.Items)
                        {
                            result = RenamingService.ReplaceWithNumber((string)fileItem.ToolTip, this.NamePrefix.Text, startNumber++, this.NamePostfix.Text);

                            // Validate renaming result
                            if (!result.IsSuccess)
                            {
                                ClearFilesList();

                                break;
                            }

                            UpdateNameOnList(fileItem, result.NewFilePath);
                        }

                        if (result.IsSuccess)
                        {
                            // Set the last number as the new start number
                            this.StartingNumber.Text = startNumber.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        result = (false, $"Invalid \"Start number\" value: {this.StartingNumber.Text}.", string.Empty);
                    }
                }
            }

            DisplayPopup(result);
        }

        #region Button handlers
        /// <summary>
        /// Activates <see cref="RadioButton"/> which belongs to this renaming strategy.
        /// </summary>
        private void StartNumberRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.StartNumberRadioButton.Activate();

            ResetAllRadioButtonsExcept(this.StartNumberRadioButton);
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> when this label was clicked.
        /// </summary>
        private void StartNumberLabel_Clicked(object sender, RoutedEventArgs @event)
        {
            StartNumberRadioButton_Checked(sender, @event);
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> when this input text field was activated (on focus).
        /// </summary>
        private void StartNumberTextBox_Focus(object sender, RoutedEventArgs @event)
        {
            StartNumberRadioButton_Checked(sender, @event);
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Clears this <see cref="RadioButton"/> and input text field.
        /// </summary>
        private void ClearIncrementedNumber()
        {
            this.StartNumberRadioButton.Deactivate();

            this.NamePrefix.Text = string.Empty;
            this.StartingNumber.Text = string.Empty;
            this.NamePostfix.Text = string.Empty;
        }
        #endregion
    }
}
