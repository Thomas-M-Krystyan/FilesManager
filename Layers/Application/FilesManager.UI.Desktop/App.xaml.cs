using FilesManager.Core.Converters;
using FilesManager.Core.Converters.Interfaces;
using FilesManager.Core.Models.DTOs.Files;
using FilesManager.UI.Desktop.ViewModels.Root;
using FilesManager.UI.Desktop.ViewModels.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Windows;

namespace FilesManager.UI.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = ConfigureServices(new ServiceCollection());

            this._serviceProvider = services.BuildServiceProvider();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            // ViewModels
            _ = services.AddSingleton<MainWindowViewModel>();
            _ = services.AddSingleton<IncrementNumberViewModel>();
            _ = services.AddSingleton<PrependAppendViewModel>();
            _ = services.AddSingleton<LeadingZerosViewModel>();

            // Converters
            _ = services.AddSingleton<IFilePathConverter<Match,           FilePathNameDto>,    FilePathNameDtoConverter>();
            _ = services.AddSingleton<IFilePathConverter<FilePathNameDto, FileZerosDigitsDto>, FileZerosDigitsDtoConverter>();

            return services;
        }

        protected override void OnStartup(StartupEventArgs arguments)
        {
            // Sets the context of the application (MainWindow.xaml.cs)
            this.MainWindow = new MainWindow()
            {
                DataContext = this._serviceProvider.GetService<MainWindowViewModel>()
            };

            // Displays the graphic layer of the application (MainWindow.xaml)
            this.MainWindow.Show();

            base.OnStartup(arguments);
        }
    }
}
