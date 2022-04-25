using BSC_Stand.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSC_Stand.Services;
using System.Diagnostics;
using System.Collections.ObjectModel;
using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;

namespace BSC_Stand.ViewModels
{
    internal class BSCControlViewModel:ViewModelBase
    {
        private StandConfigurationViewModel _standConfigurationViewModel;
        private RealTimeStandControlService _realTimeStandControlService;

        public BSCControlViewModel(StandConfigurationViewModel standConfigurationViewModel) 
        {
         _standConfigurationViewModel = standConfigurationViewModel;
         _realTimeStandControlService = new RealTimeStandControlService(this, _standConfigurationViewModel);

        }




        public void SendV27ModBusCommand(ConfigurationMode configurationMode)
        {
            Debug.WriteLine($"Send V27 ModBus Command {DateTime.Now} {configurationMode.MaxValue}");
        }
        public void SendV100ModBusCommand(ConfigurationMode configurationMode)
        {
            Debug.WriteLine($"Send V100 ModBus Command {DateTime.Now} {configurationMode.MaxValue}");
        }


        public void StartExpiremnt()
        {
          
            _realTimeStandControlService.StartExpirent();
        }
    }
}
