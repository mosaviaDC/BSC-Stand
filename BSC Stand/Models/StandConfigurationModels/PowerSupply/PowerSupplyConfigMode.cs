using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models.StandConfigurationModels
{
    internal class PowerSupplyConfigMode:ConfigMode
    {
        public float MaxValue1 { get; set; }

        public float Power {
            get => MaxValue1 * MaxValue;
        }

    }
}
