using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;
using BSC_Stand.Models.StandConfigurationModels;
using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;

namespace BSC_Stand.ViewModels
{
    class StandConfigurationViewModel:ViewModels.Base.ViewModelBase
    {

      
        #region Properties
       private ProgrammablePowerSupplyModule _AKIP1311;
       public ProgrammablePowerSupplyModule AKIP1311
        {
            get => _AKIP1311;
            set=>Set(ref _AKIP1311, value);
        }

        private ProgrammablePowerSupplyModule _AKIP1311_4;
        public ProgrammablePowerSupplyModule AKIP1311_4
        {
            get => _AKIP1311_4;
            set => Set(ref _AKIP1311_4, value);
        }
        public ObservableCollection<ProgrammablePowerSupplyModule> programmablePowerSupplyModules { get; set; }
        #endregion

        #region Commands



        #endregion

        #region Services
        #endregion Services

        public StandConfigurationViewModel()
        {
            List<ConfigurationMode> Akip1311_Config = new List<ConfigurationMode>()
            {
                 new ConfigurationMode()
            {
                ModeName = "Стабилизация напряжения",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 60,
                ModeUnit = "В"
            },
                 new ConfigurationMode()
            {
                ModeName = "Cтабилизация силы тока",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 25,
                ModeUnit = "A"
            },
                 new ConfigurationMode()
            {
                ModeName = "Cтабилизация мощности",
                Discreteness = 0.5f,
                MinValue = 60,
                MaxValue = 600,
                ModeUnit = "Вт"
                 }
            };
            List<ConfigurationMode> Akip1311_4Config = new List<ConfigurationMode>()
            {
                 new ConfigurationMode()
            {
                ModeName = "Стабилизация напряжения",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 60,
                ModeUnit = "В"
            },
                 new ConfigurationMode()
            {
                ModeName = "Cтабилизация силы тока",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 25,
                ModeUnit = "A"
            },
                 new ConfigurationMode()
            {
                ModeName = "Cтабилизация мощности",
                Discreteness = 0.5f,
                MinValue = 60,
                MaxValue = 600,
                ModeUnit = "Вт"
                 }
            };
            ProgrammablePowerSupplyModule _AKIP1311 = new ProgrammablePowerSupplyModule("Акип 1311",Akip1311_Config);
                ProgrammablePowerSupplyModule _AKIP1311_4 = new ProgrammablePowerSupplyModule("Акип 1311/4",Akip1311_4Config);
                programmablePowerSupplyModules = new ObservableCollection<ProgrammablePowerSupplyModule>();

                programmablePowerSupplyModules.Add(_AKIP1311);
                programmablePowerSupplyModules.Add(_AKIP1311_4);

                #region Commands

                #endregion

            
        }




    }
}
