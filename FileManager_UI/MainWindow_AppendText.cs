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
            (bool IsSuccess, string Message, string NewFilePath) result = (false, String.Empty, String.Empty);

            foreach (ListBoxItem fileItem in this.FilesList.Items)
            {
                // Process renaming of the file
                result = FilesManager.EnrichWithPrependAndAppend(fileItem.ToolTip as string, this.PrependName.Text, this.AppendName.Text);

                // Validate renaming result
                if (!result.IsSuccess)
                {
                    ClearFilesList();

                    break;
                }

                UpdateNameOnList(fileItem, result.NewFilePath);
            }

            DisplayPopup(result);
        }

        #region Events
        /// <summary>
        /// Activates this method <see cref="RadioButton"/>.
        /// </summary>
        private void PrependAppendRadioButtonRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.PrependAppendRadioButton.Activate();
            this._isAnyMethodSelected = true;
        }

        /// <summary>
        /// Selects the radio button when this label was clicked.
        /// </summary>
        private void PrependAppendLabel_Clicked(object sender, RoutedEventArgs @event)
        {
            PrependAppendRadioButtonRadioButton_Checked(sender, @event);
        }

        /// <summary>
        /// Selects the radio button when this input text field was activated (on focus).
        /// </summary>
        private void PrependAppendTextBox_Focus(object sender, RoutedEventArgs @event)
        {
            PrependAppendRadioButtonRadioButton_Checked(sender, @event);
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Clears radio button and input text field.
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
