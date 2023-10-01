using System.Windows;

namespace FilesManager.UI.Desktop
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.SizeToContent = SizeToContent.WidthAndHeight;  // NOTE: Adjust window to its content in real-time
        }
    }
}
