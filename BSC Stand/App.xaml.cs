using BSC_Stand.ViewModels;
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

        public static void ConfigureServices(HostBuilderContext host,IServiceCollection services)
        {
            // services.AddSingleton
            services.AddSingleton<MenuWindowViewModel>();
            services.AddSingleton<StandConfigurationViewModel>();
            services.AddSingleton<StandVizualizationViewModel>();

        }
      
    }
}
