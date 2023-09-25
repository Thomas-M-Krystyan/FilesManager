using FilesManager.UI.Desktop.Properties;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    internal sealed class IncrementNumberViewModel : ViewModelBase
    {
        public string MethodHeader { get; } = Resources.Header_Method_IncrementNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementNumberViewModel"/> class.
        /// </summary>
        internal IncrementNumberViewModel()
        {
        }
    }
}
