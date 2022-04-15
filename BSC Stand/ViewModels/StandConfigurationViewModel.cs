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
using BSC_Stand.Services;
using System.IO;

namespace BSC_Stand.ViewModels
{
    class StandConfigurationViewModel:ViewModels.Base.ViewModelBase
    {

      public ConfigurationMode SelectedConfigMode { get; set; }
      
     


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
        public ObservableCollection<ConfigurationMode> Bus27ConfigurationModes { get; set; }
        public ObservableCollection<ConfigurationMode> Bus100ConfigurationModes { get; set; }
        public ObservableCollection<ProgrammablePowerSupplyModule> programmablePowerSupplyModules { get; set; }

        private readonly StandVizualizationViewModel _standVizualizationViewModel;
        #endregion

        #region Commands
        public ICommand AddConfigToCyclogram { get; set; }

        public void AddConfigToCyclogramExecuted(object p)
        {
            var parametres = (object[])p;
            var programmablePowerSupplyModule = (ProgrammablePowerSupplyModule)parametres[0];
            var configMode = (ConfigurationMode)parametres[1];
           
            if (configMode != null && programmablePowerSupplyModule !=null)
            {
                Debug.WriteLine(configMode.MaxValue);
                ConfigurationMode configurationMode = new ConfigurationMode()
                {
                    Discreteness = configMode.Discreteness,
                    Duration = configMode.Duration,
                    MaxValue = configMode.MaxValue,
                    ModeName = configMode.ModeName,
                    MinValue = configMode.MinValue,
                    ModeUnit = configMode.ModeUnit,

                };
                if (programmablePowerSupplyModule.ModuleName == "Нагрузка электронная (шина 27В)")
                {
                     Bus27ConfigurationModes.Add(configurationMode);
                }
                else if    (programmablePowerSupplyModule.ModuleName == "Нагрузка электронная (шина 100В)")
                {
                    Bus100ConfigurationModes.Add(configurationMode);
                }
                UpdateCyclograms(null);
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
            UpdateCommand = new ActionCommand(UpdateCyclograms);
            #endregion
            #region Services
            #endregion

            SelectedConfigMode = new ConfigurationMode()
            {
                Duration = 15,
                MaxValue = 600,
                MinValue = 600
            };
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
              Bus27ConfigurationModes = new ObservableCollection<ConfigurationMode>();
              Bus100ConfigurationModes = new ObservableCollection<ConfigurationMode>();
             Bus27ConfigurationModes.CollectionChanged += Bus27ConfigurationModes_CollectionChanged;
              Bus100ConfigurationModes.CollectionChanged += Bus100ConfigurationModes_CollectionChanged;
                programmablePowerSupplyModules = new ObservableCollection<ProgrammablePowerSupplyModule>();
                programmablePowerSupplyModules.Add(_AKIP1311);
                programmablePowerSupplyModules.Add(_AKIP1311_4);
            //    programmablePowerSupplyModules.Add(_Tetron15016C);

        
        }

        private void Bus100ConfigurationModes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCyclograms(null);
        }
        private void Bus27ConfigurationModes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCyclograms(null);
        }

        private void UpdateCyclograms(object p )
        {
         //   Debug.WriteLine($"{Bus27ConfigurationModes.Count} {Bus100ConfigurationModes.Count}");
            _standVizualizationViewModel.Update27BusPlotModel(this.Bus27ConfigurationModes);
            _standVizualizationViewModel.Update100BusPlotModel(this.Bus100ConfigurationModes);


        }

        public void UpdateConfigurationModes(ObservableCollection<ConfigurationMode> V27BusConfig, ObservableCollection<ConfigurationMode> V100BusConfig)
        {
            this.Bus100ConfigurationModes.Clear();
            this.Bus27ConfigurationModes.Clear();
            
            foreach(var p in V27BusConfig)
            {
                Bus27ConfigurationModes.Add(p);
            }
            foreach (var p in V100BusConfig)
            {
                Bus100ConfigurationModes.Add(p);
            }



        }

    }
}
