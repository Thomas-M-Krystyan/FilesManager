using FilesManager.Core.Converters;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    /// <summary>
    /// The strategy to update the file name by appending to it leading zeroes.
    /// </summary>
    /// <seealso cref="StrategyBase"/>
    internal sealed class LeadingZerosViewModel : StrategyBase
    {
        #region Texts
        public static readonly string Method_Header = Resources.Header_Method_LeadingZeros;
        public static readonly string Method_Tooltip = Resources.Tooltip_Method_LeadingZeros;

        public static readonly string Set_Label = Resources.Label_Set;
        public static readonly string Set_Tooltip = Resources.Tooltip_Set;
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
        internal int MaxFileLength { get; set; }
        #endregion

        /// <summary>
        /// Renames given files adding a specific amount of leading zeros before the name.
        /// </summary>
        //private void RenameWithLeadingZeros()
        //{
        //    var result = RenamingResultDto.Failure();

        //    // Validate input value (cannot be converted to small positive number; it's either too small, equal to "0", or too large)
        //    if (byte.TryParse(this.LeadingZeros.Text, out byte zerosCount) &&
        //        zerosCount >= 0 && zerosCount <= 7)
        //    {
        //        // Raw items from the files list
        //        ListBoxItem[] listBoxItems = this.FilesList.Items.Cast<ListBoxItem>().ToArray();

        //        // Paths converted into components
        //        PathZerosDigitsExtensionDto[] filePaths = listBoxItems
        //            .Select(fileItem => FilePathConverter.GetPathZerosDigitsExtension((string)fileItem.ToolTip))
        //            .ToArray();

        //        // Only digits components of the names
        //        string[] filesNamesDigits = filePaths.Select(filePath => filePath.Digits)
        //                                             .ToArray();

        //        int maxDigitLength = Helper.GetMaxLength(filesNamesDigits);

        //        // Process renaming of the file
        //        for (int index = 0; index < this.FilesList.Items.Count; index++)
        //        {
        //            result = RenamingService.SetLeadingZeros((string)listBoxItems[index].ToolTip, filePaths[index],
        //                                                     zerosCount, maxDigitLength);
        //            // Validate renaming result
        //            if (!result.IsSuccess)
        //            {
        //                break;
        //            }

        //            UpdateNameOnList(listBoxItems[index], result.NewFilePath);
        //        }

        //        // Reset input field
        //        this.LeadingZeros.Text = string.Empty;
        //    }
        //    else
        //    {
        //        result = RenamingResultDto.Failure($"Invalid value in \"Leading zeros\": " +
        //            $"{(string.IsNullOrWhiteSpace(this.LeadingZeros.Text) ? "Empty" : this.LeadingZeros.Text)}.");
        //    }

        //    DisplayPopup(result);
        //}

        #region Polymorphism
        /// <inheritdoc cref="StrategyBase.Process(IList{FileData})"/>
        internal override sealed RenamingResultDto Process(IList<FileData> loadedFiles)
        {
            return RenamingResultDto.Success();
        }

        /// <inheritdoc cref="StrategyBase.Reset()"/>
        protected override sealed void Reset()  // NOTE: Speficic behavior for this concrete strategy. Overloading restricted
        {
            this.LeadingZeros = DefaultStartingNumber;

            base.Reset();
        }

        /// <inheritdoc cref="StrategyBase.GetNewFilePath(Match)"/>
        protected internal override sealed string GetNewFilePath(Match filePathMatch)
        {
            PathZerosDigitsExtensionDto fileDto = FilePathConverter.GetPathZerosDigitsExtension(filePathMatch);

            return FilePathConverter.GetFilePath(
                path: fileDto.Path,
                name: $"{GetDigitsWithLeadingZeros(fileDto)}" +
                      $"{fileDto.Name}",
                extension: fileDto.Extension);
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

            //int zerosToAdd = fileDto.Digits.Length == this._currentLeadingZeros
            //    ? this._currentLeadingZeros
            //    : this.MaxFileLength - fileDto.Digits.Length + this._currentLeadingZeros;

            return $"{new string('0', this._currentLeadingZeros)}{fileDto.Digits}";
        }
        #endregion
    }
}
