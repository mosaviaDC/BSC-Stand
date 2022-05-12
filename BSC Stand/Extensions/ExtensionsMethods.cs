using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Extensions
{
    public static class ExtensionsMethods
    {

        public static string ToVoltageString(this  float value)
        {
            return new string($"V {value}");
        }

    }
}
