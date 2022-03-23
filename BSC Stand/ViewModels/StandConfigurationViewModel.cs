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
using Microsoft.Extensions.Logging;
using BSC_Stand.Services.FileLoggingService;
using System.IO;

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

        private ProgrammablePowerSupplyModule _Tetron15016C;
        public ProgrammablePowerSupplyModule Tetron15016C
        {
            get => _Tetron15016C;
            set => Set(ref _Tetron15016C, value);
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
        public ICommand MouseWheelHandleCommand { get; }
        private void MouseWheelHandleCommandExecute(object p)
        {
            Debug.WriteLine(p.ToString());
        }
        private bool CanMouseWheelHandleCommandExecuted(object p)
        {
            return true;
        }


        #endregion

        #region Services
        #endregion Services

        public StandConfigurationViewModel(IFileLogger fileLogger)
        {
  
  
            #region Commands
            MouseWheelHandleCommand = new ActionCommand(MouseWheelHandleCommandExecute, CanMouseWheelHandleCommandExecuted);
            #endregion
            #region Services
            #endregion


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
            List<ConfigurationMode> Tetron15016CConfig = new List<ConfigurationMode>()
            {
                 new ConfigurationMode()
            {
                ModeName = "Ограничение по силе тока",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 16,
                ModeUnit = "А"
            },
               new ConfigurationMode()
            {
                ModeName = "Ограничение по напряжению",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 160,
                ModeUnit = "В"
            },
             new ConfigurationMode()
            {
                ModeName = "Удержание напряжения",
                Discreteness = 0.5f,
                MinValue = 150,
                MaxValue = 160,
                ModeUnit = "В"
            },
                new ConfigurationMode()
            {
                ModeName = "Удержание силы тока",
                Discreteness = 0.5f,
                MinValue = 10,
                MaxValue = 11,
                ModeUnit = "А"
            },


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
            _Tetron15016C = new ProgrammablePowerSupplyModule("Тетрон15016С", Tetron15016CConfig);
                programmablePowerSupplyModules = new ObservableCollection<ProgrammablePowerSupplyModule>();
                programmablePowerSupplyModules.Add(_AKIP1311);
                programmablePowerSupplyModules.Add(_AKIP1311_4);
                programmablePowerSupplyModules.Add(_Tetron15016C);

         

            
        }




    }
}
