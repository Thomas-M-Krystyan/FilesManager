using FilesManager.UI.Desktop.ViewModels.Base;
using FilesManager.UI.Desktop.ViewModels.Strategies;

namespace FilesManager.UI.Desktop.ViewModels.Root
{
    /// <summary>
    /// View model for the main window of the application.
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        // View Models
        public IncrementNumberViewModel IncrementStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        internal MainWindowViewModel() : base()
        {
            this.IncrementStrategy = new IncrementNumberViewModel();
        }

        /// <inheritdoc cref="ViewModelBase.Reset()"/>
        protected override void Reset()
        {
            this.IncrementStrategy.OnReset();
        }

        /// <inheritdoc cref="ViewModelBase.Dispose(bool)"/>
        protected override void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                }

                this._disposed = true;
            }
        }
    }
}
