using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
using BSC_Stand.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BSC_Stand.Services
{
    internal class RealTimeStandControlService 
    {

        private static DispatcherTimer expiremtTimer { get; set; }

        private ObservableCollection<ConfigurationMode> V27configurationModes;

        private ObservableCollection<ConfigurationMode> V100configurationModes;

        int Duration;
        public RealTimeStandControlService(BSCControlViewModel bSCControlViewModel, StandConfigurationViewModel standConfigurationViewModel)
        {
            expiremtTimer = new DispatcherTimer();
            expiremtTimer.Interval = TimeSpan.FromMilliseconds(250);
            expiremtTimer.Tick += TimerEventHandler;
            expiremtTimer.Start();
            V27configurationModes = standConfigurationViewModel.Bus27ConfigurationModes;
            V100configurationModes = standConfigurationViewModel.Bus100ConfigurationModes;
           
        }

        public void TimerEventHandler(object sender, EventArgs e)
        {
            
            
        }





    }
}
