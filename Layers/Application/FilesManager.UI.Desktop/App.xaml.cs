using FilesManager.UI.Desktop.ViewModels.Root;
using System.Windows;

namespace FilesManager.UI.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs @event)
        {
            this.MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
            };

            this.MainWindow.Show();

            base.OnStartup(@event);
        }
    }
}
