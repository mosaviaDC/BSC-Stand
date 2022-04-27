using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models
{
    class CommandParams
    {
        public CommandParams (ConfigurationMode configurationMode, int index, bool LastCommand)
        {
            this.configurationMode = configurationMode;
            this.SelectedIndex = index;
            this.LastCommand = LastCommand;
        }

        public ConfigurationMode configurationMode;
        public int SelectedIndex;
        public bool LastCommand;
    }
}
