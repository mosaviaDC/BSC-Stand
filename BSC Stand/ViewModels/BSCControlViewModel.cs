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
        private ObservableCollection<ConfigurationMode> bus100ConfigModes;
        private ObservableCollection<ConfigurationMode> bus27ConfigModes;

        public BSCControlViewModel(StandConfigurationViewModel standConfigurationViewModel) 
        {
            RealTimeStandControlService real = new RealTimeStandControlService(this,standConfigurationViewModel );
        }

    }
}
