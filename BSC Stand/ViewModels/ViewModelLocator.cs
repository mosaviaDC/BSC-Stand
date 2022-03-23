using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.ViewModels
{
    internal class ViewModelLocator
    {
        public MenuWindowViewModel MenuWindowViewModel => App.Host.Services.GetRequiredService<MenuWindowViewModel>();

        public StandVizualizationViewModel StandVizualizationViewModel => App.Host.Services.GetRequiredService<StandVizualizationViewModel>();
        public StandConfigurationViewModel StandConfigurationViewModel => App.Host.Services.GetRequiredService<StandConfigurationViewModel>();

    }
}
