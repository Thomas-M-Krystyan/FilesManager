using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Strategies.Base;
using System.Collections.Generic;

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
        private byte _leadingZeros;
        public byte LeadingZeros
        {
            get => this._leadingZeros;
            set
            {
                this._leadingZeros = value;
                OnPropertyChanged(nameof(this.LeadingZeros));
            }
        }
        #endregion

        #region Properties
        internal ushort MaxFileLength { get; set; }
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
            this.LeadingZeros = default;

            base.Reset();
        }

        /// <inheritdoc cref="StrategyBase.GetNewFilePath(string)"/>
        protected internal override sealed string GetNewFilePath(string oldFilePath)
        {
            //var dto = FilePathConverter.GetPathZerosDigitsExtension(oldFilePath);

            //RenamingResultDto result = Validate.IsPathDtoValid(dto, dto.FullName);

            //if (!result.IsSuccess)
            //{
            //    throw new InvalidOperationException(result.Message);
            //}

            //Match filePathMatch = Validate.IsFilePathValid(dto.Path);

            //return filePathMatch.Success
            //    ? FilePathConverter.GetFilePath(
            //        path: dto.Path,
            //        name: $"{GetDigitsWithLeadingZeros(dto.Digits, this.LeadingZeros, this.MaxFileLength)}" +
            //              $"{dto.Name}",
            //        extension: dto.Extension)
            //    : string.Empty;

            return string.Empty;
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Adds the leading zeros to the beginning of the file.
        /// </summary>
        /// <param name="initialDigits">The extracted digits part of the file name.</param>
        /// <param name="zerosCount">The amount of zeros to be added. Cannot be 0.</param>
        /// <param name="maxNumberLength">The lenght of the longest numeric component.</param>
        /// <returns>New file name preceeded by zeros.</returns>
        internal static string GetDigitsWithLeadingZeros(string initialDigits, byte zerosCount, int maxNumberLength)
        {
            if (zerosCount == 0 || maxNumberLength <= 0)
            {
                return initialDigits;
            }

            int zerosToAdd = initialDigits.Length == maxNumberLength
                ? zerosCount
                : maxNumberLength - initialDigits.Length + zerosCount;

            return $"{new string('0', zerosToAdd)}{initialDigits}";
        }
        #endregion
    }
}
