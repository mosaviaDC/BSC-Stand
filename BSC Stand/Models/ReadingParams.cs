using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models
{
    internal class ReadingParams
    {
        public  float ITCAValue;
        public float ITCVValue;
        public float ITCWValue;

        public float AKIPWValue;
        public float AKIPAValue;
        public float AKIPVValue;

        public float V27Value;
        public float I27Value;

        public float V100Value;
        public float I100Value;

        //Параметры с Owen
        public float IBXATemperature;
        public float BSCTemperature;


    }
}
