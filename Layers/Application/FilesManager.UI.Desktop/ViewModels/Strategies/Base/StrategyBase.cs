using FilesManager.Core.Converters;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Services.Writing;
using FilesManager.Core.Validation;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.Utilities;
using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies.Interfaces;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FilesManager.UI.Desktop.ViewModels.Strategies.Base
{
    /// <summary>
    /// Base class for all file manipulation strategies.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    /// <seealso cref="IRenamingStrategy"/>
    internal abstract class StrategyBase<TFileDto> : ViewModelBase, IRenamingStrategy
        where TFileDto : PathNameExtensionDto, new()
    {
        #region Texts
        public static readonly string RadioButton_Tooltip = Resources.Tooltip_RadioButton;

        public static readonly string Content_NonEmptyField_Tooltip = Resources.Tooltip_Tip_Content_NonEmptyField;
        public static readonly string Content_OnlyPositives_Tooltip = Resources.Tooltip_Tip_Content_OnlyPositives;
        public static readonly string Content_OnlyVerySmallPositives_Tooltip = Resources.Tooltip_Tip_Content_OnlyVerySmallPositives;
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        private bool _isEnabled = false;

        /// <summary>
        /// Determines whether this strategy is activated / enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                OnPropertyChanged(nameof(this.IsEnabled));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs BEFORE the <see cref="Select()"/> - when it was requested on this view model.
        /// </summary>
        public event Action? BeforeOnSelected;

        /// <summary>
        /// Occurs AFTER the <see cref="Select()"/> - when it was requested on this view model.
        /// </summary>
        public event Action<object>? AfterOnSelected;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyBase"/> class.
        /// </summary>
        protected StrategyBase() : base()
        {
        }

        #region IRenamingStrategy
        /// <inheritdoc cref="IRenamingStrategy.Process(ObservableCollection{FileData})"/>
        RenamingResultDto IRenamingStrategy.Process(ObservableCollection<FileData> loadedFiles)
        {
            return Process(loadedFiles);
        }

        /// <inheritdoc cref="IRenamingStrategy.DisplayPopup(RenamingResultDto)"/>
        void IRenamingStrategy.DisplayPopup(RenamingResultDto result)
        {
            _ = result.IsSuccess
                ? Message.InfoOk(Resources.RESULT_Operation_Success_Header, Resources.RESULT_Operation_Success_Text)
                : Message.ErrorOk(Resources.RESULT_Operation_Failure_Header, result.Value);
        }
        #endregion

        #region Polymorphism
        /// <inheritdoc cref="ViewModelBase.Select(object)"/>
        protected override sealed void Select(object parameter)  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            if (!this.IsEnabled)
            {
                this.BeforeOnSelected?.Invoke();  // NOTE: Subscribed action to be invoked BEFORE the selection
                this.IsEnabled = true;
                this.AfterOnSelected?.Invoke(parameter);  // NOTE: Subscribed action to be invoked AFTER the selection
            }
        }

        /// <inheritdoc cref="ViewModelBase.Deselect()"/>
        protected override sealed void Deselect()  // NOTE: Default behavior for all strategies, no need to change it. Overloading restricted
        {
            if (this.IsEnabled)
            {
                this.IsEnabled = false;
            }
        }

        /// <inheritdoc cref="ViewModelBase.Reset()"/>
        protected override void Reset()
        {
            Deselect();
            ClearAllErrors();
        }
        #endregion

        #region Abstract
        /// <inheritdoc cref="IRenamingStrategy.Process(ObservableCollection{FileData})"/>
        internal abstract RenamingResultDto Process(ObservableCollection<FileData> loadedFiles);

        /// <summary>
        /// Gets the new file path for a file to be renamed.
        /// </summary>
        /// <param name="fileDto">The file DTO composed from RegEx <see cref="Match"/>.</param>
        /// <returns>
        ///   The new file path (changed by the current renaming strategy).
        /// </returns>
        protected internal abstract string GetNewFilePath(TFileDto fileDto);
        #endregion

        #region Concrete (Validation)
        /// <summary>
        /// Validates if the value of the specified property contains any illegal characters.
        /// <para>
        ///   A specific <see cref="Action"/>s will be invoked in case of success or failure.
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        protected void ValidateIllegalChars(string propertyName, string propertyValue)
        {
            _ = Validate.ContainInvalidCharacters(propertyValue,
                () => ClearErrors(propertyName),
                () => AddError(propertyName, Resources.ERROR_Validation_Field_ContainsIllegalCharacter, propertyValue));
        }

        /// <summary>
        /// Validates if the value of the specified property contains only numbers (optional: within a specified maximum range).
        /// <para>
        ///   A specific <see cref="Action"/>s will be invoked in case of success or failure.
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="number">The result of string to <paramref name="TNumber"/> conversion.</param>
        /// <param name="maxLimit">The maximum limit of the output number.</param>
        protected void ValidateNumMaxLimit<TNumber>(string propertyName, TNumber number, TNumber maxLimit)
            where TNumber : struct, IComparable, IComparable<TNumber>, IConvertible, IEquatable<TNumber>, IFormattable  // NOTE: Numeric type
        {
            _ = Validate.WithinLimit(number, maxLimit,
            () => ClearErrors(propertyName),
            () => AddError(propertyName, Resources.ERROR_Validation_Field_ExceedsMaxLimit, maxLimit));
        }
        #endregion

        #region Concrete (Logic)
        protected internal  RenamingResultDto TryUpdatingFiles(ObservableCollection<FileData> loadedFiles)
        {
            var result = RenamingResultDto.Failure();
            FileData file;
            TFileDto dto;

            for (ushort index = 0; index < loadedFiles.Count; index++)
            {
                file = loadedFiles[index];
                dto = file.Match.GetPathNameExtensionDto<TFileDto>();
                result = WritingService.RenameFile(file.Path, GetNewFilePath(dto));

                if (result.IsSuccess)
                {
                    UpdateFilesList(loadedFiles, index, () =>
                    {
                        file.Path = result.Value;

                        return file;
                    });
                }
                else
                {
                    loadedFiles.Clear();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the <see cref="ObservableCollection{T}"/> of files.
        /// </summary>
        /// <param name="loadedFiles">The list of files to by updated.</param>
        /// <param name="index">The index of the element to be modified.</param>
        /// <param name="updateLogic">The specific logic how to update selected element.</param>
        protected internal static void UpdateFilesList(ObservableCollection<FileData> loadedFiles, ushort index, Func<FileData> updateLogic)
        {
            if (index > loadedFiles.Count - 1)
            {
                return;  // Index would be out of range
            }

            loadedFiles.RemoveAt(index);  // NOTE: Triggers OnCollectionChanged event

            // Update the file
            FileData file = updateLogic.Invoke();

            loadedFiles.Insert(index, file);  // NOTE: Triggers OnCollectionChanged event
        }
        #endregion
    }
}
