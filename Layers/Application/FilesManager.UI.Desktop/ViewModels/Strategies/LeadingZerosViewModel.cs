﻿using FilesManager.Core.Converters;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.Core.Services.Writing;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    /// <summary>
    /// The strategy to update the file name by appending to it leading zeroes.
    /// </summary>
    /// <seealso cref="StrategyBase"/>
    internal sealed class LeadingZerosViewModel : StrategyBase<PathZerosDigitsExtensionDto>
    {
        #region Texts
        public static readonly string Method_Header = Resources.Header_Method_LeadingZeros;
        public static readonly string Method_Tooltip = Resources.Tooltip_Method_LeadingZeros;

        public static readonly string Set_Label = Resources.Label_Set;
        public static readonly string Set_Tooltip = Resources.Tooltip_Set;

        public static readonly string ClearZeros_Tooltip = Resources.Tooltip_ClearZeros;
        #endregion

        // NOTE: All binding elements should be public
        #region Properties (binding)
        private ushort _currentLeadingZeros;                   // NOTE: Logic
        private string _leadingZeros = DefaultStartingNumber;  // NOTE: UI
        public string LeadingZeros
        {
            get => this._leadingZeros;
            set
            {
                this._leadingZeros = value;
                ValidateOnlyNumbers(nameof(this.LeadingZeros), value, out this._currentLeadingZeros, maxLimit: 7);
                OnPropertyChanged(nameof(this.LeadingZeros));
            }
        }
        #endregion

        #region Properties
        internal int MaxDigitLength { private get; set; }
        #endregion

        #region Polymorphism
        /// <inheritdoc cref="StrategyBase{TFileDto}.Process(ObservableCollection{FileData})"/>
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
            // 2. Preparing the processing data
            // --------------------------------
            this.MaxDigitLength = default;  // Reset value

            PathZerosDigitsExtensionDto[] dtos = loadedFiles.Select(file =>  // NOTE: Executing both logics at once
            {
                // Conversion to DTO
                PathZerosDigitsExtensionDto dto = file.Match.GetPathZerosDigitsExtensionDto();
                
                // Counting max length
                this.MaxDigitLength = Math.Max(this.MaxDigitLength, dto.Digits.Length);

                return dto;
            })
            .ToArray();

            // --------------------------------
            // 3. Process renaming of the files
            // --------------------------------
            var result = RenamingResultDto.Failure();
            FileData file;

            for (ushort index = 0; index < loadedFiles.Count; index++)
            {
                file = loadedFiles[index];
                result = WritingService.RenameFile(file.Path, GetNewFilePath(dtos[index]));

                if (result.IsSuccess)
                {
                    UpdateFilesList(loadedFiles, index, () =>
                    {
                        file.Path = result.NewFilePath;

                        return file;
                    });
                }
            }

            // ------------------------------
            // 4. Finalization of the process
            // ------------------------------
            this.LeadingZeros = DefaultStartingNumber;

            return result;
        }

        /// <inheritdoc cref="StrategyBase{TFileDto}.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.LeadingZeros = DefaultStartingNumber;

            base.Reset();
        }

        /// <inheritdoc cref="StrategyBase{TFileDto}.GetNewFilePath(TFileDto)"/>
        protected internal override sealed string GetNewFilePath(PathZerosDigitsExtensionDto fileDto)
        {
            return fileDto.GetFilePath(
                name: $"{GetDigitsWithLeadingZeros(fileDto)}" +
                      $"{fileDto.Name}");
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Adds the leading zeros to the beginning of the file.
        /// </summary>
        /// <param name="file">The file DTO with extracted parts of the file name (zeros or digits).</param>
        /// <returns>
        ///   The new file name preceeded by zeros.
        /// </returns>
        private string GetDigitsWithLeadingZeros(PathZerosDigitsExtensionDto fileDto)
        {
            // There is no point to process further
            if (this.HasErrors)
            {
                return fileDto.Zeros + fileDto.Digits;
            }

            // Removing zeros mode
            if (this._currentLeadingZeros == 0)
            {
                return fileDto.Digits.Length > 0
                    ? fileDto.Digits                 // Cases: "01.jpg" => "1.jpg" or "01a.png" => "1a.png" (return trimmed digits without zeros)
                    // There is no digits
                    : fileDto.Name.Length > 0
                        ? string.Empty               // Cases: "0test.jpg" => "test.jpg" (name will be added later so, for now return just empty)
                        // There is no name
                        : fileDto.Zeros.Length > 1
                            ? DefaultStartingNumber  // Cases: "00.jpg" => "0.jpg" (try to reduce number of zeros to none, but prevent ".jpg")
                            : fileDto.Zeros;         // Cases: "0.jpg" (if there is no name or digits but "no zeros" was requested. Prevent ".jpg")
            }

            int zerosToAdd = fileDto.Digits.Length == this.MaxDigitLength  // For 2n => MIN: "1" (Length: 1), MAX: "10" (Length: 2)
                ? this._currentLeadingZeros  // (Count: 1) => "0"
                : this.MaxDigitLength + this._currentLeadingZeros - fileDto.Digits.Length;  // 2 (max len) + 1 (count) - 1 (min len) => "00" + "1"

            return $"{new string(char.Parse(DefaultStartingNumber), zerosToAdd)}{fileDto.Digits}";
        }
        #endregion
    }
}
