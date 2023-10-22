using FilesManager.UI.Common.Properties;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.ViewModels.Base
{
    /// <summary>
    /// Base class for all view models in MVVM architecture.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged"/>
    internal abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
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
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
        }

        #region INotifyPropertyChanged
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies a specific view model whether its property was changed from XAML code.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyDataErrorInfo
        private readonly Dictionary<string /* Property name */, ICollection<string> /* Errors */> _propertyErrors = new();
        
        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors => this._propertyErrors.Count > 0;

        /// <inheritdoc cref="INotifyDataErrorInfo.GetErrors(string?)"/>
        public IEnumerable GetErrors(string? propertyName)
        {
            return string.IsNullOrWhiteSpace(propertyName)
                ? Array.Empty<string>()
                : this._propertyErrors.GetValueOrDefault(propertyName, Array.Empty<string>());
        }

        /// <summary>
        /// Lists all errors of all properties from the current view model.
        /// </summary>
        protected string GetAllErrors()
        {
            return string.Join(Environment.NewLine, this._propertyErrors.Select(errors =>
                string.Join(Environment.NewLine, GetErrors(errors.Key).Cast<string>())));
        }

        /// <summary>
        /// Adds new validation error related to the given property.
        /// </summary>
        protected void AddError<T>(string propertyName, string errorMessage, T value)
        {
            if (string.IsNullOrWhiteSpace(propertyName) ||
                string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new InvalidOperationException(Resources.ERROR_Internal_PropertyOrMessageAreEmpty);
            }

            if (!this._propertyErrors.ContainsKey(propertyName))
            {
                this._propertyErrors.Add(propertyName, new List<string>());
            }

            this._propertyErrors[propertyName].Add($"{propertyName}: {errorMessage}: \"{value}\".");
            
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Clears all validation errors related to the given property.
        /// </summary>
        protected void ClearErrors(string propertyName)
        {
            _ = this._propertyErrors.Remove(propertyName);
        }

        /// <summary>
        /// Clears all validation errors of all properties from the current view model.
        /// </summary>
        protected void ClearAllErrors()
        {
            this._propertyErrors.Clear();
        }
        #endregion

        #region Abstract
        /// <summary>
        /// Selects certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Select(object parameter);  // NOTE: Should be implemented by all view models

        /// <summary>
        /// Deselects certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Deselect();  // NOTE: Should be implemented by all view models

        /// <summary>
        /// Resets certain elements or controls related to this view model.
        /// </summary>
        protected abstract void Reset();  // NOTE: Should be implemented by all view models
        #endregion
    }
}
