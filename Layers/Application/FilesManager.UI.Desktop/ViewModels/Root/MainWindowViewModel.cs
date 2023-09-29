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
        #region View Models
        /// <inheritdoc cref="IncrementNumberViewModel"/>
        public IncrementNumberViewModel IncrementNumberStrategy { get; internal set; } = new();

        /// <inheritdoc cref="PrependAppendViewModel"/>
        public PrependAppendViewModel PrependAppendStrategy { get; internal set; } = new();

        /// <inheritdoc cref="LeadingZerosViewModel"/>
        public LeadingZerosViewModel LeadingZerosStrategy { get; internal set; } = new();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        internal MainWindowViewModel() : base()
        {
            SubscribeEvents();
        }

        /// <summary>
        /// Destructs this instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        ~MainWindowViewModel()
        {
            UnsubscribeEvents();
        }

        #region Polymorphism
        /// <summary>
        /// Deselects all strategies.
        /// </summary>
        protected override sealed void Deselect()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            this.IncrementNumberStrategy.DeselectCommand.Execute(null);
            this.PrependAppendStrategy.DeselectCommand.Execute(null);
            this.LeadingZerosStrategy.DeselectCommand.Execute(null);
        }

        /// <summary>
        /// Resets all strategies.
        /// </summary>
        protected override sealed void Reset()  // NOTE: Specific behavior of the hub for other view models. Overloading restricted
        {
            this.IncrementNumberStrategy.ResetCommand.Execute(null);
            this.PrependAppendStrategy.ResetCommand.Execute(null);
            this.LeadingZerosStrategy.ResetCommand.Execute(null);
        }
        #endregion

        #region Subscriptions
        private void SubscribeEvents()
        {
            this.IncrementNumberStrategy.OnSelected += Deselect;
            this.PrependAppendStrategy.OnSelected += Deselect;
            this.LeadingZerosStrategy.OnSelected += Deselect;
        }

        private void UnsubscribeEvents()
        {
            this.IncrementNumberStrategy.OnSelected -= Deselect;
            this.PrependAppendStrategy.OnSelected -= Deselect;
            this.LeadingZerosStrategy.OnSelected -= Deselect;
        }
        #endregion
    }
}
