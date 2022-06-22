using BSC_Stand.Models.StandConfigurationModels;
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
        public FileProjectConfigurationModel(ObservableCollection<ElectronicConfigMode> V100BusConfigurationModes, ObservableCollection<ElectronicConfigMode> V27BusConfigurationModes, ObservableCollection<PowerSupplyConfigMode> PowerSupplyConfigModes, int V27BusCyclogramRepeatCount, int V100BusCyclogramRepeatCount, int PowerSupplyCyclogramRepeatCount)
        {

            this.V100BusConfigurationModes = V100BusConfigurationModes;
            this.V27BusCyclogramRepeatCount = V27BusCyclogramRepeatCount;
            this.PowerSupplyConfigModes = PowerSupplyConfigModes;
            this.V27BusConfigurationModes = V27BusConfigurationModes;
            this.V100BusCyclogramRepeatCount = V100BusCyclogramRepeatCount;
            this.PowerSupplyCyclogramRepeatCount = PowerSupplyCyclogramRepeatCount;
        }
        public ObservableCollection<ElectronicConfigMode> V100BusConfigurationModes { get; set; }
        public ObservableCollection<ElectronicConfigMode> V27BusConfigurationModes { get; set; }

        public ObservableCollection<PowerSupplyConfigMode> PowerSupplyConfigModes { get; set; }
       public int V27BusCyclogramRepeatCount { get; set; }
       public int V100BusCyclogramRepeatCount { get; set; }
        public int PowerSupplyCyclogramRepeatCount { get; set; }
    }
}
