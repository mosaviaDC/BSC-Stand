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

       //public ConfigurationMode 
      
     


        #region Properties
       private ProgrammablePowerSupplyModule _AKIP1311;
       public ProgrammablePowerSupplyModule AKIP1311
        {
            get => _AKIP1311;
            set
            {
                Debug.WriteLine("Set");
                Set(ref _AKIP1311, value);
            }
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
            set
            {
              
                Set(ref _AKIP1311_4, value);
            }
        }
        public ObservableCollection<ConfigurationMode> ConfigurationModes { get; set; }
        public ObservableCollection<ProgrammablePowerSupplyModule> programmablePowerSupplyModules { get; set; }

        private readonly StandVizualizationViewModel _standVizualizationViewModel;
        #endregion

        #region Commands
        public ICommand AddConfigToCyclogram { get; set; }

        public void AddConfigToCyclogramExecuted(object p)
        {
            var configMode = (ConfigurationMode)p;

            if (configMode != null)
            {
                ConfigurationMode configurationMode = new ConfigurationMode()
                {
                    Discreteness = configMode.Discreteness,
                    Duration = configMode.Duration,
                    MaxValue = configMode.MaxValue,
                    ModeName = configMode.ModeName,
                    MinValue = configMode.MinValue,
                    ModeUnit = configMode.ModeUnit,

                };

                ConfigurationModes.Add(configurationMode);
                Test(configMode);
            }

        }

        public bool CanAddConfigToCyclogramExecuted (object p)
        {
            return true;
        }

        public ICommand UpdateCommand { get; set; }

        #endregion

        #region Services
        #endregion Services

        public StandConfigurationViewModel(IFileLogger fileLogger,StandVizualizationViewModel standVizualizationViewModel)
        {
            _standVizualizationViewModel = standVizualizationViewModel;

            #region Commands
            AddConfigToCyclogram = new ActionCommand(AddConfigToCyclogramExecuted, CanAddConfigToCyclogramExecuted);
            UpdateCommand = new ActionCommand(Test);
            #endregion
            #region Services
            #endregion


            List<ConfigurationMode> Akip1311_Config = new List<ConfigurationMode>()
            {
            //     new ConfigurationMode()
            //{
            //    ModeName = "Стабилизация напряжения",
            //    Discreteness = 0.5f,
            //    MinValue = 0,
            //    MaxValue = 60,
            //    ModeUnit = "В"
            //},
            //     new ConfigurationMode()
            //{
            //    ModeName = "Cтабилизация силы тока",
            //    Discreteness = 0.5f,
            //    MinValue = 0,
            //    MaxValue = 25,
            //    ModeUnit = "A"
            //},
                 new ConfigurationMode()
            {
                ModeName = "Cтабилизация мощности",
                Discreteness = 0.5f,
                MinValue = 60,
                MaxValue = 600,
                ModeUnit = "Вт",
                Duration = 15
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
            //     new ConfigurationMode()
            //{
            //    ModeName = "Стабилизация напряжения",
            //    Discreteness = 0.5f,
            //    MinValue = 0,
            //    MaxValue = 60,
            //    ModeUnit = "В"
            //},
            //     new ConfigurationMode()
            //{
            //    ModeName = "Cтабилизация силы тока",
            //    Discreteness = 0.5f,
            //    MinValue = 0,
            //    MaxValue = 25,
            //    ModeUnit = "A"
            //},
                 new ConfigurationMode()
            {
                ModeName = "Cтабилизация мощности",
                Discreteness = 0.5f,
                MinValue = 60,
                MaxValue = 600,
                ModeUnit = "Вт",
                Duration = 15
                 }
            };
             _AKIP1311 = new ProgrammablePowerSupplyModule("Нагрузка электронная (шина 27В)",Akip1311_Config);
             _AKIP1311_4 = new ProgrammablePowerSupplyModule("Нагрузка электронная (шина 100В)",Akip1311_4Config);
                //     _Tetron15016C = new ProgrammablePowerSupplyModule("Источник питания", Tetron15016CConfig);
              ConfigurationModes = new ObservableCollection<ConfigurationMode>();
              ConfigurationModes.CollectionChanged += ConfigurationModes_CollectionChanged;
                programmablePowerSupplyModules = new ObservableCollection<ProgrammablePowerSupplyModule>();
                programmablePowerSupplyModules.Add(_AKIP1311);
                programmablePowerSupplyModules.Add(_AKIP1311_4);
            //    programmablePowerSupplyModules.Add(_Tetron15016C);

        
        }
        private void Test(object p)
        {
            try
            {
                var currentSelectedItem = (ConfigurationMode)p;
                Debug.WriteLine($"CurrentSelectedItem{currentSelectedItem?.MaxValue} ");
                
                _standVizualizationViewModel.UpdateAllPlot(this.ConfigurationModes);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void ConfigurationModes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Test(null);
        }
    }
}
