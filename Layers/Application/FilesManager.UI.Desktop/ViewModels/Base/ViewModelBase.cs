using FilesManager.UI.Desktop.Properties;
using Microsoft.Xaml.Behaviors.Core;
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
        #region Texts
        public static readonly string Content_NonEmptyField_Tooltip = Resources.Tooltip_Tip_Content_NonEmptyField;
        public static readonly string Content_OnlyPositives_Tooltip = Resources.Tooltip_Tip_Content_OnlyPositives;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Commands
        /// <summary>
        /// Occurs when a select was requested on this view model.
        /// </summary>
        public ICommand OnSelected => new ActionCommand(Select);

        /// <summary>
        /// Occurs when a deselect was requested on this view model.
        /// </summary>
        public ICommand OnDeselected => new ActionCommand(Deselect);

        /// <summary>
        /// Occurs when a reset was requested on this view model.
        /// </summary>
        public ICommand OnReset => new ActionCommand(Reset);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
        }

        #region Listener
        /// <summary>
        /// Notifies a specific view model whether its property was changed from XAML code.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Virtual
        /// <summary>
        /// Selects certain elements or controls related to this view model.
        /// </summary>
        protected virtual void Select() { }  // NOTE: "MainWindowViewModel" doesn't need to overload it
        #endregion

        #region Abstract
        /// <summary>
        /// Deselects certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Deselect();  // NOTE: Should be implemented by all view models

        /// <summary>
        /// Clears certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Reset();  // NOTE: Should be implemented by all view models
        #endregion
    }
}
