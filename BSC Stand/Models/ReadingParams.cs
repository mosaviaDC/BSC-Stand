using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models
{
    public class ReadingParams
    {
        public float ITCAValue { get; set; }
        public float ITCVValue { get; set; }
        public float ITCWValue { get; set; }

        public float AKIPWValue { get; set; }
        public float AKIPAValue { get; set; }
        public float AKIPVValue { get; set; }

        public float V27Value { get; set; }
        public float I27Value { get; set; }

        public float V100Value { get; set; }
        public float I100Value { get; set; }

        //Параметры с Owen
        public float IBXATemperature { get; set; }
        public float BSCTemperature { get; set; }

        public float ExpTime { get; set; }


    }
}
