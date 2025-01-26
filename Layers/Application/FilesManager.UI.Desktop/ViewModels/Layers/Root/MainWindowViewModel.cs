using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Validation;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.Utilities;
using FilesManager.UI.Desktop.ViewModels.Layers.Common.Facades;
using FilesManager.UI.Desktop.ViewModels.Layers.Specific;
using FilesManager.UI.Desktop.ViewModels.Layers.Specific.Renaming;
using FilesManager.UI.Desktop.ViewModels.Layers.Specific.Renaming.Interfaces;
using Microsoft.Xaml.Behaviors.Core;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Root
{
    /// <summary>
    /// View model for the main window of the application.
    /// </summary>
    /// <seealso cref="BasicViewModelFacade"/>
    internal sealed class MainWindowViewModel : BasicViewModelFacade
    {
        #region Texts
        // Title
        public static string WindowTitle = Resources.WindowTitle;

        // Layers
        public static string Layer_RenamingButton_Header = Resources.Content_Layer_RenamingButton;
        public static string Layer_RenamingButton_Tooltip = Resources.Tooltip_Layer_RenamingButton;

        public static string Layer_ConvertingButton_Header = Resources.Content_Layer_ConvertingButton;
        public static string Layer_ConvertingButton_Tooltip = Resources.Tooltip_Layer_ConvertingButton;

        public static string Layer_ValidatingButton_Header = Resources.Content_Layer_ValidatingButton;
        public static string Layer_ValidatingButton_Tooltip = Resources.Tooltip_Layer_ValidatingButton;

        // Files (list)
        public static string FilesList_Tooltip = Resources.Tooltip_FilesList;

        // Files (counter)
        public static string FilesCounter_Label = Resources.Label_FilesCounter;
        public static string FilesCounter_Tooltip = Resources.Tooltip_FilesCounter;

        // Buttons
        public static string ResetButton_Content = Resources.Content_ResetButton;
        public static string ResetButton_Tooltip = Resources.Tooltip_ResetButton;

        public static string ClearButton_Content = Resources.Content_ClearButton;
        public static string ClearButton_Tooltip = Resources.Tooltip_ClearButton;

        public static string ProcessButton_Content = Resources.Content_ProcessButton;
        public static string ProcessButton_Tooltip = Resources.Tooltip_ProcessButton;
        #endregion

        #region Fields
        private IRenaming? _activeStrategy;
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        /// <inheritdoc cref="ActiveLayerViewModel"/>
        public ActiveLayerViewModel Layer { get; }

        /// <inheritdoc cref="IncrementNumberViewModel"/>
        public IncrementNumberViewModel IncrementNumberStrategy { get; }

        /// <inheritdoc cref="PrependAppendViewModel"/>
        public PrependAppendViewModel PrependAppendStrategy { get; }

        /// <inheritdoc cref="LeadingZerosViewModel"/>
        public LeadingZerosViewModel LeadingZerosStrategy { get; }

        /// <summary>
        /// A collection of files that were dragged and dropped on the specific UI section in the <see cref="MainWindow"/>.
        /// </summary>
        public ObservableCollection<FileData> Files { get; } = [];

        private int _counter;

        /// <summary>
        /// Holds the number of loaded files.
        /// </summary>
        public int Counter
        {
            get => this._counter;
            set
            {
                this._counter = value;
                OnPropertyChanged(nameof(this.Counter));
            }
        }

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

        private bool _canClear;

        /// <summary>
        /// Determines whether the "Clear" button is enabled on the UI.
        /// </summary>
        public bool CanClear
        {
            get => this._canClear;
            set
            {
                this._canClear = value;
                OnPropertyChanged(nameof(this.CanClear));
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

        #region Properties
        /// <summary>
        /// Determines if the list of files is already loaded.
        /// </summary>
        private bool IsFileListLoaded => this.Files.Count > 0;

        /// <summary>
        /// Determines if the current strategy is already set.
        /// </summary>
        private bool IsStrategySelected => this._activeStrategy != null;
        #endregion

        // NOTE: All binding elements should be public
        #region Commands (binding)
        /// <summary>
        /// Handles subscribed <see cref="LoadFiles(object)"/> action.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public ICommand LoadFilesCommand => new ActionCommand(LoadFiles);

        /// <summary>
        /// Handles subscribed <see cref="ClearStrategies()"/> action.
        /// </summary>
        public ICommand ClearCommand => new ActionCommand(ClearStrategies);

        /// <summary>
        /// Handles subscribed <see cref="Process()"/> action.
        /// </summary>
        public ICommand ProcessCommand => new ActionCommand(Process);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(  // NOTE: Used for Dependency Injection
            ActiveLayerViewModel activeLayerViewModel,
            IncrementNumberViewModel incrementViewModel,
            PrependAppendViewModel prependViewModel,
            LeadingZerosViewModel zerosViewModel)
            : base()
        {
            this.Layer = activeLayerViewModel;

            this.IncrementNumberStrategy = incrementViewModel;
            this.PrependAppendStrategy = prependViewModel;
            this.LeadingZerosStrategy = zerosViewModel;

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
        /// Determines which strategy is selected (if any).
        /// </summary>
        /// <param name="parameter">
        ///   The event's argument (<see cref="MouseButtonEventArgs"/>) passed
        ///   when the specific trigger was activated on the XAML side.
        /// </param>
        public override sealed void Select(object parameter)
        {
            // When any "Label" is clicked ("MouseLeftButtonDown" event) remove focus from all "TextBox" controls
            if (parameter is MouseButtonEventArgs)
            {
                Keyboard.ClearFocus();
            }

            // Check which strategy is active
            this._activeStrategy = this.IncrementNumberStrategy.IsEnabled ? this.IncrementNumberStrategy :
                                   this.PrependAppendStrategy.IsEnabled   ? this.PrependAppendStrategy   :
                                   this.LeadingZerosStrategy.IsEnabled    ? this.LeadingZerosStrategy    :
                                   null;

            UpdateMainButtons();
        }

        /// <summary>
        /// Deselects all strategies.
        /// </summary>
        public override sealed void Deselect()  // NOTE: Specific behavior for the hub of other view models. Overloading restricted
        {
            this.IncrementNumberStrategy.DeselectCommand.Execute(null);
            this.PrependAppendStrategy.DeselectCommand.Execute(null);
            this.LeadingZerosStrategy.DeselectCommand.Execute(null);
        }

        /// <summary>
        /// Clears the list of files and resets all of the strategies.
        /// </summary>
        public override sealed void Reset()  // NOTE: Specific behavior for the hub of other view models. Overloading restricted
        {
            ClearFiles();
            ClearStrategies();
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
        /// <exception cref="InvalidOperationException"/>
        private void LoadFiles(object parameter)
        {
            if (parameter is DragEventArgs dragEvent &&
                dragEvent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Clear the previous state of the files list
                ClearFiles();

                // Get paths from dragged files
                string[] droppedFilesPaths = (string[])dragEvent.Data.GetData(DataFormats.FileDrop, autoConvert: true);

                // Populate the list
                foreach (string filePath in droppedFilesPaths)
                {
                    if (Validate.IsFilePathValid(filePath, out Match filePathMatch))  // Ignore files that doesn't match the pattern "[name].[extension]"
                    {
                        this.Files.Add(new FileData(filePathMatch));
                    }
                    else
                    {
                        ClearFiles();
                        UpdateMainButtons();  // NOTE: Cleaning up "Reset" button would be blocked until user click "OK" on the pupup

                        _ = Message.ErrorOk(Resources.ERROR_Validation_Files_PathNotRecognized_Header,
                                            Resources.ERROR_Validation_Files_PathNotRecognized_Text + $" \"{Path.GetFileName(filePath)}\"");
                        return;
                    }
                }

                this.Counter = this.Files.Count;

                UpdateMainButtons();
            }
            else
            {
                Validate.ReportInvalidCommandUsage(nameof(LoadFiles));
            }
        }

        /// <summary>
        /// Clears the loaded files and their count.
        /// </summary>
        private void ClearFiles()
        {
            this.Files.Clear();
            this.Counter = default;
        }

        /// <summary>
        /// Processes the currently active strategy (a view model extending <see cref="StrategyBase"/>).
        /// </summary>
        private void Process()
        {
            if (this.CanProcess)
            {
                RenamingResultDto result = this._activeStrategy!.Process(this.Files);  // NOTE: Collection is modified by reference

                this._activeStrategy!.DisplayPopup(result);
            }
        }

        /// <summary>
        /// Clears certain elements or controls related to this view model.
        /// </summary>
        private void ClearStrategies()
        {
            this.IncrementNumberStrategy.ResetCommand.Execute(null);
            this.PrependAppendStrategy.ResetCommand.Execute(null);
            this.LeadingZerosStrategy.ResetCommand.Execute(null);

            this._activeStrategy = null;

            UpdateMainButtons();
        }

        private void UpdateMainButtons()
        {
            // NOTE: At least one condition is required
            this.CanReset = this.IsFileListLoaded || this.IsStrategySelected;

            // NOTE: Just ony condition is required
            this.CanClear = this.IsStrategySelected;

            // NOTE: All conditions are required
            this.CanProcess = this.IsFileListLoaded && this.IsStrategySelected;
        }
        #endregion

        #region Subscriptions
        private void SubscribeEvents()
        {
            this.IncrementNumberStrategy.BeforeOnSelected += Deselect;
            this.IncrementNumberStrategy.AfterOnSelected += Select;

            this.PrependAppendStrategy.BeforeOnSelected += Deselect;
            this.PrependAppendStrategy.AfterOnSelected += Select;

            this.LeadingZerosStrategy.BeforeOnSelected += Deselect;
            this.LeadingZerosStrategy.AfterOnSelected += Select;
        }

        private void UnsubscribeEvents()
        {
            this.IncrementNumberStrategy.BeforeOnSelected -= Deselect;
            this.IncrementNumberStrategy.AfterOnSelected -= Select;

            this.PrependAppendStrategy.BeforeOnSelected -= Deselect;
            this.PrependAppendStrategy.AfterOnSelected -= Select;

            this.LeadingZerosStrategy.BeforeOnSelected -= Deselect;
            this.LeadingZerosStrategy.AfterOnSelected -= Select;
        }
        #endregion
    }
}
