using FileManager_Logic;
using FileManager_UI.ExtensionMethods;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
{
    public partial class MainWindow : Window
    {
        private bool _isAnyMethodSelected;

        public MainWindow()
        {
            InitializeComponent();
        }

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

                    if (FilesManager.IsValidFileType(filePath))
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
        private void ClearButton_Click(object sender, RoutedEventArgs @event)
        {
            ClearFilesList();

            // Cleanup input sections
            ClearIncrementedNumber();
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs @event)
        {
            // Validate if any radio button of a method is selected
            if (!this._isAnyMethodSelected)
            {
                _ = MessageBox.Show("No renaming method was selected.", "Invalid operation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (this.StartNumberRadioButton.IsChecked ?? false)
                {
                    RenameWithIncrementedNumber();
                }
            }
        }
        #endregion

        #region Rename with incremented number
        /// <summary>
        /// Renames given files using incremented start number.
        /// </summary>
        private void RenameWithIncrementedNumber()
        {
            (bool IsSuccess, string Message, string NewFilePath) result = (false, String.Empty, String.Empty);

            // Validate null or empty input
            if (String.IsNullOrWhiteSpace(this.StartingNumber.Text))
            {
                result = (false, "Provide \"Start number\".", String.Empty);
            }
            else
            {
                // Validate input value (cannot be converted or it's too large)
                if (UInt16.TryParse(this.StartingNumber.Text, out ushort startNumber) &&
                    startNumber - this.FilesList.Items.Count <= UInt16.MaxValue)
                {
                    foreach (ListBoxItem fileItem in this.FilesList.Items)
                    {
                        // Process renaming of the file
                        result = FilesManager.ReplaceFile(fileItem, startNumber++, this.NamePostfix.Text);

                        // Validate renaming result
                        if (!result.IsSuccess)
                        {
                            ClearFilesList();

                            break;
                        }

                        // Update names and paths of the files on the list
                        fileItem.Content = Path.GetFileName(result.NewFilePath);
                        fileItem.ToolTip = result.NewFilePath;
                    }

                    // Set the last number as the new start number
                    this.StartingNumber.Text = startNumber.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    result = (false, $"Invalid \"Start number\": {this.StartingNumber.Text}.", String.Empty);
                }
            }

            // Display popup message
            _ = result.IsSuccess
                ? MessageBox.Show("All files were renamed!", result.Message, MessageBoxButton.OK, MessageBoxImage.Information)
                : MessageBox.Show(result.Message, "Renaming error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Selects the radio button when this input text field is activated (on focus).
        /// </summary>
        private void StartingInputField_Focus(object sender, RoutedEventArgs @event)
        {
            this.StartNumberRadioButton.Activate(ref this._isAnyMethodSelected);
        }

        /// <summary>
        /// Selects the radio button when this input text field is activated (on focus).
        /// </summary>
        private void PostfixInputField_Focus(object sender, RoutedEventArgs @event)
        {
            this.StartNumberRadioButton.Activate(ref this._isAnyMethodSelected);
        }

        /// <summary>
        /// Clears radio button and input text field.
        /// </summary>
        private void ClearIncrementedNumber()
        {
            this.StartNumberRadioButton.Deactivate(ref this._isAnyMethodSelected);
            this.StartingNumber.Text = String.Empty;
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
        #endregion
    }
}
