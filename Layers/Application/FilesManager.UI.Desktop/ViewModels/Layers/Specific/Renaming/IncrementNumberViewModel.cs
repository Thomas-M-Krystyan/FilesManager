﻿using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Extensions;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Layers.Specific.Renaming.Base;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Specific.Renaming
{
    /// <summary>
    /// The strategy to update the file name by using prefix, incremented number, and postfix.
    /// </summary>
    /// <seealso cref="RenamingBase"/>
    internal sealed class IncrementNumberViewModel : RenamingBase
    {
        private readonly IFilePathConverter<Match, FilePathNameDto> _converter;

        #region Texts
        public static readonly string Method_Header = Resources.Header_Method_IncrementNumber;
        public static readonly string Method_Tooltip = Resources.Tooltip_Method_IncrementNumber;

        public static readonly string Prefix_Label = Resources.Label_Prefix;
        public static readonly string Prefix_Tooltip = Resources.Tooltip_Prefix;
        public static readonly string StartNumber_Label = Resources.Label_StartNumber;
        public static readonly string StartNumber_Tooltip = Resources.Tooltip_StartNumber;
        public static readonly string Postfix_Label = Resources.Label_Postfix;
        public static readonly string Postfix_Tooltip = Resources.Tooltip_Postfix;
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        private string _namePrefix = string.Empty;
        public string NamePrefix
        {
            get => this._namePrefix;
            set
            {
                this._namePrefix = value;
                ValidateIllegalChars(nameof(this.NamePrefix), value);
                OnPropertyChanged(nameof(this.NamePrefix));
            }
        }

        private ushort _startingNumber;
        public ushort StartingNumber
        {
            get => this._startingNumber;
            set
            {
                this._startingNumber = value;
                //ValidateOnlyNumbers(nameof(this.StartingNumber), value, out this._currentStartingNumber);
                OnPropertyChanged(nameof(this.StartingNumber));
            }
        }

        private string _namePostfix = string.Empty;
        public string NamePostfix
        {
            get => this._namePostfix;
            set
            {
                this._namePostfix = value;
                ValidateIllegalChars(nameof(this.NamePostfix), value);
                OnPropertyChanged(nameof(this.NamePostfix));
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementNumberViewModel"/> class.
        /// </summary>
        public IncrementNumberViewModel(  // Used for Dependency Injection
            IFilePathConverter<Match, FilePathNameDto> converter)
            : base()
        {
            this._converter = converter;
        }

        #region Polymorphism
        /// <inheritdoc cref="RenamingBase.Process(IList{FileData}))"/>
        internal override sealed RenamingResultDto Process(IList<FileData> loadedFiles)
        {
            // -----------------------------------------
            // 1. Validate if there are any input errors
            // -----------------------------------------
            if (this.HasErrors)
            {
                return RenamingResultDto.Failure(GetAllErrors());
            }

            // ---------------------------------
            // 2. Validate additional conditions
            // ---------------------------------
            if (this.StartingNumber + loadedFiles.Count - 1 > ushort.MaxValue)  // Exceeding the maximum possible value
            {
                // EXAMPLE: "startNumber" is 65530 and there is 6 files on the list. The result of ++ would be 65536 => which is more than maximum for ushort
                return RenamingResultDto.Failure(Resources.ERROR_Validation_Field_ValueWillExceedUshortMax);
            }

            // --------------------------------
            // 3. Process renaming of the files
            // --------------------------------
            RenamingResultDto result = TryUpdatingFiles(loadedFiles, GetNewFilePath);

            // ------------------------------
            // 4. Finalization of the process
            // ------------------------------
            if (result.IsSuccess)
            {
                // Set the last number as the new start number
                this.StartingNumber = this.StartingNumber is ushort.MinValue
                    ? --this.StartingNumber  // Revert the effect of value overflow (ushort.MaxValue + 1 => 0). Keep the ushort.MaxValue
                    : this.StartingNumber;
            }

            return result;
        }

        /// <inheritdoc cref="RenamingBase.Reset()"/>
        public override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.NamePrefix = string.Empty;
            this.StartingNumber = default;
            this.NamePostfix = string.Empty;

            base.Reset();
        }

        /// <inheritdoc cref="RenamingBase.GetNewFilePath(FileData)"/>
        protected internal override sealed string GetNewFilePath(FileData fileData)
        {
            return this._converter.GetFilePath(new
            (
                path: fileData.Dto.Path,
                // Updating the "Name"
                name: $"{this.NamePrefix.TrimOnlyWhiteSpaces()}" +
                      $"{this.StartingNumber++}" +  // NOTE: Very important! Keep incrementing the current number
                      $"{this.NamePostfix.TrimOnlyWhiteSpaces()}",
                extension: fileData.Dto.Extension
            ));
        }
        #endregion
    }
}
