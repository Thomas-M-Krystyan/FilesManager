using FileManager.Layers.Logic;
using FilesManager.Core.Converters;
using FilesManager.Core.DTOs;
using FilesManager.Core.Helpers;
using FilesManager.UI.Desktop.ExtensionMethods;
using System.Linq;
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
            RenamingResultDto result = RenamingResultDto.Failure();

            // Validate input value (cannot be converted to small positive number; it's either too small, equal to "0", or too large)
            if (byte.TryParse(this.LeadingZeros.Text, out byte zerosCount) &&
                zerosCount > 0 && zerosCount <= 7)
            {
                // Raw items from the files list
                ListBoxItem[] listBoxItems = this.FilesList.Items.Cast<ListBoxItem>().ToArray();

                // Paths converted into components
                PathZerosDigitsExtensionDto[] filePaths = listBoxItems
                    .Select(fileItem => FilePathConverter.GetPathZerosDigitsExtension((string)fileItem.ToolTip))
                    .ToArray();

                // Only digits components of the names
                string[] filesNamesDigits = filePaths.Select(filePath => filePath.Digits)
                                                     .ToArray();

                int maxDigitLength = Helper.GetMaxLength(filesNamesDigits);

                // Process renaming of the file
                for (int index = 0; index < this.FilesList.Items.Count; index++)
                {
                    result = RenamingService.SetLeadingZeros((string)listBoxItems[index].ToolTip, filePaths[index],
                                                             zerosCount, maxDigitLength);
                    // Validate renaming result
                    if (!result.IsSuccess)
                    {
                        break;
                    }

                    UpdateNameOnList(listBoxItems[index], result.NewFilePath);
                }

                // Reset input field
                this.LeadingZeros.Text = string.Empty;
            }
            else
            {
                result = RenamingResultDto.Failure($"Invalid value in \"Leading zeros\": " +
                    $"{(string.IsNullOrWhiteSpace(this.LeadingZeros.Text) ? "Empty" : this.LeadingZeros.Text)}.");
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
