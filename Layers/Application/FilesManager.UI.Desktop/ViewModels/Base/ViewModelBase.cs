using FilesManager.UI.Desktop.Properties;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Base
{
    /// <summary>
    /// Base class for all view models in MVVM architecture.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged"/>
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Events
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Commands (binding)
        /// <summary>
        /// Handles subscribed <see cref="Select()"/> action.
        /// </summary>
        public ICommand SelectCommand => new ActionCommand(Select);

        /// <summary>
        /// Handles subscribed <see cref="Deselect()"/> action.
        /// </summary>
        public ICommand DeselectCommand => new ActionCommand(Deselect);

        /// <summary>
        /// Handles subscribed <see cref="Reset()"/> action.
        /// </summary>
        public ICommand ResetCommand => new ActionCommand(Reset);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
        }

        #region INotifyPropertyChanged
        /// <summary>
        /// Notifies a specific view model whether its property was changed from XAML code.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Abstract
        /// <summary>
        /// Selects certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Select();  // NOTE: Should be implemented by all view models

        /// <summary>
        /// Deselects certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Deselect();  // NOTE: Should be implemented by all view models

        /// <summary>
        /// Clears certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Reset();  // NOTE: Should be implemented by all view models
        #endregion

        #region Protected
        /// <summary>
        /// Reports the invalid usage of an event.
        /// <para>
        ///   Used for development purposes to monitor if an event from the XAML side was binded to a command
        ///   subscribing to a method which is using a received object parameter as a proper event argument.
        /// </para>
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <exception cref="InvalidOperationException">The event argument is invalid.</exception>
        protected static void ReportInvalidCommandUsage(string methodName)
        {
            throw new InvalidOperationException($"{Resources.ERROR_Operation_WrongEventArg_Exception} {methodName}");
        }
        #endregion
    }
}
