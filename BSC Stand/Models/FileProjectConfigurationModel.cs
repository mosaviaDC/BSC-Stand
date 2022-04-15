using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models
{
    internal class FileProjectConfigurationModel
    {
       public  FileProjectConfigurationModel(ObservableCollection<ConfigurationMode> V100BusConfigurationModes, ObservableCollection<ConfigurationMode> V27BusConfigurationModes)
        {

            this.V100BusConfigurationModes = V100BusConfigurationModes;
            this.V27BusConfigurationModes = V27BusConfigurationModes;
        }
       public ObservableCollection<ConfigurationMode> V100BusConfigurationModes { get; set; }
       public ObservableCollection<ConfigurationMode> V27BusConfigurationModes { get; set; }

    }
}
