using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using BSC_Stand.Services.FileLoggingService;
using System.IO;
using BSC_Stand.Services;
using System.Diagnostics;
using System.Threading;
using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
using System.Collections.ObjectModel;

namespace BSC_Stand.ViewModels
{
    internal class StandVizualizationViewModel : ViewModels.Base.ViewModelBase
    {
        #region Properties
        private string _TextBlock;
        public string TextBlock
        {
            get => _TextBlock;
            set => Set(ref _TextBlock, value);
        }


        private float _27VBusAmperage;
        public float L27VBusAmperage
        {
            get => _27VBusAmperage;
            set => Set(ref _27VBusAmperage, value);
        }


        public PlotModel Bus27PlotModel { get; set; }
        public PlotModel Bus100PlotModel { get; set; }
        private IGraphService _graphService;
        public TwoColorAreaSeries s1 { get; set; }
        public TwoColorAreaSeries s2 { get; set; }

        #endregion





 
        List<DataPoint> dataPoints { get; set; }

        public StandVizualizationViewModel(IFileLogger fileLogger, IGraphService graphService)

        {
            _graphService = graphService;

            for (int i = 0; i < 150; i++)
            {
                _TextBlock += $"Информационное сообщение {i} \n";
            }


            //     _TextBlock = "Информационное Сообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\n ";

            Bus27PlotModel = new PlotModel { Title = "27B" };
            Bus100PlotModel = new PlotModel
            {
                Title = "100B"
            };
            s1 = new TwoColorAreaSeries
            {
                Title = "Мощность ",
                TrackerFormatString = "{4:0} Вт {2:0} сек",
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
            s2 = new TwoColorAreaSeries()
            {

                Title = "Мощность ",
                TrackerFormatString = "{4:0} Вт {2:0} сек",
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

            Bus27PlotModel.Series.Add(s1);
            Bus27PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Unit = "Вт", ExtraGridlines = new[] { 0.0 } });
            Bus27PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Unit = "сек", ExtraGridlines = new[] { 0.0 } });
            Bus100PlotModel.Series.Add(s2);
            Bus100PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Unit = "Вт", ExtraGridlines = new[] { 0.0 } });
            Bus100PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Unit = "сек", ExtraGridlines = new[] { 0.0 } });
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(UpdatePlot);
            //   timer.Start();

   
           
        }

    
        public void Update27BusPlotModel(ObservableCollection<ConfigurationMode> configurationModes)
        {
            if (configurationModes.Count > 0)
            {
                s1.Points.Clear();
                if (configurationModes.Count == 1)
                {


                }

                {
                    s1.Points.Add(new DataPoint(0, configurationModes[0].MaxValue));
                    s1.Points.Add(new DataPoint(configurationModes[0].Duration, configurationModes[0].MaxValue));

                    foreach (var configurationMode in configurationModes)
                    {
                        if (configurationModes.IndexOf(configurationMode) > 0)
                        {
                            s1.Points.Add(new DataPoint(s1.Points.Last().X, configurationMode.MaxValue));
                            s1.Points.Add(new DataPoint(configurationMode.Duration + s1.Points.Last().X, configurationMode.MaxValue));
                            
                        }

                    }

                }
                Bus27PlotModel.InvalidatePlot(true);
            }
        }
        public void Update100BusPlotModel(ObservableCollection<ConfigurationMode> configurationModes)
        {
            if (configurationModes.Count > 0)
            {
                s2.Points.Clear();
                if (configurationModes.Count == 1)
                {


                }

                {
                    s2.Points.Add(new DataPoint(0, configurationModes[0].MaxValue));
                    s2.Points.Add(new DataPoint(configurationModes[0].Duration, configurationModes[0].MaxValue));

                    foreach (var configurationMode in configurationModes)
                    {
                        if (configurationModes.IndexOf(configurationMode) > 0)
                        {
                            s2.Points.Add(new DataPoint(s2.Points.Last().X, configurationMode.MaxValue));
                            s2.Points.Add(new DataPoint(configurationMode.Duration + s2.Points.Last().X, configurationMode.MaxValue));
                        }

                    }

                }
                Bus100PlotModel.InvalidatePlot(true);
            }

        }













        //ConfigurationMode[] modes = new ConfigurationMode[configurationModes.Count];
        //s1.Points.Clear();
        //if (configurationModes.Count > 0)
        //{
        //    if (configurationModes.Count == 1)
        //    {
        //        s1.Points.Add(new DataPoint(configurationModes[0].Duration,configurationModes[0].MaxValue));
        //    }
        //    for (int i = 0; i < modes.Length; i++)
        //    {
        //        if (i == 0)
        //        {
        //            modes[i] = new ConfigurationMode()
        //            {
        //                Discreteness = configurationModes[i].Discreteness,
        //                MaxValue = configurationModes[i].MaxValue,
        //                MinValue = configurationModes[i].MinValue,
        //                ModeName = configurationModes[i].ModeName,
        //                Duration = configurationModes[i].Duration,
        //                ModeUnit = configurationModes[i].ModeUnit,


        //            };
        //            s1.Points.Add(new DataPoint(0, modes[i].MaxValue));
        //        }

        //        if (i > 0)
        //        {
        //            int sum = 0;
        //            for (int j = 0; j < configurationModes.Count; j++)
        //            {
        //                sum += configurationModes[j].Duration;
        //            }

        //       //     Debug.WriteLine($"{sum}");
        //            modes[i] = new ConfigurationMode()
        //            {
        //                Discreteness = configurationModes[i].Discreteness,
        //                MaxValue = configurationModes[i].MaxValue,
        //                MinValue = configurationModes[i].MinValue,
        //                ModeName = configurationModes[i].ModeName,
        //                Duration = sum,
        //                ModeUnit = configurationModes[i].ModeUnit,

        //            };
        //            s1.Points.Add(new DataPoint(modes[i].Duration, modes[i].MaxValue));
        //        }

        //    }






        //   Debug.WriteLine($"UpdateAllPlot {modes.Length}");
        //ObservableCollection<ConfigurationMode> newConfigs = new ObservableCollection<ConfigurationMode>();
        //newConfigs = configurationMode();
        //    testPlotModel.InvalidatePlot(true);




        private void UpdatePlot(object sender, EventArgs e)
        {
            // Debug.WriteLine("UpdatePlot");

            s2.Points.AddRange(_graphService.GetDataPoints());
            int numberOfVisiblePoints = 0;
            foreach (DataPoint dataPoint in s1.Points)
            {
                if (s1.GetScreenRectangle().Contains(s1.Transform(dataPoint)))
                {
                    numberOfVisiblePoints++;
                }
            }
            Debug.WriteLine(numberOfVisiblePoints);
            
           
            if (numberOfVisiblePoints <= 3000)
            {
                Bus27PlotModel.PlotView.InvalidatePlot(true);
               
            }

            // else if (numberOfVisiblePoints <=3000)
            //  testPlotModel.PlotView.InvalidatePlot(true);


            //   testPlotModel.PlotView.
            //testPlotModel.InvalidatePlot(true);
        }

    }
}
