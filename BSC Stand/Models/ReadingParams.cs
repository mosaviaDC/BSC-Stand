using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models
{
    public class ReadingParams
    {
        [Name ("Сила тока ITC8156C+")]
        public float ITCAValue { get; set; }
        [Name("Напряжение ITC8156C+")]
        public float ITCVValue { get; set; }
        [Name("Мощность ITC8156C+")]
        public float ITCWValue { get; set; }


        [Name("Сила тока АКИП")]
        public float AKIPAValue { get; set; }

        [Name("Напряжение")]
        public float AKIPVValue { get; set; }

        [Name("Мощность АКИП")]
        public float AKIPWValue { get; set; }


        [Name("Напряжение шины 27В")]
        public float V27Value { get; set; }

        [Name("Сила тока 27В")]
        public float I27Value { get; set; }


        [Name("Напряжение шины 100В")]
        public float V100Value { get; set; }

        [Name("Сила тока на шине 100В")]
        public float I100Value { get; set; }

        [Name("Температура ИБХА")]
        public float IBXATemperature { get; set; }

        [Name("Температура ЭО БСК")]
        public float BSCTemperature { get; set; }

        [Name("Напряжение источника питания")]
        public float TetronVValue { get; set; }

        [Name("Cила тока источника питания")]
        public float TetronAValue { get; set; }
        [Name("Мощность источника питания")]
        public float TetronWValue { get; set; }

        [Name("Секунда эксперимента")]
        public float ExpTime { get; set; }

        [Name("Время фиксации значения(Unix TimeStamp UTC +3)")]
        public long TimeStamp { get; set; }


    }
}
