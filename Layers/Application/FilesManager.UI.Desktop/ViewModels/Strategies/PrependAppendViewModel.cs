﻿using FilesManager.Core.Converters;
using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.ObjectModel;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    /// <summary>
    /// The strategy to add text before and after the original file name.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    internal sealed class PrependAppendViewModel : StrategyBase<PathNameExtensionDto>
    {
        #region Texts
        public static readonly string Method_Header = Resources.Header_Method_PrependAppend;
        public static readonly string Method_Tooltip = Resources.Tooltip_Method_PrependAppend;

        public static readonly string Prepend_Label = Resources.Label_Prepend;
        public static readonly string Prepend_Tooltip = Resources.Tooltip_Prepend;
        public static readonly string Append_Label = Resources.Label_Append;
        public static readonly string Append_Tooltip = Resources.Tooltip_Append;
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        private string _prependName = string.Empty;
        public string PrependName
        {
            get => this._prependName;
            set
            {
                this._prependName = value;
                ValidateIllegalChars(nameof(this.PrependName), value);
                OnPropertyChanged(nameof(this.PrependName));
            }
        }

        private string _appendName = string.Empty;
        public string AppendName
        {
            get => this._appendName;
            set
            {
                this._appendName = value;
                ValidateIllegalChars(nameof(this.AppendName), value);
                OnPropertyChanged(nameof(this.AppendName));
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PrependAppendViewModel"/> class.
        /// </summary>
        internal PrependAppendViewModel() : base()
        {
        }

        // PathNameExtensionDto fileDto = FilePathConverter.GetPathNameExtension(file.Match);


        //private void RenameWithPrependAndAppendedText()
        //{
        //    // Validate strings which are going to be used in file name
        //    RenamingResultDto result = ValidateIllegalCharacters(this.PrependName, this.AppendName);

        //    if (result.IsSuccess)
        //    {
        //        // Process renaming of the file
        //        foreach (ListBoxItem fileItem in this.FilesList.Items)
        //        {
        //            result = RenamingService.EnrichWithPrependAndAppend((string)fileItem.ToolTip, this.PrependName, this.AppendName);

        //            // Validate renaming result
        //            if (!result.IsSuccess)
        //            {
        //                break;
        //            }

        //            UpdateNameOnList(fileItem, result.NewFilePath);
        //        }

        //        // Reset input fields
        //        this.PrependName = string.Empty;
        //        this.AppendName = string.Empty;
        //    }

        //    DisplayPopup(result);
        //}

        #region Polymorphism
        /// <inheritdoc cref="StrategyBase.Process(ObservableCollection{FileData})"/>
        internal override sealed RenamingResultDto Process(ObservableCollection<FileData> loadedFiles)
        {
            return RenamingResultDto.Success();
        }

        /// <inheritdoc cref="StrategyBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.PrependName = string.Empty;
            this.AppendName = string.Empty;

            base.Reset();
        }

        /// <inheritdoc cref="StrategyBase{TFileDto}.GetNewFilePath(TFileDto)"/>
        protected internal override sealed string GetNewFilePath(PathNameExtensionDto fileDto)
        {
            return FilePathConverter.GetFilePath(
                path: fileDto.Path,
                name: $"{this.PrependName.GetValueOrEmpty()}" +
                      $"{fileDto.Name}" +
                      $"{this.AppendName.GetValueOrEmpty()}",
                extension: fileDto.Extension);
        }
        #endregion
    }
}
