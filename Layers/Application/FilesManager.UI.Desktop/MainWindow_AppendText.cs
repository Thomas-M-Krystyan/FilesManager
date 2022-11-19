using FileManager.Layers.Logic;
using FilesManager.Core.DTOs;
using FilesManager.UI.Desktop.ExtensionMethods;
using System.Windows;
using System.Windows.Controls;

namespace FilesManager.UI.Desktop
{
    public partial class MainWindow
    {
        private void RenameWithPrependAndAppendedText()
        {
            // Validate strings which are going to be used in file name
            RenamingResultDto result = ValidateIllegalCharacters(this.PrependName.Text, this.AppendName.Text);

            if (result.IsSuccess)
            {
                // Process renaming of the file
                foreach (ListBoxItem fileItem in this.FilesList.Items)
                {
                    result = RenamingService.EnrichWithPrependAndAppend((string)fileItem.ToolTip, this.PrependName.Text, this.AppendName.Text);

                    // Validate renaming result
                    if (!result.IsSuccess)
                    {
                        break;
                    }

                    UpdateNameOnList(fileItem, result.NewFilePath);
                }

                // Reset input fields
                this.PrependName.Text = string.Empty;
                this.AppendName.Text = string.Empty;
            }

            DisplayPopup(result);
        }

        #region Button handlers
        /// <summary>
        /// Activates <see cref="RadioButton"/> which belongs to this renaming strategy.
        /// </summary>
        private void PrependAppendRadioButtonRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.PrependAppendRadioButton.Activate();

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

            this.PrependName.Text = string.Empty;
            this.AppendName.Text = string.Empty;
        }
        #endregion
    }
}
