using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Models.StandConfigurationModels
{
    public abstract class ConfigMode
    {
        [DisplayName("Column Name 1")]
        public float MinValue { get; set; }
        [DisplayName("Column Name 2")]
        public float MaxValue { get; set; }

        [DisplayName("Column Name 3")]
        public float Discreteness { get; set; }

        [DisplayName("Column Name 4")]
        public string ModeName { get; set; }

        public double Duration
        {
            get;
            set;
        }
        /// <summary>
        /// Единицы измерения
        /// </summary>
        public string ModeUnit { get; set; }

        public object Current
        {
            get
            {
                return this;
            }
          
        }

    
    }
}
