using FilesManager.UI.Desktop.ViewModels.Layers.Common.Interfaces;
using System.ComponentModel;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Common.Facades
{
    /// <seealso cref="Selectable"/>
    /// <seealso cref="IObservableViewModel"/>
    internal abstract class BasicViewModelFacade : Selectable, IObservableViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicViewModelFacade"/> class.
        /// </summary>
        protected BasicViewModelFacade()
        {
        }

        #region IObservableViewModel
        /// <inheritdoc cref="IObservableViewModel.PropertyChanged"/>
        private event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc cref="IObservableViewModel.OnPropertyChanged(string)"/>
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
