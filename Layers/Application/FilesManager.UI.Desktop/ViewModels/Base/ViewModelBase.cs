using FilesManager.UI.Desktop.Properties;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Base
{
    /// <summary>
    /// Base class for all view models in MVVM architecture.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged"/>
    /// <seealso cref="IDisposable"/>
    internal abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region Texts
        public static readonly string Content_NonEmptyField_Tooltip = Resources.Tooltip_Tip_Content_NonEmptyField;
        public static readonly string Content_OnlyPositives_Tooltip = Resources.Tooltip_Tip_Content_OnlyPositives;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Commands
        /// <summary>
        /// Occurs when the reset was requested on this view model.
        /// </summary>
        public ICommand OnReset => new ActionCommand(Reset);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
        }

        #region Regular
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
        /// Clears some elements related to this view model (e.g., <see cref="RadioButton"/>(s) or input fields).
        /// </summary>
        protected abstract void Reset();
        #endregion

        #region IDisposable pattern
        private protected bool _disposed;

        /// <inheritdoc cref="IDisposable.Dispose()"/>
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="IDisposable.Dispose()"/>
        protected abstract void Dispose(bool disposing);
        #endregion
    }
}
