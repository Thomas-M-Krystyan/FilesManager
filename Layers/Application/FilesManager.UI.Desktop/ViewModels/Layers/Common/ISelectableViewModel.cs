using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Common
{
    /// <summary>
    /// The basic "select", "deselect", and "reset" operations used by view models.
    /// </summary>
    internal interface ISelectableViewModel
    {
        #region Commands
        /// <summary>
        /// Handles subscribed <see cref="Select(object)"/> action.
        /// </summary>
        internal ICommand SelectCommand { get; }

        /// <summary>
        /// Handles subscribed <see cref="Deselect()"/> action.
        /// </summary>
        internal ICommand DeselectCommand { get; }

        /// <summary>
        /// Handles subscribed <see cref="Reset()"/> action.
        /// </summary>
        internal ICommand ResetCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Selects certain elements or controls related to this view model.
        /// </summary>
        internal void Select(object parameter);

        /// <summary>
        /// Deselects certain elements or controls related to this view model.
        /// </summary>
        internal void Deselect();

        /// <summary>
        /// Resets certain elements or controls related to this view model.
        /// </summary>
        internal void Reset();
        #endregion
    }
}
