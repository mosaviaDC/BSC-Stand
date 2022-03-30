using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels
{
    internal class ConfigurationMode
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float Discreteness { get; set; }
        public string? ModeName { get; set; }

        public int Duration { get; set; }
        /// <summary>
        /// Единицы измерения
        /// </summary>
        public string? ModeUnit { get; set; }
    }
}
