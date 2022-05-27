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
        public static string ToAmperageString(this float value)
        {
            return new string($"A {value}");
        }

        public static string ToConnectionStatusString (this bool value)
        {
            
            if (value)
            {
                return new string ("Соединение установлено");
            }
            else
            {
                return new string("Нет  соединения");
            }
            
        }

    }
}
