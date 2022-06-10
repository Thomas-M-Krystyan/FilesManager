using FileManager_Logic;
using FileManager_UI.ExtensionMethods;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
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
                ValidateIllegalCharacters(this.NamePostfix.Text);

            if (result.IsSuccess)
            {
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
                            result = FilesManager.ReplaceWithNumber(fileItem.ToolTip as string, startNumber++, this.NamePostfix.Text);

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
                        result = (false, $"Invalid \"Start number\": {this.StartingNumber.Text}.", String.Empty);
                    }
                }
            }

            DisplayPopup(result);
        }

        #region Events
        /// <summary>
        /// Activates this method <see cref="RadioButton"/>.
        /// </summary>
        private void StartNumberRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.StartNumberRadioButton.Activate();
            this._isAnyMethodSelected = true;

            ResetAllRadioButtonsExcept(this.StartNumberRadioButton);
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
        #endregion

        #region Cleanup
        /// <summary>
        /// Clears radio button and input text field.
        /// </summary>
        private void ClearIncrementedNumber()
        {
            this.StartNumberRadioButton.Deactivate();

            this.StartingNumber.Text = String.Empty;
            this.NamePostfix.Text = String.Empty;
        }
        #endregion
    }
}
