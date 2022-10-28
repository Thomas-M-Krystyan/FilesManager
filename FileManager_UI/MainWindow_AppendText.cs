using FileManager_Logic;
using FileManager_UI.ExtensionMethods;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
{
    public partial class MainWindow
    {
        private void RenameWithPrependAndAppendedText()
        {
            // Validate strings which are going to be used in file name
            (bool IsSuccess, string Message, string NewFilePath) result =
                ValidateIllegalCharacters(this.PrependName.Text, this.AppendName.Text);

            if (result.IsSuccess)
            {
                // Process renaming of the file
                foreach (ListBoxItem fileItem in this.FilesList.Items)
                {
                    result = FilesManager.EnrichWithPrependAndAppend(fileItem.ToolTip as string, this.PrependName.Text, this.AppendName.Text);

                    // Validate renaming result
                    if (!result.IsSuccess)
                    {
                        ClearFilesList();

                        break;
                    }

                    UpdateNameOnList(fileItem, result.NewFilePath);
                }

                // Reset input fields
                this.PrependName.Text = String.Empty;
                this.AppendName.Text = String.Empty;
            }

            DisplayPopup(result);
        }

        #region Button handlers
        /// <summary>
        /// Activates <see cref="RadioButton"/> which belongs to this renaming strategy.
        /// </summary>
        private void PrependAppendRadioButtonRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.PrependAppendRadioButton.Activate(this);

            ResetAllRadioButtonsExcept(this.PrependAppendRadioButton);
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> when this label was clicked.
        /// </summary>
        private void PrependAppendLabel_Clicked(object sender, RoutedEventArgs @event)
        {
            PrependAppendRadioButtonRadioButton_Checked(sender, @event);
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> when this input text field was activated (on focus).
        /// </summary>
        private void PrependAppendTextBox_Focus(object sender, RoutedEventArgs @event)
        {
            PrependAppendRadioButtonRadioButton_Checked(sender, @event);
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Clears this <see cref="RadioButton"/> and input text field.
        /// </summary>
        private void ClearPrependAppend()
        {
            this.PrependAppendRadioButton.Deactivate();

            this.PrependName.Text = String.Empty;
            this.AppendName.Text = String.Empty;
        }
        #endregion
    }
}
