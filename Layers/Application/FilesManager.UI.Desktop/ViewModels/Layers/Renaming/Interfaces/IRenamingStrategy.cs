using FilesManager.Core.Models.DTOs.Results;
using FilesManager.Core.Models.POCOs;
using System.Collections.Generic;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Renaming.Interfaces
{
    internal interface IRenamingStrategy
    {
        /// <summary>
        /// Executes logic related to this specific strategy.
        /// </summary>
        /// <param name="loadedFiles">
        ///   The collection of loaded files, dragged and dropped into dedicated UI section.
        ///   <para>
        ///     It will never be null or empty.
        ///   </para>
        /// </param>
        /// <returns>
        ///   <inheritdoc cref="RenamingResultDto" path="/summary"/>
        /// </returns>
        internal RenamingResultDto Process(IList<FileData> loadedFiles);

        /// <summary>
        /// Displays a proper <see cref="MessageBoxResult"/> popup with feedback information.
        /// </summary>
        /// <param name="result">The result of processing operation.</param>
        internal void DisplayPopup(RenamingResultDto result);
    }
}
