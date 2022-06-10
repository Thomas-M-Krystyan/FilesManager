using FileManager_Logic;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileManager_UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Drop_Files(object sender, DragEventArgs @event)
        {
            if (@event.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Clear the list
                this.filesList.Items.Clear();
                
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

                        _ = this.filesList.Items.Add(listBoxItem);
                    }
                    else
                    {
                        _ = MessageBox.Show("Unrecognized type of file.", "File error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs @event)
        {
            // Clear the list
            this.filesList.Items.Clear();
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs @event)
        {
            (bool IsSuccess, string Message, string NewFilePath) result = (false, String.Empty, String.Empty);

            // Validate null or empty input
            if (String.IsNullOrWhiteSpace(this.startingNumber.Text))
            {
                result = (false, "Provide \"Start number\".", String.Empty);
            }
            else
            {
                // Validate input value
                if (UInt16.TryParse(this.startingNumber.Text, out ushort startNumber))
                {
                    foreach (ListBoxItem fileItem in this.filesList.Items)
                    {
                        // Process renaming of the file
                        result = FilesManager.ReplaceFile(fileItem, startNumber++);

                        // Validate renaming result
                        if (!result.IsSuccess)
                        {
                            // Clear the list
                            this.filesList.Items.Clear();

                            break;
                        }

                        // Update names and paths of the files on the list
                        fileItem.Content = Path.GetFileName(result.NewFilePath);
                        fileItem.ToolTip = result.NewFilePath;
                    }

                    // Set the last number as the new start number
                    this.startingNumber.Text = startNumber.ToString();
                }
                else
                {
                    result = (false, $"Invalid \"Start number\": {this.startingNumber.Text}.", String.Empty);
                }
            }

            // Display popup message
            _ = result.IsSuccess
                ? MessageBox.Show("All files were renamed!", result.Message, MessageBoxButton.OK, MessageBoxImage.Information)
                : MessageBox.Show(result.Message, "Renaming error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
