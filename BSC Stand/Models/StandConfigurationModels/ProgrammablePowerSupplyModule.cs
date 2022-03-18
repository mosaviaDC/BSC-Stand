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
        public float MaxAmperage { get; set; }
        public float MinAmperage { get; set; }
        public float MaxVoltage { get; set; }
        public float MinVoltage { get; set; }
        public ProgrammablePowerSupplyModule(string Name, float MinAmperage, float MaxAmperage,float MinVoltage,float MaxVoltage )
        {
            ModuleName = Name;
            this.MaxAmperage = MaxAmperage;
            this.MinAmperage = MinAmperage;
            this.MaxVoltage = MaxVoltage;
            this.MinVoltage = MinVoltage;
        }
    }
}
