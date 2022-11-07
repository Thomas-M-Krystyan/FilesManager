using FileManager.Layers.Logic;
using FilesManager.Core.DTOs;
using FilesManager.Core.Helpers;
using FilesManager.UI.Desktop.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FilesManager.UI.Desktop
{
    public partial class MainWindow
    {
        /// <summary>
        /// Renames given files adding a specific amount of leading zeros before the name.
        /// </summary>
        private void RenameWithLeadingZeros()
        {
            (bool IsSuccess, string Message, string NewFilePath) result = (false, string.Empty, string.Empty);

            // Validate input value (cannot be converted or it's too large)
            if (sbyte.TryParse(this.LeadingZeros.Text, out sbyte zerosCount) &&
                zerosCount <= 5)
            {
                // Determine max length of leading number
                int maxNumberLength = 0;

                List<(ListBoxItem FileItem, GroupCollection? FileGroups, GroupCollection? NumberGroups)> filesData = new();

                foreach (ListBoxItem fileItem in this.FilesList.Items)
                {
                    NumberLengthDto resultDto = Helper.GetNumberLength((string)fileItem.ToolTip);

                    maxNumberLength = Math.Max(maxNumberLength, resultDto.NumberLength);

                    filesData.Add((fileItem, resultDto.FileGroups, resultDto.NumberGroups));
                }

                // Process renaming of the file
                foreach ((ListBoxItem FileItem, GroupCollection? FileGroups, GroupCollection? NumberGroups) fileData in filesData)
                {
                    result = RenamingService.SetLeadingZeros(
                        (string)fileData.FileItem.ToolTip, fileData.FileGroups, fileData.NumberGroups, zerosCount, maxNumberLength);

                    // Validate renaming result
                    if (!result.IsSuccess)
                    {
                        ClearFilesList();

                        break;
                    }

                    UpdateNameOnList(fileData.FileItem, result.NewFilePath);
                }

                // Reset input field
                this.LeadingZeros.Text = string.Empty;
            }
            else
            {
                result = (false, $"Invalid \"Leading zeros\" value: {this.LeadingZeros.Text}.", string.Empty);
            }

            DisplayPopup(result);
        }

        #region Button handlers
        /// <summary>
        /// Activates <see cref="RadioButton"/> which belongs to this renaming strategy.
        /// </summary>
        private void SetLeadingZerosRadioButton_Checked(object sender, RoutedEventArgs @event)
        {
            this.SetLeadingZerosRadioButton.Activate();

            ResetAllRadioButtonsExcept(this.SetLeadingZerosRadioButton);
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> when this label was clicked.
        /// </summary>
        private void LeadingZerosLabel_Clicked(object sender, RoutedEventArgs @event)
        {
            SetLeadingZerosRadioButton_Checked(sender, @event);
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> when this input text field was activated (on focus).
        /// </summary>
        private void LeadingZerosTextBox_Focus(object sender, RoutedEventArgs @event)
        {
            SetLeadingZerosRadioButton_Checked(sender, @event);
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Clears this <see cref="RadioButton"/> and input text field.
        /// </summary>
        private void ClearLeadingZeros()
        {
            this.SetLeadingZerosRadioButton.Deactivate();

            this.LeadingZeros.Text = string.Empty;
        }
        #endregion
    }
}
