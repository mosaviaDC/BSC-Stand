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
                    I27Series.Points.Clear();
                    TIBXASeries.Points.Clear();
                    TBSCSeries.Points.Clear();
                    I100Series.Points.Clear();
                    V100Series.Points.Clear();
                    AKIPWSeries.Points.Clear();
                    AKIPVSeries.Points.Clear();
                    AKIPASeries.Points.Clear();
                    ITCWSeries.Points.Clear();
                    ITCVSeries.Points.Clear();
                    ITCASeries.Points.Clear();
                    GraphView1.InvalidatePlot(true);
                    GraphView2.InvalidatePlot(true);
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
            if (!_realTimeStandControlService.GetExperimentStatus())
            {

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

                        await Task.Factory.StartNew(() =>
                      {
                          result = _modBusService.InitConnections();

                      });


                        if (!result.Item2)
                        {
                            WriteMessage(result.Item1, MessageType.Warning);
                            WriteMessage("Ошибка при проверке подключения", MessageType.Warning);


                            //UpdateDataTimer.Start();
                        }
                        else
                        {
                            WriteMessage("Проверка подключения завершена успешно", MessageType.Info);

                        }
                    }
                }
            }
            else
            {
                _userDialogWindowService.ShowErrorMessage("Для проведения проверки, эксперимент должен быть остановлен");
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
            _modBusService.ExitCommand();
            _realTimeStandControlService.StopExpirement();

            UpdateDataTimer.Stop();
        }


        private bool CanStopExpirementCommandExecuted(object p)
        {
            return true;

        }

        public ICommand ResetPlotScaleCommand { get; set; }

        private void ResetPlotScaleCommandExecute (object p)
        {
            GraphView1.ResetAllAxes();
            GraphView1.InvalidatePlot(true);
            GraphView2.ResetAllAxes();
            GraphView2.InvalidatePlot(true);

        }
        private bool CanResetPlotScaleCommandExecute (object p)
        {
            return true;
        }


        public ICommand ShowHideOxyPlotLegendCommand { get; set; }

        public void ShowHideOxyPlotLegendCommandExecute(object p )
        {
            GraphView2.IsLegendVisible = !GraphView2.IsLegendVisible;
            GraphView1.IsLegendVisible = !GraphView1.IsLegendVisible;
            GraphView1.InvalidatePlot(true);
            GraphView2.InvalidatePlot(true);
            //     sCPIService.Write(@"*IDN?");
            //   sCPIService.Write("*IDN?");

            //_modBusService.SetAKIPPowerValue(2);

        }



        private int index;

        #endregion


        #region properties
        public PlotModel GraphView1 { get; set; }
        public PlotModel GraphView2 { get; set; }
        private LineSeries V27Series;
        private LineSeries I27Series;
        private LineSeries V100Series;
        private LineSeries I100Series;
        private LineSeries TIBXASeries;
        private LineSeries TBSCSeries;
        private LineSeries ITCVSeries;
        private TimeSpan ExpTimeSpan;
        private LineSeries ITCASeries;
        private LineSeries ITCWSeries;
        private LineSeries AKIPVSeries;
        private LineSeries AKIPASeries;
        private LineSeries AKIPWSeries;
        private DateTime StartTime;
     





        private bool _AKIPVSeriesVisible;
        public bool AKIPVSeriesVisible
        {
            get
            {
                return _AKIPVSeriesVisible;
            }
            set
            {
                Set(ref _AKIPVSeriesVisible, value);
                AKIPVSeries.IsVisible = value;
                GraphView1.InvalidatePlot(true);
            }
        }

        private bool _AKIPASeriesVisible;
        public bool AKIPASeriesVisible
        {
            get
            {
                return _AKIPASeriesVisible;
            }
            set
            {
                Set(ref _AKIPASeriesVisible, value);
                AKIPASeries.IsVisible = value;
                GraphView1.InvalidatePlot(true);
            }
        }

        private bool _AKIPWSeriesVisible;
        public bool AKIPWSeriesVisible
        {
            get
            {
                return _AKIPWSeriesVisible;
            }
            set
            {
                Set(ref _AKIPWSeriesVisible, value);
                AKIPWSeries.IsVisible = value;
                GraphView1.InvalidatePlot(true);
            }
        }

        private bool _ITCVSeriesVisible;
        public bool ITCVSeriesVisible
        {
            get
            {
                return _ITCVSeriesVisible;
            }
            set
            {
                Set(ref _ITCVSeriesVisible, value);
                ITCVSeries.IsVisible = value;
                GraphView1.InvalidatePlot(true);
            }
        }

        private bool _ITCASeriesVisible;
        public bool ITCASeriesVisible
        {
            get
            {
                return _ITCASeriesVisible;
            }
            set
            {
                Set(ref _ITCASeriesVisible, value);
                ITCASeries.IsVisible = value;
                GraphView1.InvalidatePlot(true);
            }
        }
        private bool _ITCWSeriesVisible;
        public bool ITCWSeriesVisible
        {
            get
            {
                return _ITCWSeriesVisible;
            }
            set
            {
                Set(ref _ITCWSeriesVisible, value);
                ITCWSeries.IsVisible = value;
                GraphView1.InvalidatePlot(true);
            }
        }














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
                GraphView1.InvalidatePlot(true);
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
                GraphView1.InvalidatePlot(true);
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
                GraphView1.InvalidatePlot(true);
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
                GraphView1.InvalidatePlot(true);
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
                GraphView1.InvalidatePlot(true);
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
                GraphView1.InvalidatePlot(true);
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

        private string _I100Value;
        public string I100Value
        {
            get => _I100Value;
            set => Set(ref _I100Value, value);

        }
        private string _V100Value;
        public string V100Value
        {
            get => _V100Value;
            set => Set(ref _V100Value, value);

        }

        private string _AKIPVValue;
        public string AKIPVValue
        {
            get => _AKIPVValue;
            set => Set(ref _AKIPVValue, value);

        }

        private string _AKIPAValue;
        public string AKIPAValue
        {
            get => _AKIPAValue;
            set => Set(ref _AKIPAValue, value);

        }

        private string _AKIPWValue;
        public string AKIPWValue
        {
            get => _AKIPWValue;
            set => Set(ref _AKIPWValue, value);

        }

        private string _ITCVValue;
        public string ITCVValue
        {
            get => _ITCVValue;
            set => Set(ref _ITCVValue, value);

        }

        private string _ITCAValue;
        public string ITCAValue
        {
            get => _ITCAValue;
            set => Set(ref _ITCAValue, value);

        }
        private string _ITCWValue;
        public string ITCWValue
        {
            get => _ITCWValue;
            set => Set(ref _ITCWValue, value);

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

           
            #region registerCommands
            StartExpirementCommand = new ActionCommand(StartExpirementCommandExecute, CanStartExpirementCommandExecuted);
            StopExpirementCommand = new ActionCommand(StopExpirementCommandExecute, CanStopExpirementCommandExecuted);
            ResetPlotScaleCommand = new ActionCommand(ResetPlotScaleCommandExecute, CanResetPlotScaleCommandExecute);
            CheckConnectionStatusCommand = new ActionCommand(CheckConnectionStatusCommandExecute);
            ShowHideOxyPlotLegendCommand = new ActionCommand(ShowHideOxyPlotLegendCommandExecute);
            V27Value = "V Нет данных";
            I27Value = "I Нет данных";
            OwenConnectStatus = "Нет соединения";
            
            AKIPVValue = "V Нет данных";
            AKIPAValue = "A Нет данных";
            AKIPWValue = "W Нет данных";

            ITCVValue = "V Нет данных";
            ITCAValue = "A Нет данных";
            ITCWValue = "W Нет данных";

            V100Value = "V Нет данных";
            I100Value = "I Нет данных";
            if  (DateTime.Now - Properties.Settings.Default.LastCheckDateTime > TimeSpan.FromDays(5)) // 
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
            GraphView1 = new PlotModel()
            {

            };
            GraphView2 = new PlotModel();


            V27Series = new LineSeries
            {
                Title = "V 27",
                TrackerFormatString = "{4:0.###} В {2:0.##} сек",
                Color = OxyColors.Blue,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };



            I27Series = new LineSeries
            {
                Title = "I 27",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                Color = OxyColors.BlueViolet,
                MarkerFill = OxyColors.DarkBlue,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true

            };


            V100Series = new LineSeries
            {
                Title = "V 100",
                TrackerFormatString = "{4:0} В {2:0} сек",
                Color = OxyColors.DarkOrange,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };



            I100Series = new LineSeries
            {
                Title = "I 100",
                TrackerFormatString = "{4:0} A {2:0} сек",
                Color = OxyColors.OrangeRed,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };


            GraphView1.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",

                LegendTitleFontSize = 48,
                



            }) ;


            GraphView2.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",

                LegendTitleFontSize = 48,




            });


            TIBXASeries = new LineSeries
            {
                Title = "T℃  ИБХА",
                TrackerFormatString = "{4:0} T℃  {2:0} сек",
                Color = OxyColors.Green,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };

            TBSCSeries = new LineSeries
            {
                Title = "T℃  ЭОБСК",
                TrackerFormatString = "{4:0} T℃  {2:0} сек",
                Color = OxyColors.ForestGreen,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };



            ITCVSeries = new LineSeries
            {
                Title = "V IT8516C+",
                TrackerFormatString = "{4:0.###} В {2:0.##} сек",
                Color = OxyColors.Brown,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            ITCASeries = new LineSeries
            {
                Title = "A IT8516C+",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                Color = OxyColors.RosyBrown,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            ITCWSeries = new LineSeries
            {
                Title = "W IT8516C+",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                Color = OxyColors.SandyBrown,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };


            AKIPVSeries = new LineSeries
            {
                Title = "V АКИП",
                TrackerFormatString = "{4:0.###} В {2:0.##} сек",
                Color = OxyColors.Violet,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            AKIPASeries = new LineSeries
            {
                Title = "A АКИП",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                Color = OxyColors.BlueViolet,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            AKIPWSeries = new LineSeries
            {
                Title = "W АКИП",
                TrackerFormatString = "{4:0.###} W {2:0.##} сек",
                Color = OxyColors.DarkViolet,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };


            GraphView1.Series.Add(I27Series);
            GraphView1.Series.Add(V27Series);
            GraphView1.Series.Add(TIBXASeries);
            GraphView1.Series.Add(TBSCSeries);
            GraphView1.Series.Add(V100Series);
            GraphView1.Series.Add(I100Series);
            GraphView1.Series.Add(ITCVSeries);
            GraphView1.Series.Add(ITCASeries);
            GraphView1.Series.Add(ITCWSeries);
            GraphView1.Series.Add(AKIPVSeries);
            GraphView1.Series.Add(AKIPASeries);
            GraphView1.Series.Add(AKIPWSeries);
            TIBXASeriesVisible = true;
            TBSCSeriesVisible = true;
            I27SeriesVisible = true;
            V27SeriesVisible = true;
            AKIPASeriesVisible = true;
            AKIPVSeriesVisible = true;
            AKIPWSeriesVisible = true;
            ITCASeriesVisible = true;
            ITCVSeriesVisible = true;
            ITCWSeriesVisible = true;
            V100SeriesVisible = true;
            I100SeriesVisible = true;
            GraphView2.Series.Add(new LineSeries()
            {

            });
         



        }










        private async void UpdateDataTimer_Tick(object? sender, EventArgs e)
        {
            //To Do добавить сервис записи данных в файл(лог)

            ExpTimeSpan = DateTime.Now - StartTime;




            //ReadV27Value();

            //OwenConnectStatus = false.ToConnectionStatusString();
            var result =  await  _modBusService.ReadElectroninLoadParams();
            if (result != null)
            {
                ITCVValue = result[0].ToVoltageString();
                ITCAValue = result[1].ToAmperageString();
                ITCWValue = result[2].ToPowerString();








                AKIPWSeries.Points.Add(new DataPoint(ExpTimeSpan.TotalSeconds, result[3]));
                AKIPWValue = result[3].ToPowerString();
                AKIPASeries.Points.Add(new DataPoint(ExpTimeSpan.TotalSeconds, result[4]));
                AKIPAValue  = result[4].ToAmperageString();
                AKIPVSeries.Points.Add(new DataPoint(ExpTimeSpan.TotalSeconds, result[5]));
                AKIPVValue = result[5].ToVoltageString();












                GraphView1.InvalidatePlot(true);
                GraphView2.InvalidatePlot(true);
                Debug.WriteLine($"W{result[3]}  A{result[4]} V{result[5]}");
            }
           
           // TestUpdateData();
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
            GraphView1.InvalidatePlot(true);
            GraphView2.InvalidatePlot(true);





        }



        private async void ReadV27Value()
        {
            
            var r = await _modBusService.Read27BusVoltage();
         
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
                     
                        V27Series.Points.Add(new DataPoint(ExpTimeSpan.TotalSeconds, r));
                        GraphView1.PlotView.InvalidatePlot(true);
                       GraphView2.PlotView.InvalidatePlot(true);
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
            _modBusService.SetAKIPPowerValue(commandParams.configurationMode.MaxValue);
            WriteMessage($"Отправлена команда на шину 27B: постоянная мощность (СW) {commandParams.configurationMode.MaxValue}Вт", MessageType.Info);
        }
        public void SendV100ModBusCommand(CommandParams commandParams)
        {
            if (commandParams.LastCommand)
            {
                StopExpirementCommandExecute(null);
                return;
            }
            V100SelectedIndex = commandParams.SelectedIndex;
            _modBusService.SetITCPowerValue(commandParams.configurationMode.MaxValue);
            WriteMessage($"Отправлена команда на шину 100B: постоянная мощность (СW) {commandParams.configurationMode.MaxValue}Вт", MessageType.Info);
        }
      
    }
}
