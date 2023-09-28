using FilesManager.UI.Desktop.ViewModels.Root;
using System.Windows;

namespace FilesManager.UI.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs arguments)
        {
            // Sets the context of the application (MainWindow.xaml.cs)
            this.MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
            };

            // Displays the graphic layer of the application (MainWindow.xaml)
            this.MainWindow.Show();

            base.OnStartup(arguments);
        }
    }
}
