﻿using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Validation;
using FilesManager.UI.Desktop.Utilities;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FilesManager.UI.Desktop
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.SizeToContent = SizeToContent.WidthAndHeight;  // NOTE: Adjust window to its content in real-time
        }

        #region Button handlers (common)
        /// <summary>
        /// Processes the selected renaming method.
        /// </summary>
        private void ProcessButton_Click(object sender, RoutedEventArgs @event)
        {
            // Validate if there are any files on the list
            //if (this.FilesList.Items.Count == 0)
            {
                _ = Message.WarningOk("Missing files", "The list of files is empty.");
            }
            //else
            {
                // Methods control
                //if (this.StartNumberRadioButton.IsChecked())
                {
                    //RenameWithIncrementedNumber();  // TODO: Uncomment
                }
                //else if (this.PrependAppendRadioButton.IsChecked())
                {
                    //RenameWithPrependAndAppendedText();  // TODO: Uncomment
                }
                //else if (this.SetLeadingZerosRadioButton.IsChecked())
                {
                    //RenameWithLeadingZeros();  // TODO: Uncomment
                }
                //else
                {
                    _ = Message.WarningOk("Nothing selected", "No renaming method was selected.");
                }
            }
        }
        #endregion

        #region Helper (partial) methods - used by strategies

        /// <summary>
        /// Validates if the provided text values contains illegal characters.
        /// </summary>
        private static RenamingResultDto ValidateIllegalCharacters(params string[] textInputs)
        {
            return Validate.ContainsIllegalCharacters(textInputs, out string invalidValue)
                ? RenamingResultDto.Failure($"The given value contains illegal characters \"{invalidValue}\"")
                : RenamingResultDto.Success();
        }

        /// <summary>
        /// Updates name and path of the given file on the <see cref="ItemCollection"/> list.
        /// </summary>
        private static void UpdateNameOnList(ListBoxItem fileItem, string newFilePath)
        {
            fileItem.Content = Path.GetFileName(newFilePath);
            fileItem.ToolTip = newFilePath;
        }

        /// <summary>
        /// Displays a proper <see cref="MessageBoxResult"/> popup with feedback information.
        /// </summary>
        private static void DisplayPopup(RenamingResultDto result)
        {
            _ = result.IsSuccess
                ? Message.InfoOk("Operation successful", "All files were renamed!")
                : Message.ErrorOk("Operation failed", result.Message);
        }
        #endregion
    }
}
