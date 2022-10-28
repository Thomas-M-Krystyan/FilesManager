using FileManager_Logic;
using FileManager_UI.ExtensionMethods;
using FileManager_UI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
{
    public partial class MainWindow : Window
    {
        #region Fields
        protected internal bool IsAnyMethodSelected { get; set; }

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
                string[] droppedFilesPaths = @event.Data.GetData(format: DataFormats.FileDrop, autoConvert: true) as string[];

                // Populate the list
                foreach (string filePath in droppedFilesPaths)
                {
                    ListBoxItem listBoxItem = new();

                    if (FilesManager.HasValidExtension(filePath))  // Ignore folders and files with non-standard extensions
                    {
                        listBoxItem.Content = Path.GetFileName(filePath);
                        listBoxItem.ToolTip = filePath;

                        _ = this.FilesList.Items.Add(listBoxItem);
                    }
                    else
                    {
                        _ = MessageBox.Show("Unrecognized type of file.", "File error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        #region Global buttons
        /// <summary>
        /// Clears list of files and any methods values.
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs @event)
        {
            ClearFilesList();

            // Cleanup input sections
            ClearIncrementedNumber();
            ClearPrependAppend();
            ClearLeadingZeros();

            // Clear flags
            this.IsAnyMethodSelected = false;
        }

        /// <summary>
        /// Processes the selected renaming method.
        /// </summary>
        private void ProcessButton_Click(object sender, RoutedEventArgs @event)
        {
            // Validate if there are any files on the list
            if (this.FilesList.Items.Count == 0)
            {
                _ = Message.WarningOk("The list of files is empty.", "Missing files");
            }
            // Validate if any radio button of a method is selected
            else if (!this.IsAnyMethodSelected)
            {
                _ = Message.WarningOk("No renaming method was selected.", "Nothing selected");
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
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Clears the list of dropped files.
        /// </summary>
        private void ClearFilesList()
        {
            this.FilesList.Items.Clear();
        }

        /// <summary>
        /// Validates if the provided text values contains illegal characters.
        /// </summary>
        private static (bool IsSuccess, string Message, string NewFilePath) ValidateIllegalCharacters(params string[] textInputs)
        {
            return FilesManager.ContainsIllegalCharacters(out string invalidValue, textInputs)
                ? (false, $"The given value contains illegal characters \"{invalidValue}\"", String.Empty)
                : (true, String.Empty, String.Empty);  // Only the first value is important
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
        /// Displays a proper <see cref="MessageBox"/> popup with feedback information.
        /// </summary>
        private static void DisplayPopup((bool IsSuccess, string Message, string NewFilePath) result)
        {
            _ = result.IsSuccess
                ? Message.InfoOk("Operation successful", "All files were renamed!")
                : Message.ErrorOk("Operation failed", result.Message);
        }

        /// <summary>
        /// Populates collection of all available <see cref="RadioButton"/>s.
        /// </summary>
        private void RegisterRadioButtons()
        {
            this._radioButtons.Add(this.StartNumberRadioButton);
            this._radioButtons.Add(this.PrependAppendRadioButton);
            this._radioButtons.Add(this.SetLeadingZerosRadioButton);
        }

        /// <summary>
        /// Deactivates all radio buttons except the provided one.
        /// </summary>
        private void ResetAllRadioButtonsExcept(RadioButton excludedRadioButton)
        {
            RadioButton[] radioButtonsToDeactivate = this._radioButtons.Where(button => button != excludedRadioButton)
                                                                       .ToArray();
            foreach (RadioButton button in radioButtonsToDeactivate)
            {
                button.Deactivate();
            }
        }
        #endregion
    }
}
