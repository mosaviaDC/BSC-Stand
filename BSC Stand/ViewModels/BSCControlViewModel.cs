using BSC_Stand.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSC_Stand.Services;
using System.Diagnostics;
using System.Collections.ObjectModel;
using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
using System.Windows.Threading;
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;
using System.IO;
using BSC_Stand.Models;
using BSC_Stand.Extensions;
namespace BSC_Stand.ViewModels
{
    internal class BSCControlViewModel:ViewModelBase
    {
        private readonly IUserDialogWindowService _userDialogWindowService;
        #region services
        private StandConfigurationViewModel _standConfigurationViewModel;
        private RealTimeStandControlService _realTimeStandControlService;
        private IModbusService _modBusService;
        private static DispatcherTimer UpdateDataTimer;
        #endregion

        #region Commands
        public ICommand StartExpirementCommand { get; set; } 

        private async void StartExpirementCommandExecute (object p)
        {
            if (_realTimeStandControlService.GetExperimentStatus())
            {
                _userDialogWindowService.ShowErrorMessage("Эксперимент уже активен");
                return;

            }
            if (!_modBusService.GetConnectStatus())
            {
                _userDialogWindowService.ShowErrorMessage("Ошибка связи");
                return;
            }

            if (V27ConfigurationModes.Count == 0 || V100ConfigurationModes.Count == 0)
            {
                _userDialogWindowService.ShowErrorMessage("Выбрана пустая конфигурация");
            }
            else
            {
                WriteMessage("Начало эксперимента", MessageType.Info);
                StartTime = DateTime.Now;
                _realTimeStandControlService.StartExpirent();
                if (V27Series.Points.Count != 0)
                {
                    V27Series.Points.Clear();
                    GraphView.InvalidatePlot(true);
                }
                UpdateDataTimer?.Start();
            }

            
        }


        private bool CanStartExpirementCommandExecuted(object p)
        {
            return true;

        }


        public ICommand CheckConnectionStatusCommand { get; set; }

        private async void CheckConnectionStatusCommandExecute(object p)
        {
            UpdateDataTimer.Stop();

            (string, bool) result = ("", false);
            if (_modBusService.GetBusyStatus())
            {
                _userDialogWindowService.ShowErrorMessage("Операция уже выполняется");
            }
            else
            {
                WriteMessage("Проверка подключения", MessageType.Info);
              
                if (!UpdateDataTimer.IsEnabled)
                {
                
                     await  Task.Factory.StartNew(() =>
                    {
                      result=  _modBusService.InitConnections();

                    });

                    
                    if (!result.Item2)
                    {
                        WriteMessage(result.Item1, MessageType.Warning);
                        WriteMessage("Ошибка при проверке подключения", MessageType.Warning);


                        UpdateDataTimer.Start();
                    }
                    else
                    {
                       WriteMessage("Проверка подключения завершена успешно", MessageType.Info);
                       UpdateDataTimer.Start();
                    }
                }
            }
          
        }

     

        public ICommand StopExpirementCommand { get; set; }

        private void StopExpirementCommandExecute(object p)
        {
            if (!_realTimeStandControlService.GetExperimentStatus())
            {
                _userDialogWindowService.ShowErrorMessage("Нет активного эксперимента");
                return;

            }
            WriteMessage("Эксперимент остановлен", MessageType.Info);
            _realTimeStandControlService.StopExpirement();
        }


        private bool CanStopExpirementCommandExecuted(object p)
        {
            return true;

        }

        public ICommand ResetPlotScaleCommand { get; set; }

        private void ResetPlotScaleCommandExecute (object p)
        {
            GraphView.ResetAllAxes();
            GraphView.InvalidatePlot(true);

        }
        private bool CanResetPlotScaleCommandExecute (object p)
        {
            return true;
        }


        public ICommand ShowHideOxyPlotLegendCommand { get; set; }

        public void ShowHideOxyPlotLegendCommandExecute(object p )
        {
            GraphView.IsLegendVisible = !GraphView.IsLegendVisible;
            sCPIService.Write (@"SYSTEM:REM 
            Mode:Power
            Power 5");
        }



        private int index;

        #endregion


        #region properties
        public PlotModel GraphView { get; set; }

        private LineSeries V27Series;
        private LineSeries I27Series;
        private LineSeries V100Series;
        private LineSeries I100Series;
        private LineSeries TIBXASeries;
        private LineSeries TBSCSeries;
        private DateTime StartTime;
        private readonly SCPIService sCPIService;
        private bool _V100SeriesVisible;
        public bool V100SeriesVisible
        {
            get
            {
                return _V100SeriesVisible;
            }
            set
            {
                Set(ref _V100SeriesVisible, value);
                V100Series.IsVisible = value;
            }
        }

        private bool _I100SeriesVisible;
        public bool I100SeriesVisible
        {
            get
            {
                return _I100SeriesVisible;
            }
            set
            {
                Set(ref _I100SeriesVisible, value);
                I100Series.IsVisible = value;
            }
        }

        private bool _I27SeriesVisible;
        public bool I27SeriesVisible
        {
            get
            {
                return _I27SeriesVisible;
            }
            set
            {
                Set(ref _I27SeriesVisible, value);
                I27Series.IsVisible = value;
            }
        }


        private bool _V27SeriesVisible;
        public bool V27SeriesVisible
        {
            get
            {
                return _V27SeriesVisible;
            }
            set
            {
                Set(ref _V27SeriesVisible, value);
                V27Series.IsVisible= value;
            }
        }


        private bool _TIBXASeriesVisible;
        public bool TIBXASeriesVisible
        {
            get
            {
                return _TIBXASeriesVisible;
            }
            set
            {
                Set(ref _TIBXASeriesVisible, value);
                TIBXASeries.IsVisible = value;
            }
        }

        private bool _TBSCSeriesVisible;
        public bool TBSCSeriesVisible
        {
            get
            {
                return _TBSCSeriesVisible;
            }
            set { 
                
                Set(ref _TBSCSeriesVisible, value);
                TBSCSeries.IsVisible = value;
            }
        }







        #region InfoStringProperties
        public string DebugString
        {
            get => _DebugString;
            set => Set(ref _DebugString, value);
        }

        private string _DebugString;


        private float _OwenTemperature;

        public float OwenTemperature
        {
            get => _OwenTemperature;
            set => Set(ref _OwenTemperature, value);
        }

        private int _V27SelectedIndex;
        public int V27SelectedIndex 
        {
            get => _V27SelectedIndex;
            set => Set(ref _V27SelectedIndex, value);
        }

        private int _V100SelectedIndex;
        public int V100SelectedIndex
        {
            get => _V100SelectedIndex;
            set => Set(ref _V100SelectedIndex, value);
        }


        private string  _OwenConnectStatus;

        public string OwenConnectStatus
        {
            get => _OwenConnectStatus;
            set => Set(ref _OwenConnectStatus, value);
        }

        private string _V27Value;
        public string V27Value
        {
            get => _V27Value;
            set=> Set(ref _V27Value, value);

        }
        private string _I27Value;
       

        public string I27Value
        {
            get=> _I27Value;
            set => Set(ref _I27Value, value);

        }


        public ObservableCollection<ConfigurationMode> V27ConfigurationModes { get; set; }
        public ObservableCollection<ConfigurationMode> V100ConfigurationModes { get; set; }

        #endregion

        #endregion


        public BSCControlViewModel(StandConfigurationViewModel standConfigurationViewModel, IModbusService modbusService, IUserDialogWindowService userDialogWindowService)
        {
      
            _userDialogWindowService = userDialogWindowService;
            _standConfigurationViewModel = standConfigurationViewModel;
            V27ConfigurationModes = standConfigurationViewModel.Bus27ConfigurationModes;
            V100ConfigurationModes = standConfigurationViewModel.Bus100ConfigurationModes;
            _realTimeStandControlService = new RealTimeStandControlService(this, _standConfigurationViewModel, _userDialogWindowService);
            _modBusService = modbusService;
            InitSeries();
            UpdateDataTimer = new DispatcherTimer();
            UpdateDataTimer.Interval = TimeSpan.FromMilliseconds(100);
            UpdateDataTimer.Tick += UpdateDataTimer_Tick;
            StartTime = DateTime.Now;

            sCPIService = new SCPIService();
            sCPIService.Init();
            #region registerCommands
            StartExpirementCommand = new ActionCommand(StartExpirementCommandExecute, CanStartExpirementCommandExecuted);
            StopExpirementCommand = new ActionCommand(StopExpirementCommandExecute, CanStopExpirementCommandExecuted);
            ResetPlotScaleCommand = new ActionCommand(ResetPlotScaleCommandExecute, CanResetPlotScaleCommandExecute);
            CheckConnectionStatusCommand = new ActionCommand(CheckConnectionStatusCommandExecute);
            ShowHideOxyPlotLegendCommand = new ActionCommand(ShowHideOxyPlotLegendCommandExecute);
            V27Value = "V Нет соединения";
            I27Value = "I Нет соединения";
            OwenConnectStatus = "Нет соединения";
            if  (DateTime.Now - Properties.Settings.Default.LastCheckDateTime > TimeSpan.FromDays(1)) // 
            {
           
                WriteMessage("Необходимо выполнить периодическую проверку оборудования", MessageType.Warning);
            }

            Task.Factory.StartNew(() =>
            {
                CheckConnectionStatusCommandExecute(null);
            });


            #endregion
        }



        private void InitSeries()
        {
            index = 0;
            GraphView = new PlotModel()
            {

            };


            V27Series = new LineSeries
            {
                Title = "V 27",
                TrackerFormatString = "{4:0} В {2:0} сек",
                Color = OxyColors.Black,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = false
            };



            I27Series = new LineSeries
            {
                Title = "I 27",
                TrackerFormatString = "{4:0} A {2:0} сек",
                Color = OxyColors.Green,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = false

            };


            V100Series = new LineSeries
            {
                Title = "V 100",
                TrackerFormatString = "{4:0} В {2:0} сек",
                Color = OxyColors.GreenYellow,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = false
            };



            I100Series = new LineSeries
            {
                Title = "I 100",
                TrackerFormatString = "{4:0} A {2:0} сек",
                Color = OxyColors.OrangeRed,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = false
            };


            GraphView.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",

                LegendTitleFontSize = 48,
                



            }) ;


            TIBXASeries = new LineSeries
            {
                Title = "T℃  ИБХА",
                TrackerFormatString = "{4:0} T℃  {2:0} сек",
                Color = OxyColors.Green,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = false,
            };

            TBSCSeries = new LineSeries
            {
                Title = "T℃  ЭОБСК",
                TrackerFormatString = "{4:0} T℃  {2:0} сек",
                Color = OxyColors.Green,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = false
            };


            GraphView.Series.Add(I27Series);
            GraphView.Series.Add(V27Series);
            GraphView.Series.Add(TIBXASeries);
            GraphView.Series.Add(TBSCSeries);
            GraphView.Series.Add(V100Series);
            GraphView.Series.Add(I100Series);
            TIBXASeriesVisible = true;
            TBSCSeriesVisible = true;

        }










        private async void UpdateDataTimer_Tick(object? sender, EventArgs e)
        {
            //To Do добавить сервис записи данных в файл(лог)






            //ReadV27Value();

            //OwenConnectStatus = false.ToConnectionStatusString();

            TestUpdateData();
        }

        private async void TestUpdateData()
        {

            index++;
            Random rnd = new Random();
           
            I27Series.Points.Add(new DataPoint(index, rnd.Next(26, 28)));
            V27Series.Points.Add( new DataPoint(index, rnd.Next(23, 24)));
            I100Series.Points.Add(new DataPoint(index, 0));
            V100Series.Points.Add(new DataPoint(index, 3f));
            TIBXASeries.Points.Add(new DataPoint(index, 6f));
            TBSCSeries.Points.Add(new DataPoint(index, 12f));
            GraphView.InvalidatePlot(true);

          




        }



        private async void ReadV27Value()
        {
            
            var r = await _modBusService.Read27BusVoltage();
            var x = DateTime.Now - StartTime;
            if (r == -1) //Если нет подключения к устройству
                {
   
                     V27Value = "V Нет соединения";
                    if (_realTimeStandControlService.GetExperimentStatus())
                        WriteMessage("Потеряно соединение с Е856ЭЛ", MessageType.Warning);
                    return;
                }
                else
                {
                    V27Value = r.ToVoltageString();
                    if (_realTimeStandControlService.GetExperimentStatus())
                    {
                     
                        V27Series.Points.Add(new DataPoint(x.TotalSeconds, r));
                        GraphView.PlotView.InvalidatePlot(true);
                    }
                }
        }


        public void WriteMessage(string Message, MessageType messageType)
        {
            DebugString += $"[{messageType.ToString()}] {Message} {DateTime.Now}\n";
        }

        public void SendV27ModBusCommand(CommandParams commandParams)
        {
            if (commandParams.LastCommand)
            {
                StopExpirementCommandExecute(null);
                return;
            }
            V27SelectedIndex = commandParams.SelectedIndex;
            WriteMessage($"Отправлена команда на шину 27B: стабилизация мощности {commandParams.configurationMode.MaxValue}Вт", MessageType.Info);
        }
        public void SendV100ModBusCommand(CommandParams commandParams)
        {
            if (commandParams.LastCommand)
            {
                StopExpirementCommandExecute(null);
                return;
            }
            V100SelectedIndex = commandParams.SelectedIndex;
            WriteMessage($"Отправлена команда на шину 100B: стабилизация мощности {commandParams.configurationMode.MaxValue}Вт", MessageType.Info);
        }
      
    }
}
