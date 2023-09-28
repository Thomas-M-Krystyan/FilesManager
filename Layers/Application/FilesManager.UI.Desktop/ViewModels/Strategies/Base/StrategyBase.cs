using FilesManager.UI.Desktop.ViewModels.Base;
using System;
using System.Windows.Controls;

namespace FilesManager.UI.Desktop.ViewModels.Strategies.Base
{
    /// <summary>
    /// Base class for all file manipulation strategies.
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    /// <seealso cref="IDisposable"/>
    internal abstract class StrategyBase : ViewModelBase, IDisposable
    {
        #region Properties
        private bool _isEnabled = false;

        /// <summary>
        /// Determines whether this strategy is activated / enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                OnPropertyChanged(nameof(this.IsEnabled));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Performs cleanup <see cref="Action"/>s.
        /// </summary>
        protected event Action OnClear = () => { };
        #endregion

        #region Methods
        /// <summary>
        /// Clears elements related to this strategy (<see cref="RadioButton"/> and input fields).
        /// </summary>
        protected abstract void Clear();
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
