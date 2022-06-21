using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models.StandConfigurationModels
{
    internal class PowerSupply:ProggramebleModule
    {

        public List<PowerSupplyConfigMode> Modes { get; }
        public ConfigMode CurrentConfigMode { get; set; }
        public string ModuleName { get; }
        public PowerSupply(string Name, List<PowerSupplyConfigMode> configurationModes)
        {
            this.ModuleName = Name;
            this.Modes = configurationModes;

        }




    }
}
