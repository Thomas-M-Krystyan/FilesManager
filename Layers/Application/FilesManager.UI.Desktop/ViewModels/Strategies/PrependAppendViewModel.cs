﻿using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    /// <summary>
    /// The strategy to add text before and after the original file name.
    /// </summary>
    /// <seealso cref="StrategyBase"/>
    internal sealed class PrependAppendViewModel : StrategyBase
    {
        private readonly IFilePathConverter<Match, FilePathNameDto> _converter;

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
            this._converter = new FileDtoPathNameConverter();
        }

        #region Polymorphism
        /// <inheritdoc cref="StrategyBase.Process(ObservableCollection{FileData})"/>
        internal override sealed RenamingResultDto Process(ObservableCollection<FileData> loadedFiles)
        {
            // -----------------------------------------
            // 1. Validate if there are any input errors
            // -----------------------------------------
            if (this.HasErrors)
            {
                return RenamingResultDto.Failure(GetAllErrors());
            }

            // --------------------------------
            // 2. Process renaming of the files
            // --------------------------------
            RenamingResultDto result = TryUpdatingFiles(loadedFiles, GetNewFilePath);

            // ------------------------------
            // 3. Finalization of the process
            // ------------------------------
            if (result.IsSuccess)
            {
                this.PrependName = string.Empty;
                this.AppendName = string.Empty;
            }

            return result;
        }

        /// <inheritdoc cref="StrategyBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.PrependName = string.Empty;
            this.AppendName = string.Empty;

            base.Reset();
        }

        /// <inheritdoc cref="StrategyBase.GetNewFilePath(FileData)"/>
        protected internal override sealed string GetNewFilePath(FileData fileData)
        {
            return this._converter.GetFilePath(new
            (
                path: fileData.Dto.Path,
                name: $"{this.PrependName.TrimOnlyWhiteSpaces()}" +
                      $"{fileData.Dto.Name}" +
                      $"{this.AppendName.TrimOnlyWhiteSpaces()}",
                extension: fileData.Dto.Extension
            ));
        }
        #endregion
    }
}
