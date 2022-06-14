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

        public static string ToIBXATemperatureString (this float value)
        {
            
            if (value !=-1)
            {
                return new string ($"Температура ИБХА {value} ℃");
            }
            else
            {
                return new string("Температура ИБХА - Нет данных");
            }
            
        }

        public static string ToBSCTemperatureString(this float value)
        {

            if (value != -1)
            {
                return new string($"Температура ЭО БСК {value} ℃");
            }
            else
            {
                return new string("Температура ЭО БСК - Нет данных");
            }

        }

        //IBXATemperature = "Температура ИБХА - Нет данных";
        //    BSCTemperature = "Температура ЭО БСК - Нет данных";

    }
}
