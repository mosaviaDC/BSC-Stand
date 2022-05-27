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
        public BSCControlViewModel BSCControlViewModel => App.Host.Services.GetRequiredService<BSCControlViewModel>();
        public StatusBarViewModel StatusBarViewModel => App.Host.Services.GetRequiredService<StatusBarViewModel>();
        public ConnectionCheckListViewModel ConnectionCheckListViewModel => App.Host.Services.GetRequiredService<ConnectionCheckListViewModel>();

        public RealTimeGraphsViewModel RealTimeGraphsViewModel => App.Host.Services.GetRequiredService<RealTimeGraphsViewModel>();
        public RealTimeGraphsLegendViewModel RealTimeGraphsLegendViewModel => App.Host.Services.GetRequiredService<RealTimeGraphsLegendViewModel>();
        public PostAnalyzeViewModel PostAnalyzeViewModel => App.Host.Services.GetRequiredService<PostAnalyzeViewModel>();
    }
}
