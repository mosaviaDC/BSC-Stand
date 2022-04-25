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

namespace BSC_Stand.ViewModels
{
    internal class BSCControlViewModel:ViewModelBase
    {
        #region services
        private StandConfigurationViewModel _standConfigurationViewModel;
        private RealTimeStandControlService _realTimeStandControlService;
        private IModbusService _modBusService;
        private static DispatcherTimer UpdateDataTimer;
        #endregion

        #region properties
        public PlotModel GraphView { get; set; }

        private TwoColorAreaSeries s1;
        private TwoColorAreaSeries s2;
        private DateTime StartTime;
        #endregion


        public BSCControlViewModel(StandConfigurationViewModel standConfigurationViewModel, IModbusService modbusService)
        {
            _standConfigurationViewModel = standConfigurationViewModel;
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
         UpdateDataTimer.Start();
        }

        private async void UpdateDataTimer_Tick(object? sender, EventArgs e)
        {
           var result =  await _modBusService.ReadDataFromOwenController();
 
           byte[] bytes = new byte[result.Length * sizeof(ushort)];

            var temp = BitConverter.GetBytes(result[0]);
            Buffer.BlockCopy(temp, 0, bytes, 0, temp.Length);
            temp = BitConverter.GetBytes(result[1]);
            Buffer.BlockCopy(temp, 0, bytes, 2, temp.Length);



            float a = BitConverter.ToSingle(bytes, 0);
            var r = DateTime.Now - StartTime;
            s1.Points.Add(new DataPoint(r.TotalSeconds,a));
      

            
           GraphView.InvalidatePlot(true);
            Debug.WriteLine(a);
        }

        public void SendV27ModBusCommand(ConfigurationMode configurationMode)
        {
            Debug.WriteLine($"Send V27 ModBus Command {DateTime.Now} {configurationMode.MaxValue}");
        }
        public void SendV100ModBusCommand(ConfigurationMode configurationMode)
        {
            Debug.WriteLine($"Send V100 ModBus Command {DateTime.Now} {configurationMode.MaxValue}");
        }
        public void StartExpiremnt()
        {
          
            _realTimeStandControlService.StartExpirent();
        }
    }
}
