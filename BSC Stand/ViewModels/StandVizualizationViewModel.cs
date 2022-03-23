﻿using System;
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
    internal class StandVizualizationViewModel:ViewModels.Base.ViewModelBase
    {
        public PlotModel testPlotModel { get; set; }
        private IGraphService _graphService;
        private TwoColorAreaSeries s1;
        List<DataPoint> dataPoints { get; set; }

        public StandVizualizationViewModel(IFileLogger fileLogger, IGraphService graphService)

        {  
            _graphService = graphService;
         
            testPlotModel = new PlotModel()
            {
                Title = "Title"
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

   
            int numberOfVisiblePoints = 0;
            foreach (DataPoint dataPoint in s1.Points)
            {
                if (s1.GetScreenRectangle().Contains(s1.Transform(dataPoint)))
                {
                    numberOfVisiblePoints++;
                }
            }
            Debug.WriteLine(numberOfVisiblePoints);
            
           
            if (numberOfVisiblePoints <= 500)
            {
                testPlotModel.InvalidatePlot(true);

            }
            testPlotModel.PlotView.InvalidatePlot(true);
            s1.Points.AddRange(_graphService.GetDataPoints());
          
            //   testPlotModel.PlotView.
            //testPlotModel.InvalidatePlot(true);
        }

    }
}
