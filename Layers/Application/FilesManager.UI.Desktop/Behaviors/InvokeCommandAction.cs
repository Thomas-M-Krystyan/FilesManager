using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace FilesManager.UI.Desktop.Behaviors
{
    /// <summary>
    /// Custom implementation of <see cref="TriggerAction{T}"/> which is able to receive event arguments (e.g., <see cref="DragEventArgs"/>).
    /// </summary>
    /// <seealso cref="TriggerAction{T}" />
    public class InvokeCommandAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// Registers the declared command as <see cref="DependencyObject"/> to use it with binding on XAML side.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(InvokeCommandAction));

        /// <summary>
        /// The command to be executed.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// The command parameter to be used.
        /// </summary>
        public object? CommandParameter { get; set; }

        /// <inheritdoc cref="Microsoft.Xaml.Behaviors.TriggerAction.Invoke(object)"/>
        protected override void Invoke(object parameter)
        {
            this.Command?.Execute(this.CommandParameter ?? parameter);
        }
    }
}
