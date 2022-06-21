using BSC_Stand.Models.StandConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models
{
    class CommandParams
    {
        public CommandParams (ConfigMode configurationMode, int index, bool LastCommand)
        {
            this.configurationMode = configurationMode;
            this.SelectedIndex = index;
            this.LastCommand = LastCommand;
        }

        public ConfigMode configurationMode;
        public int SelectedIndex;
        public bool LastCommand;
    }
}
