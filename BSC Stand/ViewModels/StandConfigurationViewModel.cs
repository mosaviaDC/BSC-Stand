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
using BSC_Stand.Models.StandConfigurationModels;
using Microsoft.Extensions.Logging;
using BSC_Stand.Services;
using System.IO;

namespace BSC_Stand.ViewModels
{
    class StandConfigurationViewModel:ViewModels.Base.ViewModelBase
    {




      

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
        private ElectronicLoad _AKIP1311;
       public ElectronicLoad AKIP1311
        {
            get => _AKIP1311;
            set
            {
                Debug.WriteLine("Set");
                Set(ref _AKIP1311, value);
            }
        }

        private PowerSupply _Tetron15016C;
        public PowerSupply Tetron15016C
        {
            get => _Tetron15016C;
            set => Set(ref _Tetron15016C, value);
        }


        private ElectronicLoad _AKIP1311_4;
        public ElectronicLoad AKIP1311_4
        {
            get => _AKIP1311_4;
            set
            {
              
                Set(ref _AKIP1311_4, value);
            }
        }
        public ObservableCollection<ConfigMode> Bus27ConfigurationModes { get; set; }
        public ObservableCollection<ConfigMode> Bus100ConfigurationModes { get; set; }
        public ObservableCollection<ProggramebleModule> programmablePowerSupplyModules { get; set; }

        private readonly StandVizualizationViewModel _standVizualizationViewModel;

        #endregion

        #region Commands
        public ICommand AddConfigToCyclogram { get; set; }

        public void AddConfigToCyclogramExecuted(object p)
        {
            var parametres = (object[])p;
            var programmablePowerSupplyModule = (ElectronicLoad)parametres[0];
            var ElectronicConfigMode = (ElectronicConfigMode)parametres[1];
            Debug.WriteLine(ElectronicConfigMode.ModeName + programmablePowerSupplyModule.ModuleName);
            if (ElectronicConfigMode != null && programmablePowerSupplyModule !=null)
            {
                
                ConfigMode configurationMode = new ElectronicConfigMode()
                {
                    Discreteness = ElectronicConfigMode.Discreteness,
                    Duration = ElectronicConfigMode.Duration,
                    MaxValue = ElectronicConfigMode.MaxValue,
                    ModeName = ElectronicConfigMode.ModeName,
                    MinValue = ElectronicConfigMode.MinValue,
                    ModeUnit = ElectronicConfigMode.ModeUnit,
                };
               
                if (programmablePowerSupplyModule.ModuleName == "Нагрузка электронная (шина 27В)")
                {
                     Bus27ConfigurationModes.Add(configurationMode);
                }
                else if    (programmablePowerSupplyModule.ModuleName == "Нагрузка электронная (шина 100В)")
                {
                    Bus100ConfigurationModes.Add(configurationMode);
                }
                else if (programmablePowerSupplyModule.ModuleName == "Источник питания")
                {
                    Debug.WriteLine(ElectronicConfigMode.ModeName);


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

            
         //   Debug.WriteLine("UndoCommand");
         ////   var lastConfig = V27BusConfigurationStack.Pop();
         //   this.Bus27ConfigurationModes = lastConfig;
            
         //   this.Bus27ConfigurationModes.CollectionChanged += Bus27ConfigurationModes_CollectionChanged;
        

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
            return true;
            
        }



        public ICommand UpdateCommand { get; set; }

        #endregion

        #region Services
        #endregion Services

        public StandConfigurationViewModel(StandVizualizationViewModel standVizualizationViewModel)
        {
            _standVizualizationViewModel = standVizualizationViewModel;
          
            #region Commands
            AddConfigToCyclogram = new ActionCommand(AddConfigToCyclogramExecuted, CanAddConfigToCyclogramExecuted);
            UpdateCommand = new ActionCommand(UpdateCyclograms);
            UndoDataGridCommand = new ActionCommand(UndoDataGridCommandExecuted, CanUndoDataGridCommandExecuted);
            #endregion
            #region Services
            #endregion

           
            List<ElectronicConfigMode> Akip1311_Config = new List<ElectronicConfigMode>()
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
                 new ElectronicConfigMode()
                {
                ModeName = "Cтабилизация мощности",
                Discreteness = 0.5f,
                MinValue = 60,
                MaxValue = 600,
                ModeUnit = "Вт",
                Duration = 15
                 }
            };
            List<PowerSupplyConfigMode> Tetron15016CConfig = new List<PowerSupplyConfigMode>()
            {
                 new PowerSupplyConfigMode()
            {
                ModeName = "Ограничение по силе тока",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 16,
                ModeUnit = "А",
                Test =0,
            },
               new PowerSupplyConfigMode()
            {
                ModeName = "Ограничение по напряжению",
                Discreteness = 0.5f,
                MinValue = 0,
                MaxValue = 160,
                ModeUnit = "В",
                Test =1,
            },
             new PowerSupplyConfigMode()
            {
                ModeName = "Удержание напряжения",
                Discreteness = 0.5f,
                MinValue = 150,
                MaxValue = 160,
                ModeUnit = "В",
                Test =2
            },
                new PowerSupplyConfigMode()
            {
                ModeName = "Удержание силы тока",
                Discreteness = 0.5f,
                MinValue = 10,
                MaxValue = 11,
                ModeUnit = "А",
                Test =3
            },


            };
            List<ElectronicConfigMode> Akip1311_4Config = new List<ElectronicConfigMode>()
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
                 new ElectronicConfigMode()
            {
                ModeName = "Cтабилизация мощности",
                Discreteness = 0.5f,
                MinValue = 60,
                MaxValue = 600,
                ModeUnit = "Вт",
                Duration = 15
                 }
            };
             _AKIP1311 = new ElectronicLoad("Нагрузка электронная (шина 27В)",Akip1311_Config);
             _AKIP1311_4 = new ElectronicLoad("Нагрузка электронная (шина 100В)",Akip1311_4Config);
            _Tetron15016C = new PowerSupply("Источник питания", Tetron15016CConfig);
            V27BusCyclogramRepeatCount = 1;
            V100BusCyclogramRepeatCount = 1;

            Bus27ConfigurationModes = new ObservableCollection<ConfigMode>();
            Bus100ConfigurationModes = new ObservableCollection<ConfigMode>();
            Bus27ConfigurationModes.CollectionChanged += Bus27ConfigurationModes_CollectionChanged;
            Bus100ConfigurationModes.CollectionChanged += Bus100ConfigurationModes_CollectionChanged;
            programmablePowerSupplyModules = new ObservableCollection<ProggramebleModule>();
            programmablePowerSupplyModules.Add(_AKIP1311);
            programmablePowerSupplyModules.Add(_AKIP1311_4);
            programmablePowerSupplyModules.Add(_Tetron15016C);

        
        }

        private void Bus100ConfigurationModes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           // UpdateCyclograms(null);
            
        }
        private void Bus27ConfigurationModes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
               
          
        }

        private void UpdateCyclograms(object p )
        {
            _standVizualizationViewModel.Update27BusPlotModel(this.Bus27ConfigurationModes,V27BusCyclogramRepeatCount);
            _standVizualizationViewModel.Update100BusPlotModel(this.Bus100ConfigurationModes,V100BusCyclogramRepeatCount);
        }

        public  void UpdateConfigurationModes(ObservableCollection<ElectronicConfigMode> V27BusConfig, ObservableCollection<ElectronicConfigMode> V100BusConfig, int V27BusRepeatCount, int V100BusRepeatCount, int V27RepeatCount =1, int V100RepeatCount =1)
        {
            
                this.Bus100ConfigurationModes.Clear();
                this.Bus27ConfigurationModes.Clear();

                V27BusCyclogramRepeatCount = V27BusRepeatCount;
                V100BusCyclogramRepeatCount = V100BusRepeatCount;

                foreach (var p in V100BusConfig)
            {
                this.Bus100ConfigurationModes.Add(p);
            }
                foreach(var p in V27BusConfig)
            {
                this.Bus27ConfigurationModes.Add(p);
            }

              
                UpdateCyclograms(null);


            

 
        }

    }
}
