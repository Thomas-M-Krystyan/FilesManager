﻿using FilesManager.Core.DTOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Desktop.ExtensionMethods;
using FilesManager.UI.Desktop.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FilesManager.UI.Desktop
{
    public partial class MainWindow : Window
    {
        #region Fields
        private readonly IList<RadioButton> _radioButtons = new List<RadioButton>();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            RegisterRadioButtons();
        }

        /// <summary>
        /// Loads files drag-and-dropped (using mouse) into the list of files.
        /// </summary>
        private void Drop_Files(object sender, DragEventArgs @event)
        {
            if (@event.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ClearFilesList();

                // Load dropped files
                string[] droppedFilesPaths = (string[])@event.Data.GetData(format: DataFormats.FileDrop, autoConvert: true);

                // Populate the list
                foreach (string filePath in droppedFilesPaths)
                {
                    ListBoxItem listBoxItem = new();

                    if (Validate.HasValidExtension(filePath))  // Ignore folders and files with non-standard extensions
                    {
                        listBoxItem.Content = Path.GetFileName(filePath);
                        listBoxItem.ToolTip = filePath;

                        _ = this.FilesList.Items.Add(listBoxItem);
                    }
                    else
                    {
                        _ = Message.ErrorOk("File error", "Unrecognized type of file.");
                    }
                }
            }
        }

        #region Button handlers (common)
        /// <summary>
        /// Clears list of files and any methods values.
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs @event)
        {
            ClearFilesList();

            // Cleanup input sections and Radio Buttons
            ClearIncrementedNumber();
            ClearPrependAppend();
            ClearLeadingZeros();
        }

        /// <summary>
        /// Processes the selected renaming method.
        /// </summary>
        private void ProcessButton_Click(object sender, RoutedEventArgs @event)
        {
            // Validate if there are any files on the list
            if (this.FilesList.Items.Count == 0)
            {
                _ = Message.WarningOk("Missing files", "The list of files is empty.");
            }
            else
            {
                // Methods control
                if (this.StartNumberRadioButton.IsChecked())
                {
                    RenameWithIncrementedNumber();
                }
                else if (this.PrependAppendRadioButton.IsChecked())
                {
                    RenameWithPrependAndAppendedText();
                }
                else if (this.SetLeadingZerosRadioButton.IsChecked())
                {
                    RenameWithLeadingZeros();
                }
                else
                {
                    _ = Message.WarningOk("Nothing selected", "No renaming method was selected.");
                }
            }
        }
        #endregion

        #region Helper base (protected) methods
        /// <summary>
        /// Clears the list of dropped files.
        /// </summary>
        private protected void ClearFilesList()
        {
            this.FilesList.Items.Clear();
        }

        /// <summary>
        /// Validates if the provided text values contains illegal characters.
        /// </summary>
        private protected static RenamingResultDto ValidateIllegalCharacters(params string[] textInputs)
        {
            return Validate.ContainsIllegalCharacters(textInputs, out string invalidValue)
                ? RenamingResultDto.Failure($"The given value contains illegal characters \"{invalidValue}\"")
                : RenamingResultDto.Success();
        }

        /// <summary>
        /// Updates name and path of the given file on the <see cref="ItemCollection"/> list.
        /// </summary>
        private protected static void UpdateNameOnList(ListBoxItem fileItem, string newFilePath)
        {
            fileItem.Content = Path.GetFileName(newFilePath);
            fileItem.ToolTip = newFilePath;
        }

        /// <summary>
        /// Displays a proper <see cref="MessageBoxResult"/> popup with feedback information.
        /// </summary>
        private protected static void DisplayPopup(RenamingResultDto result)
        {
            _ = result.IsSuccess
                ? Message.InfoOk("Operation successful", "All files were renamed!")
                : Message.ErrorOk("Operation failed", result.Message);
        }

        /// <summary>
        /// Deactivates all <see cref="RadioButton"/>s except the provided one.
        /// </summary>
        private protected void ResetAllRadioButtonsExcept(RadioButton excludedRadioButton)
        {
            RadioButton[] radioButtonsToDeactivate = this._radioButtons.Where(button => button != excludedRadioButton)
                                                                       .ToArray();
            foreach (RadioButton button in radioButtonsToDeactivate)
            {
                button.Deactivate();
            }
        }
        #endregion

        #region Helper (private) methods
        /// <summary>
        /// Populates collection of all available <see cref="RadioButton"/>s.
        /// </summary>
        private void RegisterRadioButtons()
        {
            this._radioButtons.Add(this.StartNumberRadioButton);
            this._radioButtons.Add(this.PrependAppendRadioButton);
            this._radioButtons.Add(this.SetLeadingZerosRadioButton);
        }
        #endregion
    }
}
