using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models.StandConfigurationModels
{
    internal class ElectronicLoad:ProggramebleModule
    {
        public List<ElectronicConfigMode> Modes { get; }
        public ConfigMode CurrentConfigMode { get; set; }
        public string ModuleName { get; }
        public ElectronicLoad(string Name,List<ElectronicConfigMode> configurationModes)
        {
            this.ModuleName = Name;
            this.Modes = configurationModes;

        }
    }
}
