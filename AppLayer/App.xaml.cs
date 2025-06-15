using AppCore.Model.Interfaces;
using AppCore.Services.Handlers;
using AppCore.Services.Handlers.ModifierHandlers;
using AppCore.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace AppCore
{

    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IMainViewModel, MainViewModel>();
            services.AddSingleton<IImageHandlerAsync, ResizeHandler>();
            services.AddSingleton<IImageHandlerAsync, CompressHandler>();
            services.AddSingleton<IImageHandlerAsync, RemoveEXIFHandler>();
            services.AddSingleton<IDataProviderAsync, ImageInfoHandler>();
            services.AddSingleton<MainWindow>();



            // Register Views
            services.AddSingleton<MainWindow>();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            // Dispose of services if needed
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
