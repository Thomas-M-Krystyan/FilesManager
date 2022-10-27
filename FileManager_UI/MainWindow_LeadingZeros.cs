using FileManager_Logic;
using FileManager_UI.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
{
    public partial class MainWindow
    {
        /// <summary>
        /// Renames given files adding a specific amount of leading zeros before the name.
        /// </summary>
        private void RenameWithLeadingZeros()
        {
            (bool IsSuccess, string Message, string NewFilePath) result = (false, String.Empty, String.Empty);

            // Validate input value (cannot be converted or it's too large)
            if (SByte.TryParse(this.LeadingZeros.Text, out sbyte zerosCount) &&
                zerosCount <= 5)
            {
                // Determine max length of leading number
                int maxNumberLength = 0;

                List<(ListBoxItem FileItem, GroupCollection FileGroups, GroupCollection NumberGroups)> filesData = new();

                foreach (ListBoxItem fileItem in this.FilesList.Items)
                {
                    (int NumberLength, GroupCollection FileGroups, GroupCollection NumberGroups) calculationResult =
                        FilesManager.GetNumberLength(fileItem.ToolTip as string);

                    maxNumberLength = Math.Max(maxNumberLength, calculationResult.NumberLength);

                    filesData.Add((fileItem, calculationResult.FileGroups, calculationResult.NumberGroups));
                }

                // Process renaming of the file
                foreach ((ListBoxItem FileItem, GroupCollection FileGroups, GroupCollection NumberGroups) fileData in filesData)
                {
                    result = FilesManager.SetLeadingZeros(
                        fileData.FileItem.ToolTip as string, fileData.FileGroups, fileData.NumberGroups, zerosCount, maxNumberLength);

                    // Validate renaming result
                    if (!result.IsSuccess)
                    {
                        ClearFilesList();

                        break;
                    }

                    UpdateNameOnList(fileData.FileItem, result.NewFilePath);
                }

                // Reset input field
                this.LeadingZeros.Text = String.Empty;
            }
            else
            {
                result = (false, $"Invalid \"Leading zeros\" value: {this.LeadingZeros.Text}.", String.Empty);
            }

            DisplayPopup(result);
        }

        #region Events
        /// <summary>
        /// Activates this method <see cref="RadioButton"/>.
        /// </summary>
        private void SetLeadingZerosRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.SetLeadingZerosRadioButton.Activate(this);

            ResetAllRadioButtonsExcept(this.SetLeadingZerosRadioButton);
        }

        /// <summary>
        /// Selects the radio button when this label was clicked.
        /// </summary>
        private void LeadingZerosLabel_Clicked(object sender, RoutedEventArgs @event)
        {
            SetLeadingZerosRadioButton_Checked(sender, @event);
        }

        /// <summary>
        /// Selects the radio button when this input text field was activated (on focus).
        /// </summary>
        private void LeadingZerosTextBox_Focus(object sender, RoutedEventArgs @event)
        {
            SetLeadingZerosRadioButton_Checked(sender, @event);
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Clears radio button and input text field.
        /// </summary>
        private void ClearLeadingZeros()
        {
            this.SetLeadingZerosRadioButton.Deactivate();

            this.LeadingZeros.Text = String.Empty;
        }
        #endregion
    }
}
