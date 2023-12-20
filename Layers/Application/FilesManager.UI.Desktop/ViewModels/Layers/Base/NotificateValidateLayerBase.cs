using FilesManager.UI.Common.Properties;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Base
{
    /// <inheritdoc cref="INotifyDataErrorInfo"/>   => Validation
    /// <seealso cref="INotifyPropertyChanged"/>    => Notification
    /// <seealso cref="LayerBase"/>                 => Layer operations
    internal abstract class NotificateValidateLayerBase : NotificateLayerBase, INotifyDataErrorInfo
    {
        #region Fields
        private readonly Dictionary<string /* Property name */, ICollection<string> /* Errors */> _propertyErrors = new();
        #endregion

        #region Properties
        /// <inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors => this._propertyErrors.Count > 0;
        #endregion

        #region Events
        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificateValidateLayerBase"/> class.
        /// </summary>
        protected NotificateValidateLayerBase() : base()
        {
        }

        #region Public
        /// <inheritdoc cref="INotifyDataErrorInfo.GetErrors(string?)"/>
        public IEnumerable GetErrors(string? propertyName)
        {
            return string.IsNullOrWhiteSpace(propertyName)
                ? Array.Empty<string>()
                : this._propertyErrors.GetValueOrDefault(propertyName, Array.Empty<string>());
        }
        #endregion

        #region Protected
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

            if (!this._propertyErrors.TryGetValue(propertyName, out _))
            {
                this._propertyErrors.Add(propertyName, new List<string>());
            }

            this._propertyErrors[propertyName].Add($"{propertyName} | {errorMessage}: \"{value}\".");

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
    }
}
