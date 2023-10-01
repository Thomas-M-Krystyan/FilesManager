using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Desktop.Properties;
using FilesManager.UI.Desktop.Utilities;
using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
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
        public static string GeneralSection_Header = Resources.Header_General;
        public static string GeneralSection_Tooltip = Resources.Tooltip_General;

        public static string FilesList_Tooltip = Resources.Tooltip_FilesList;
        #endregion

        #region Fields
        private bool _isFileListLoaded;
        private bool _isStrategySelected;
        private StrategyBase? _activeStrategy;
        #endregion

        #region Properties (binding)
        /// <summary>
        /// A collection of files that were dragged and dropped on the specific UI section in the <see cref="MainWindow"/>.
        /// </summary>
        public ObservableCollection<FileData> Files { get; } = new ObservableCollection<FileData>();

        private bool _canReset;

        /// <summary>
        /// Determines whether the "Reset" button is enabled on the UI.
        /// </summary>
        public bool CanReset
        {
            get => this._canReset;
            set
            {
                this._canReset = value;
                OnPropertyChanged(nameof(this.CanReset));
            }
        }

        private bool _canProcess;
        
        /// <summary>
        /// Determines whether the "Process" button is enabled on the UI.
        /// </summary>
        public bool CanProcess
        {
            get => this._canProcess;
            set
            {
                this._canProcess = value;
                OnPropertyChanged(nameof(this.CanProcess));
            }
        }
        #endregion

        #region Properties (read-only): View Models
        /// <inheritdoc cref="IncrementNumberViewModel"/>
        public IncrementNumberViewModel IncrementNumberStrategy { get; }

        /// <inheritdoc cref="PrependAppendViewModel"/>
        public PrependAppendViewModel PrependAppendStrategy { get; }

        /// <inheritdoc cref="LeadingZerosViewModel"/>
        public LeadingZerosViewModel LeadingZerosStrategy { get; }
        #endregion

        #region Commands (binding)
        /// <summary>
        /// Handles subscribed <see cref="LoadFiles(object)"/> action.
        /// </summary>
        public ICommand LoadFilesCommand => new ActionCommand(LoadFiles);

        /// <summary>
        /// Handles subscribed <see cref="Process"/> action.
        /// </summary>
        public ICommand ProcessCommand => new ActionCommand(Process);
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
            this._isFileListLoaded = false;

            this.IncrementNumberStrategy.ResetCommand.Execute(null);
            this.PrependAppendStrategy.ResetCommand.Execute(null);
            this.LeadingZerosStrategy.ResetCommand.Execute(null);
            this._isStrategySelected = false;

            UpdateMainButtons();
        }
        #endregion

        #region Private
        /// <summary>
        /// Loads the files details from those dragged and dropped into a dedicated UI section of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="parameter">
        ///   The event's argument (<see cref="DragEventArgs"/>) passed
        ///   when the specific trigger was activated on the XAML side.
        /// </param>
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
                    if (Validate.IsFilePathValid(filePath).Success)  // Ignore files that doesn't match the pattern "[name].[extension]"
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
                                            Resources.ERROR_Operation_FileNotRecognized_Text + $" \"{Path.GetFileName(filePath)}\"");
                        this.Files.Clear();

                        return;
                    }
                }

                FileListIsLoaded();
            }
        }

        /// <summary>
        /// Processes the currently active strategy (a view model extending <see cref="StrategyBase"/>).
        /// </summary>
        private void Process()
        {
            if (this.CanProcess)
            {
                this._activeStrategy!.Process();
            }
        }

        private void FileListIsLoaded()
        {
            // Set 1st condition
            this._isFileListLoaded = true;

            // Inform binding properties whether their conditions are met
            UpdateMainButtons();
        }

        private void StrategyIsSelectd()
        {
            // Determine which specific strategy was activated
            this._activeStrategy = this.IncrementNumberStrategy.IsEnabled ? this.IncrementNumberStrategy :
                                   this.PrependAppendStrategy.IsEnabled   ? this.PrependAppendStrategy   :
                                   this.LeadingZerosStrategy.IsEnabled    ? this.LeadingZerosStrategy    :
                                   null;
            // Set 2nd condition
            this._isStrategySelected = this._activeStrategy != null;

            // Inform binding properties whether their conditions are met
            UpdateMainButtons();
        }

        private void UpdateMainButtons()
        {
            this.CanReset = this._isFileListLoaded || this._isStrategySelected;
            this.CanProcess = this._isFileListLoaded && this._isStrategySelected;
        }
        #endregion

        #region Subscriptions
        private void SubscribeEvents()
        {
            this.IncrementNumberStrategy.BeforeOnSelected += Deselect;
            this.IncrementNumberStrategy.AfterOnSelected += StrategyIsSelectd;

            this.PrependAppendStrategy.BeforeOnSelected += Deselect;
            this.PrependAppendStrategy.AfterOnSelected += StrategyIsSelectd;

            this.LeadingZerosStrategy.BeforeOnSelected += Deselect;
            this.LeadingZerosStrategy.AfterOnSelected += StrategyIsSelectd;
        }

        private void UnsubscribeEvents()
        {
            this.IncrementNumberStrategy.BeforeOnSelected -= Deselect;
            this.IncrementNumberStrategy.AfterOnSelected -= StrategyIsSelectd;

            this.PrependAppendStrategy.BeforeOnSelected -= Deselect;
            this.PrependAppendStrategy.AfterOnSelected -= StrategyIsSelectd;

            this.LeadingZerosStrategy.BeforeOnSelected -= Deselect;
            this.LeadingZerosStrategy.AfterOnSelected -= StrategyIsSelectd;
        }
        #endregion
    }
}
