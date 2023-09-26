using FilesManager.UI.Desktop.Properties;

namespace FilesManager.UI.Desktop.ViewModels.Strategies
{
    internal sealed class IncrementNumberViewModel : ViewModelBase
    {
        // Texts
        public static readonly string Method_Header = Resources.Header_Method_IncrementNumber;
        public static readonly string Method_Tooltip = Resources.Tooltip_Method_IncrementNumber;
        public static readonly string RadioButton_Tooltip = Resources.Tooltip_RadioButton;
        public static readonly string Prefix_Label = Resources.Label_Prefix;
        public static readonly string Prefix_Tooltip = Resources.Tooltip_Prefix;
        public static readonly string StartNumber_Label = Resources.Label_StartNumber;
        public static readonly string StartNumber_Tooltip = Resources.Tooltip_StartNumber;
        public static readonly string Postfix_Label = Resources.Label_Postfix;
        public static readonly string Postfix_Tooltip = Resources.Tooltip_Postfix;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementNumberViewModel"/> class.
        /// </summary>
        internal IncrementNumberViewModel()
        {
        }
    }
}
