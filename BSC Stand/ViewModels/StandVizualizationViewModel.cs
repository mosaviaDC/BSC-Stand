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
using BSC_Stand.Services.GraphServices;
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























        #endregion





        public PlotModel testPlotModel { get; set; }
        private IGraphService _graphService;
        private TwoColorAreaSeries s1;
        List<DataPoint> dataPoints { get; set; }

        public StandVizualizationViewModel(IFileLogger fileLogger, IGraphService graphService)

        {
            _graphService = graphService;

            for (int i = 0; i < 150; i++)
            {
                _TextBlock += $"Информационное сообщение {i} \n";
            }


            //     _TextBlock = "Информационное Сообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\nСообщение\n ";


            testPlotModel = new PlotModel()
            {
                Title = "Циклограмма"
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

            testPlotModel.Series.Add(s1);
            testPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Unit = "Вт", ExtraGridlines = new[] { 0.0 } });
            testPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Unit = "сек", ExtraGridlines = new[] { 0.0 } });

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(UpdatePlot);
            //   timer.Start();

   
           
        }

        public void UpdatePlotModel(ConfigurationMode configurationMode)
        {
          
            if (s1.Points.Count > 0)
            {
               // Debug.WriteLine("UpdatePlot");
                var previos = s1.Points.Last();


                s1.Points.Add(new DataPoint(previos.X + configurationMode.Duration, configurationMode.MaxValue));
                //foreach (var configMode in configurationModes)
                //{
                //    s1.Points.Add(new DataPoint(configMode.Duration, configMode.MaxValue));
                //}

               
            }
            else
            {
                s1.Points.Add(new DataPoint(0, configurationMode.MaxValue));
                s1.Points.Add(new DataPoint(configurationMode.Duration, configurationMode.MaxValue));
            }
            testPlotModel.InvalidatePlot(true);


        }
        public void UpdateAllPlot(ObservableCollection<ConfigurationMode> configurationModes)
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
            testPlotModel.InvalidatePlot(true);
            


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
        



        private  void UpdatePlot(object sender, EventArgs e)
        {
            // Debug.WriteLine("UpdatePlot");

            s1.Points.AddRange(_graphService.GetDataPoints());
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
                testPlotModel.PlotView.InvalidatePlot(true);
               
            }

            // else if (numberOfVisiblePoints <=3000)
            //  testPlotModel.PlotView.InvalidatePlot(true);


            //   testPlotModel.PlotView.
            //testPlotModel.InvalidatePlot(true);
        }

    }
}
