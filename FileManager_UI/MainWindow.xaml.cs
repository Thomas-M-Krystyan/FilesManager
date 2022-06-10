using FileManager_Logic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
{
    public partial class MainWindow : Window
    {
        private bool _isAnyMethodSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
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
        private void ClearButton_Click(object sender, RoutedEventArgs @event)
        {
            ClearFilesList();

            // Cleanup input sections
            ClearIncrementedNumber();
            ClearPrependAppend();

            // Clear flags
            this._isAnyMethodSelected = false;
        }

        /// <summary>
        /// Processes the selected renaming method.
        /// </summary>
        private void ProcessButton_Click(object sender, RoutedEventArgs @event)
        {
            // Validate if there are any files on the list
            if (this.FilesList.Items.Count == 0)
            {
                _ = MessageBox.Show("The list of files is empty.", "Missing files", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            // Validate if any radio button of a method is selected
            else if (!this._isAnyMethodSelected)
            {
                _ = MessageBox.Show("No renaming method was selected.", "Nothing selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Methods control
                if (this.StartNumberRadioButton.IsChecked ?? false)
                {
                    RenameWithIncrementedNumber();
                }
                else if (this.PrependAppendRadioButton.IsChecked ?? false)
                {
                    RenameWithPrependAndAppendedText();
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
                ? MessageBox.Show("All files were renamed!", result.Message, MessageBoxButton.OK, MessageBoxImage.Information)
                : MessageBox.Show(result.Message, "Renaming error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
    }
}
