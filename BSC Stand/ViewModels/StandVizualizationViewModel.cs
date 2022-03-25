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
                TrackerFormatString = "{2:0} Вт",
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
            testPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Unit="сек" });

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(UpdatePlot);
            timer.Start();

           
           
        }

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
