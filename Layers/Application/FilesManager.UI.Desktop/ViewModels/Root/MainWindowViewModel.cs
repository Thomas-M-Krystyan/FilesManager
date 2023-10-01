using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Desktop.Properties;
using FilesManager.UI.Desktop.Utilities;
using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using Microsoft.Xaml.Behaviors.Core;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Root
{
    /// <summary>
    /// View model for the main window of the application.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        #region Texts
        public static string FilesList_Tooltip = Resources.Tooltip_FilesList;
        #endregion

        #region Properties
        /// <summary>
        /// A collection of files that were dragged and dropped on the specific UI section in the <see cref="MainWindow"/>.
        /// </summary>
        public ObservableCollection<FileData> Files { get; } = new ObservableCollection<FileData>();
        #endregion

        #region View Models
        /// <inheritdoc cref="IncrementNumberViewModel"/>
        public IncrementNumberViewModel IncrementNumberStrategy { get; }

        /// <inheritdoc cref="PrependAppendViewModel"/>
        public PrependAppendViewModel PrependAppendStrategy { get; }

        /// <inheritdoc cref="LeadingZerosViewModel"/>
        public LeadingZerosViewModel LeadingZerosStrategy { get; }
        #endregion

        #region Commands
        /// <summary>
        /// Handles subscribed <see cref="LoadFiles(object)"/> action.
        /// </summary>
        public ICommand LoadFilesCommand => new ActionCommand(LoadFiles);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        internal MainWindowViewModel() : base()
        {
            this.IncrementNumberStrategy = new();
            this.PrependAppendStrategy = new();
            this.LeadingZerosStrategy = new();

            SubscribeEvents();
        }

        /// <summary>
        /// Destructs this instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        ~MainWindowViewModel()
        {
            UnsubscribeEvents();
        }

        #region Polymorphism
        /// <summary>
        /// Deselects all strategies.
        /// </summary>
        protected override sealed void Deselect()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            this.IncrementNumberStrategy.DeselectCommand.Execute(null);
            this.PrependAppendStrategy.DeselectCommand.Execute(null);
            this.LeadingZerosStrategy.DeselectCommand.Execute(null);
        }

        /// <summary>
        /// Resets all strategies.
        /// </summary>
        protected override sealed void Reset()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            this.Files.Clear();
            this.IncrementNumberStrategy.ResetCommand.Execute(null);
            this.PrependAppendStrategy.ResetCommand.Execute(null);
            this.LeadingZerosStrategy.ResetCommand.Execute(null);
        }
        #endregion

        #region Subscriptions
        private void SubscribeEvents()
        {
            this.IncrementNumberStrategy.OnSelected += Deselect;
            this.PrependAppendStrategy.OnSelected += Deselect;
            this.LeadingZerosStrategy.OnSelected += Deselect;
        }

        private void UnsubscribeEvents()
        {
            this.IncrementNumberStrategy.OnSelected -= Deselect;
            this.PrependAppendStrategy.OnSelected -= Deselect;
            this.LeadingZerosStrategy.OnSelected -= Deselect;
        }
        #endregion

        #region Private
        private void LoadFiles(object parameter)
        {
            if (parameter is DragEventArgs dragEvent &&
                dragEvent.Data.GetDataPresent(format: DataFormats.FileDrop))
            {
                // Clear the previous state of the files list
                this.Files.Clear();

                // Get paths from dragged files
                string[] droppedFilesPaths = (string[])dragEvent.Data.GetData(format: DataFormats.FileDrop, autoConvert: true);

                // Populate the list
                foreach (string filePath in droppedFilesPaths)
                {
                    if (Validate.HasValidExtension(filePath))  // Ignore files that doesn't match the pattern "[name].[extension]"
                    {
                        this.Files.Add(
                            new FileData
                            {
                                Path = filePath,
                                Name = Path.GetFileNameWithoutExtension(filePath),
                                Extension = Path.GetExtension(filePath)
                            });
                    }
                    else
                    {
                        _ = Message.ErrorOk(Resources.ERROR_Operation_FileNotRecognized_Header,
                                            Resources.ERROR_Operation_FileNotRecognized_Text);
                    }
                }
            }
        }
        #endregion
    }
}
