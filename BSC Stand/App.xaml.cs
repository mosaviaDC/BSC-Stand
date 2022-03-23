﻿using BSC_Stand.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BSC_Stand
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost? _Host;
        public static IHost Host => _Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            _Host = null;
        }

        public static void ConfigureServices(HostBuilderContext host,IServiceCollection services)
        {
            // services.AddSingleton
            services.AddSingleton<MenuWindowViewModel>();
            services.AddSingleton<StandConfigurationViewModel>();
            services.AddSingleton<StandVizualizationViewModel>();

        }
      
    }
}
