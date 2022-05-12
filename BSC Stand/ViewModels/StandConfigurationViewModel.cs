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


        private Stack<ObservableCollection<ConfigurationMode>> V27BusConfigurationStack = new Stack<ObservableCollection<ConfigurationMode>>();

        private bool UndoRedoCommandExecuted = false;
        private int _V27BusCyclogramRepeatCount;
        private string currentOpenedFilePath;

        public int V27BusCyclogramRepeatCount
        {
            get => _V27BusCyclogramRepeatCount;
            set=> Set(ref _V27BusCyclogramRepeatCount, value);
        }

        private int _V100BusCyclogramRepeatCount;

        public int V100BusCyclogramRepeatCount
        {
            get => _V100BusCyclogramRepeatCount;
            set => Set(ref _V100BusCyclogramRepeatCount, value);
        }



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
            Debug.WriteLine(configMode.ModeName);
            if (configMode != null && programmablePowerSupplyModule !=null)
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

        public ICommand UndoDataGridCommand { get; set; }

        public void UndoDataGridCommandExecuted(object p)
        {

            
            Debug.WriteLine("UndoCommand");
            var lastConfig = V27BusConfigurationStack.Pop();
            this.Bus27ConfigurationModes = lastConfig;
            
            this.Bus27ConfigurationModes.CollectionChanged += Bus27ConfigurationModes_CollectionChanged;
        

            ////this.Bus27ConfigurationModes.CollectionChanged -= Bus27ConfigurationModes_CollectionChanged;
            ////this.Bus27ConfigurationModes.Clear();
            ////this.Bus27ConfigurationModes.CollectionChanged += Bus27ConfigurationModes_CollectionChanged;
            //////var lastConfig = V27BusConfigurationStack.Pop();
            ////Debug.WriteLine("Undo" + lastConfig.Count);
            //foreach (var r in lastConfig)
            //{
            //    Debug.WriteLine(r.MaxValue);
            //    this.Bus27ConfigurationModes.Add(r);
            //}




            UpdateCyclograms(null);

        }

        public bool CanUndoDataGridCommandExecuted(object p)
        {
            if (V27BusConfigurationStack.Count > 0)
            {
                return true;
            }
            else return false;
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
            UndoDataGridCommand = new ActionCommand(UndoDataGridCommandExecuted, CanUndoDataGridCommandExecuted);
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
            V27BusCyclogramRepeatCount = 1;
            V100BusCyclogramRepeatCount = 1;
           
            Bus27ConfigurationModes = new ObservableCollection<ConfigurationMode>();
            Bus100ConfigurationModes = new ObservableCollection<ConfigurationMode>();
            Bus27ConfigurationModes.CollectionChanged += Bus27ConfigurationModes_CollectionChanged;
            Bus100ConfigurationModes.CollectionChanged += Bus100ConfigurationModes_CollectionChanged;
            programmablePowerSupplyModules = new ObservableCollection<ProgrammablePowerSupplyModule>();
            programmablePowerSupplyModules.Add(_AKIP1311);
            programmablePowerSupplyModules.Add(_AKIP1311_4);


        
        }

        private void Bus100ConfigurationModes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           // UpdateCyclograms(null);
            
        }
        private void Bus27ConfigurationModes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
                Debug.WriteLine("Collect Cnaged");
                V27BusConfigurationStack.Push(this.Bus27ConfigurationModes);
          
        }

        private void UpdateCyclograms(object p )
        {
            _standVizualizationViewModel.Update27BusPlotModel(this.Bus27ConfigurationModes);
            _standVizualizationViewModel.Update100BusPlotModel(this.Bus100ConfigurationModes);
        }

        public  void UpdateConfigurationModes(ObservableCollection<ConfigurationMode> V27BusConfig, ObservableCollection<ConfigurationMode> V100BusConfig, int V27BusRepeatCount, int V100BusRepeatCount)
        {
            
                this.Bus100ConfigurationModes.Clear();
                this.Bus27ConfigurationModes.Clear();

                V27BusCyclogramRepeatCount = V27BusRepeatCount;
                V100BusCyclogramRepeatCount = V100BusRepeatCount;

                foreach (var p in V27BusConfig)
                {
                    Bus27ConfigurationModes.Add(p);
                }
                foreach (var p in V100BusConfig)
                {
                    Bus100ConfigurationModes.Add(p);
                }
                UpdateCyclograms(null);


            

 
        }

    }
}
