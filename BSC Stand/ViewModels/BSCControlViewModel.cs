﻿using BSC_Stand.ViewModels.Base;
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

        private void StartExpirementCommandExecute (object p)
        {
            if (V27ConfigurationModes.Count ==0 || V100ConfigurationModes.Count == 0)
            {
                _userDialogWindowService.ShowErrorMessage("Выбрана пустая конфигурация");
            }
            else
            {
                WriteMessage("Начало эксперимента", MessageType.Info);
                _realTimeStandControlService.StartExpirent();
                UpdateDataTimer?.Start();
            }

            
        }


        private bool CanStartExpirementCommandExecuted(object p)
        {


            return true;

        }


        public ICommand StopExpirementCommand { get; set; }

        private void StopExpirementCommandExecute(object p)
        {
            WriteMessage("Эксперимент остановлен", MessageType.Info);
            UpdateDataTimer?.Stop();
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
             
        #endregion 


        #region properties
        public PlotModel GraphView { get; set; }

        private TwoColorAreaSeries s1;
        private TwoColorAreaSeries s2;
        private DateTime StartTime;

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


        private bool _OwenConnectStatus;

        public bool OwenConnectStatus
        {
            get => _OwenConnectStatus;
            set => Set(ref _OwenConnectStatus, value);
        }

        public ObservableCollection<ConfigurationMode> V27ConfigurationModes { get; set; }
        public ObservableCollection<ConfigurationMode> V100ConfigurationModes { get; set; }


        #endregion


        public BSCControlViewModel(StandConfigurationViewModel standConfigurationViewModel, IModbusService modbusService, IUserDialogWindowService userDialogWindowService)
        {
            _userDialogWindowService = userDialogWindowService;
            _standConfigurationViewModel = standConfigurationViewModel;
            V27ConfigurationModes = standConfigurationViewModel.Bus27ConfigurationModes;
            V100ConfigurationModes = standConfigurationViewModel.Bus100ConfigurationModes;
            _realTimeStandControlService = new RealTimeStandControlService(this, _standConfigurationViewModel);
           _modBusService = modbusService;

            GraphView = new PlotModel()
            {

            };
             s1 = new TwoColorAreaSeries
            {
                Title = "Сек",
                TrackerFormatString = "{4:0} T {2:0} сек",
                Color = OxyColors.Black,
                Color2 = OxyColors.Brown,
                MarkerFill = OxyColors.Red,
                Fill = OxyColors.Transparent,
                Fill2 = OxyColors.Transparent,
                MarkerFill2 = OxyColors.Blue,
                MarkerStroke = OxyColors.Brown,
                MarkerStroke2 = OxyColors.Black,
                StrokeThickness = 2,
                Limit = 0,

                MarkerType = MarkerType.Diamond,
                MarkerSize = 1,
            };
 
            GraphView.Series.Add(s1);
            UpdateDataTimer = new DispatcherTimer();
            UpdateDataTimer.Interval = TimeSpan.FromMilliseconds(1000);
            UpdateDataTimer.Tick += UpdateDataTimer_Tick;
            StartTime = DateTime.Now;
            

            #region registerCommands
            StartExpirementCommand = new ActionCommand(StartExpirementCommandExecute, CanStartExpirementCommandExecuted);
            StopExpirementCommand = new ActionCommand(StopExpirementCommandExecute, CanStopExpirementCommandExecuted);
            ResetPlotScaleCommand = new ActionCommand(ResetPlotScaleCommandExecute, CanResetPlotScaleCommandExecute);


            #endregion
        }

        private async void UpdateDataTimer_Tick(object? sender, EventArgs e)
        {
           
        
            //var result =  await _modBusService.ReadDataFromOwenController();
            //if (result != null)
            //{
            //    byte[] bytes = new byte[result.Length * sizeof(ushort)];
            //    OwenConnectStatus = _modBusService.GetOwenConnectionStatus();
            //    var temp = BitConverter.GetBytes(result[0]);
            //    Buffer.BlockCopy(temp, 0, bytes, 0, temp.Length);
            //    temp = BitConverter.GetBytes(result[1]);
            //    Buffer.BlockCopy(temp, 0, bytes, 2, temp.Length);
            //    OwenTemperature = BitConverter.ToSingle(bytes, 0);
            //    var r = DateTime.Now - StartTime;
            //    s1.Points.Add(new DataPoint(r.TotalSeconds, OwenTemperature));
            //    GraphView.InvalidatePlot(true);

            //}
          
        }

        public void WriteMessage(string Message, MessageType messageType)
        {
            DebugString += $"[{messageType.ToString()}] {Message} {DateTime.Now}\n";
        }

        public void SendV27ModBusCommand(CommandParams commandParams)
        {
            V27SelectedIndex = commandParams.SelectedIndex;
            WriteMessage($"Отправлена команда на шину 27B: стабилизация мощности {commandParams.configurationMode.MaxValue}Вт", MessageType.Info);
        }
        public void SendV100ModBusCommand(CommandParams commandParams)
        {
            V100SelectedIndex = commandParams.SelectedIndex;
            WriteMessage($"Отправлена команда на шину 100B: стабилизация мощности {commandParams.configurationMode.MaxValue}Вт", MessageType.Info);
        }
      
    }
}
