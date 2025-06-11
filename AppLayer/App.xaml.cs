using AppLayer.Model.Entities;
using AppLayer.Model.Interfaces;
using AppLayer.Services.Handlers;
using AppLayer.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AppLayer
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
            services.AddSingleton<IImageHandler, ResizeHandler>();
            services.AddSingleton<IImageHandler, CompressHandler>();
            services.AddSingleton<IImageHandler, RemoveEXIFHandler>();
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
