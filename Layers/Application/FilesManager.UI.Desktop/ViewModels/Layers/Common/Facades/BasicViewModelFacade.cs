using FilesManager.UI.Desktop.ViewModels.Layers.Common.Interfaces;
using Microsoft.Xaml.Behaviors.Core;
using System.ComponentModel;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Common.Facades
{
    /// <summary>
    /// This class is a facade for the <see cref="ISelectableViewModel"/> and <see cref="IObservableViewModel"/> interfaces.
    /// </summary>
    /// <seealso cref="ISelectableViewModel"/>
    /// <seealso cref="IObservableViewModel"/>
    internal abstract class BasicViewModelFacade : ISelectableViewModel, IObservableViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicViewModelFacade"/> class.
        /// </summary>
        protected BasicViewModelFacade()
        {
        }

        #region ISelectableViewModel
        // --------
        // Commands
        // --------

        /// <inheritdoc cref="ISelectableViewModel.SelectCommand"/>
        public ICommand SelectCommand => new ActionCommand(Select);

        /// <inheritdoc cref="ISelectableViewModel.DeselectCommand"/>
        public ICommand DeselectCommand => new ActionCommand(Deselect);

        /// <inheritdoc cref="ISelectableViewModel.ResetCommand"/>
        public ICommand ResetCommand => new ActionCommand(Reset);

        // -------
        // Methods
        // -------

        /// <inheritdoc cref="ISelectableViewModel.Select(object)"/>
        public abstract void Select(object parameter);

        /// <inheritdoc cref="ISelectableViewModel.Deselect()"/>
        public abstract void Deselect();

        /// <inheritdoc cref="ISelectableViewModel.Reset()"/>
        public abstract void Reset();
        #endregion

        #region IObservableViewModel
        // ------
        // Events
        // ------

        /// <inheritdoc cref="IObservableViewModel.PropertyChanged"/>
        private event PropertyChangedEventHandler? PropertyChanged;

        // -------
        // Methods
        // -------

        /// <inheritdoc cref="IObservableViewModel.OnPropertyChanged(string)"/>
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
