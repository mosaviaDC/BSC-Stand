using BSC_Stand.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSC_Stand.Services;
using System.Diagnostics;
using System.Collections.ObjectModel;
using BSC_Stand.Models.StandConfigurationModels;
using System.Windows.Threading;
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;
using System.IO;
using BSC_Stand.Models;
using BSC_Stand.Extensions;
using BSC_Stand.Models.StandConfigurationModels;

namespace BSC_Stand.ViewModels
{
    internal class BSCControlViewModel:ViewModelBase
    {
        private readonly IFileLoggerService _fileLoggerService;
        private readonly ReadingParams _readingParams;
        private readonly RealTimeGraphsViewModel _realTimeGraphsViewModel;
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
                _realTimeGraphsViewModel.ClearAllPoints();
                _fileLoggerService.CreateFile();
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
                            UpdateDataTimer.Start();
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
            switch (_realTimeGraphsViewModel.SelectedGraphIndex)
            {
                

                case 1:
                    _realTimeGraphsViewModel.PlotModel2.ResetAllAxes();
                    _realTimeGraphsViewModel.PlotModel2.InvalidatePlot(true);
                    break;
                case 2:
                    _realTimeGraphsViewModel.PlotModel3.ResetAllAxes();
                    _realTimeGraphsViewModel.PlotModel3.InvalidatePlot(true);
                    break;
                default:
                    _realTimeGraphsViewModel.PlotModel1.ResetAllAxes();
                    _realTimeGraphsViewModel.PlotModel1.InvalidatePlot(true);
                    break;

            }



    

        }
        private bool CanResetPlotScaleCommandExecute (object p)
        {
            return true;
        }


        public ICommand ShowHideOxyPlotLegendCommand { get; set; }

        public void ShowHideOxyPlotLegendCommandExecute(object p )
        {

            switch (_realTimeGraphsViewModel.SelectedGraphIndex)
            {
              
                case 1:
                    _realTimeGraphsViewModel.PlotModel2.IsLegendVisible = !_realTimeGraphsViewModel.PlotModel2.IsLegendVisible;
                    _realTimeGraphsViewModel.PlotModel2.InvalidatePlot(true);
                    break;
                case 2:
                    _realTimeGraphsViewModel.PlotModel3.IsLegendVisible = !_realTimeGraphsViewModel.PlotModel3.IsLegendVisible;
                    _realTimeGraphsViewModel.PlotModel3.InvalidatePlot(true);
                    break;
                default:
                    _realTimeGraphsViewModel.PlotModel1.IsLegendVisible = !_realTimeGraphsViewModel.PlotModel1.IsLegendVisible;
                    _realTimeGraphsViewModel.PlotModel1.InvalidatePlot(true);
                    break;
            }
        }

        public ICommand ZoomInPlotCOommand { get; set; }

        private void ZoomInPlotCOommandExecute(object p)
        {
            switch (_realTimeGraphsViewModel.SelectedGraphIndex)
            {

                case 0:
                    _realTimeGraphsViewModel.PlotModel1.ZoomAllAxes(2);
                    _realTimeGraphsViewModel.PlotModel1.InvalidatePlot(true);
                    break;
                case 1:
                    _realTimeGraphsViewModel.PlotModel2.ZoomAllAxes(2);
                    _realTimeGraphsViewModel.PlotModel2.InvalidatePlot(true);
                    break;
                default:
                    _realTimeGraphsViewModel.PlotModel3.ZoomAllAxes(2);
                    _realTimeGraphsViewModel.PlotModel3.InvalidatePlot(true);
                    break;
            }
        }

        public ICommand ZoomOutPlotCOommand { get; set; }

        private void ZoomOutPlotCOommandExecute(object p)
        {
            switch (_realTimeGraphsViewModel.SelectedGraphIndex)
            {

                case 0:
                    _realTimeGraphsViewModel.PlotModel1.ZoomAllAxes(0.5);
                    _realTimeGraphsViewModel.PlotModel1.InvalidatePlot(true);
                    break;
                case 1:
                    _realTimeGraphsViewModel.PlotModel2.ZoomAllAxes(0.5);
                    _realTimeGraphsViewModel.PlotModel2.InvalidatePlot(true);
                    break;
                default:
                    _realTimeGraphsViewModel.PlotModel3.ZoomAllAxes(0.5);
                    _realTimeGraphsViewModel.PlotModel3.InvalidatePlot(true);
                    break;
            }
        }


        #endregion

        #region properties

        private TimeSpan ExpTimeSpan;
        
        private DateTime StartTime;
        #region InfoStringProperties
        public string DebugString
        {
            get => _DebugString;
            set => Set(ref _DebugString, value);
        }

        private string _DebugString;


        private string _IBXATemperature;

        public string IBXATemperature
        {
            get => _IBXATemperature;
            set => Set(ref _IBXATemperature, value);
        }

        private string _BSCTemperature;

        public string BSCTemperature
        {
            get => _BSCTemperature;
            set => Set(ref _BSCTemperature, value);
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

        private int _PowerSupplySelectedIndex;
        public int PowerSupplySelectedIndex
        {
            get => _PowerSupplySelectedIndex;
            set => Set(ref _PowerSupplySelectedIndex, value);
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




        public ObservableCollection<ElectronicConfigMode> V27ConfigurationModes { get; set; }
        public ObservableCollection<ElectronicConfigMode> V100ConfigurationModes { get; set; }

        public ObservableCollection<PowerSupplyConfigMode> PowerSupplyConfigurationModes { get; set; }

        #endregion

        #endregion

        public BSCControlViewModel(StandConfigurationViewModel standConfigurationViewModel, IModbusService modbusService, IUserDialogWindowService userDialogWindowService, RealTimeGraphsViewModel realTimeGraphsViewModel, IFileLoggerService fileLoggerService)
        {
            _fileLoggerService = fileLoggerService;
            _readingParams = new ReadingParams();
            _realTimeGraphsViewModel = realTimeGraphsViewModel;
            _userDialogWindowService = userDialogWindowService;
            _standConfigurationViewModel = standConfigurationViewModel;
            V27ConfigurationModes = standConfigurationViewModel.Bus27ConfigurationModes;
            V100ConfigurationModes = standConfigurationViewModel.Bus100ConfigurationModes;
            PowerSupplyConfigurationModes = standConfigurationViewModel.PowerSupplyConfigurationModes;
            _realTimeStandControlService = new RealTimeStandControlService(this, _standConfigurationViewModel, _userDialogWindowService);
            _modBusService = modbusService;
   
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
            ZoomInPlotCOommand = new ActionCommand(ZoomInPlotCOommandExecute);
            ZoomOutPlotCOommand = new ActionCommand(ZoomOutPlotCOommandExecute);
            V27Value = "V Нет данных";
            I27Value = "I Нет данных";
            //  OwenConnectStatus = "Нет соединения";
            IBXATemperature = "Температура ИБХА - Нет данных";
            BSCTemperature = "Температура ЭО БСК - Нет данных";
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

        private async void UpdateDataTimer_Tick(object? sender, EventArgs e)
        {
            //To Do добавить сервис записи данных в файл(лог), выполняя проверку на статус эксперимента

            ExpTimeSpan = DateTime.Now - StartTime;

            //Параметры эл нагрузок;
            _readingParams.ExpTime = (float)ExpTimeSpan.TotalSeconds;

            var result =  await  _modBusService.ReadElectronicLoadParams();
            if (result != null)
            {
                
                ITCAValue = result[0].ToAmperageString();
                ITCVValue = result[1].ToVoltageString();
                ITCWValue = result[2].ToPowerString();

                AKIPWValue = result[3].ToPowerString();
                AKIPAValue  = result[4].ToAmperageString();
                AKIPVValue = result[5].ToVoltageString();

                _readingParams.ITCAValue = result[0];
                _readingParams.ITCVValue = result[1];
                _readingParams.ITCWValue = result[2];

                _readingParams.AKIPWValue= result[3];
                _readingParams.AKIPAValue= result[4];
                _readingParams.AKIPVValue= result[5];
            }
            // Параметры с преобразователей

            _readingParams.V27Value = await _modBusService.Read27BusVoltage();
            V27Value = _readingParams.V27Value.ToVoltageString();


            _readingParams.I27Value = await _modBusService.Read27BusAmperage();
            I27Value = _readingParams.I27Value.ToAmperageString();


            _readingParams.V100Value = await _modBusService.Read100BusVoltage();
            V100Value = _readingParams.V100Value.ToVoltageString();


            _readingParams.I100Value = await _modBusService.Read100BusAmperage();
            I100Value = _readingParams.I100Value.ToAmperageString();

            _readingParams.IBXATemperature = 0;
            _readingParams.BSCTemperature = 0;
            //Параметры с Owen
           IBXATemperature = 0f.ToIBXATemperatureString();
           BSCTemperature = 0f.ToBSCTemperatureString();

            if (_realTimeStandControlService.GetExperimentStatus())
            {
                _realTimeGraphsViewModel.UpdateGraphsSeries(this._readingParams);
                _readingParams.TimeStamp=((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                _fileLoggerService.WriteLog(_readingParams);
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
        public void SendPowerSupplyCommand(CommandParams commandParams)
        {

            if (commandParams.LastCommand)
            {
                StopExpirementCommandExecute(null);
                return;
            }
            PowerSupplySelectedIndex = commandParams.SelectedIndex;
            _modBusService.SetITCPowerValue(commandParams.configurationMode.MaxValue);
            WriteMessage($"Отправлена команда на источник питания: { ((PowerSupplyConfigMode)commandParams.configurationMode).MaxValue} {((PowerSupplyConfigMode)commandParams.configurationMode).MaxValue1 }", MessageType.Info);



        }
      
    }
}
