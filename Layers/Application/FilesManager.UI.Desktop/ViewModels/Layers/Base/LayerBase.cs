using Microsoft.Xaml.Behaviors.Core;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Base
{
    /// <summary>
    /// The main base view model defining basic layer operations.
    /// </summary>
    internal abstract class LayerBase
    {
        // NOTE: All binding elements should be public
        #region Commands (binding)
        /// <summary>
        /// Handles subscribed <see cref="Select(object)"/> action.
        /// </summary>
        public ICommand SelectCommand => new ActionCommand(Select);

        /// <summary>
        /// Handles subscribed <see cref="Deselect()"/> action.
        /// </summary>
        internal ICommand DeselectCommand => new ActionCommand(Deselect);  // NOTE: This command doesn't need to have binding

        /// <summary>
        /// Handles subscribed <see cref="Reset()"/> action.
        /// </summary>
        public ICommand ResetCommand => new ActionCommand(Reset);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerBase"/> class.
        /// </summary>
        protected LayerBase()
        {
        }

        #region Abstract
        /// <summary>
        /// Selects certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Select(object parameter);

        /// <summary>
        /// Deselects certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Deselect();

        /// <summary>
        /// Resets certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Reset();
        #endregion
    }
}
