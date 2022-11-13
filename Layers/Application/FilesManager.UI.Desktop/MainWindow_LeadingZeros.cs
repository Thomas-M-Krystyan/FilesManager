using FileManager.Layers.Logic;
using FilesManager.Core.DTOs;
using FilesManager.Core.Helpers;
using FilesManager.Core.Validation;
using FilesManager.UI.Desktop.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
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
            RenamingResultDto result = new(true);

            // Validate input value (cannot be converted to small positive number; it's either too small, equal to "0", or too large)
            if (byte.TryParse(this.LeadingZeros.Text, out byte zerosCount) &&
                zerosCount > 0 && zerosCount <= 7)
            {
                //int currentIndex = 0;

                //(string Path, int Index)[] fileData = this.FilesList.Items.Cast<ListBoxItem>()
                //                                                          .Select(fileItem => ((string)fileItem.ToolTip, currentIndex++))
                //                                                          .ToArray();

                //result = RenamingService.SetLeadingZeros(fileData, zerosCount);

                //// Validate renaming result
                //if (!result.IsSuccess)
                //{
                //    ClearFilesList();
                //}

                //UpdateNameOnList(fileData.FileItem, result.NewFilePath);
                
                //// Reset input field
                //this.LeadingZeros.Text = string.Empty;
            }
            else
            {
                result = new RenamingResultDto(false, $"Invalid value in \"Leading zeros\": " +
                    $"{(string.IsNullOrWhiteSpace(this.LeadingZeros.Text) ? "Empty" : this.LeadingZeros.Text)}.", string.Empty);
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
