using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models.StandConfigurationModels
{
    /// <summary>
    /// Тетрон 15016С
    /// </summary>
    class ProgrammablePowerSupplyModule
    {   
        public string ModuleName { get;}
        public float Amperage { get; set; }
        public float Voltage { get; set; }
        public float Power { get; set; }
        public ProgrammablePowerSupplyModule(string Name="Тетрон 15016С")
        {
            ModuleName = Name;
        }
    }
}
