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
            if (value != -1)
            {
                return new string($"V {value}");
            }
            else
            {
                return new string($"V Нет соединения");
            }

        
        }
        public static string ToAmperageString(this float value)
        {
            if (value != -1)
            {
                return new string($"A {value}");
            }
            else
            {
                return new string($"A Нет соединения");
            }
         
        }
        public static string ToPowerString(this float value)
        {
            if (value != -1)
            {
                return new string($"W {value}");
            }
            else 
                return new string($"W Нет соединения");
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
