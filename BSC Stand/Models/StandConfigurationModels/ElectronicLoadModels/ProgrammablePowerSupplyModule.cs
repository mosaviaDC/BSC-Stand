using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels
{
    internal class ProgrammablePowerSupplyModule
    {
        public List<ConfigurationMode> Modes { get; }
        public ConfigurationMode CurrentConfigMode { get; set; }
        public string ModuleName { get; }
        public ProgrammablePowerSupplyModule(string Name,List<ConfigurationMode> configurationModes)
        {
            this.ModuleName = Name;
            this.Modes = configurationModes;

        }
    }
}
