using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace BSC_Stand.Services.GraphServices
{
    internal class DebugGraphService : IGraphService
    {


        private int CurrentIndex;
        private readonly PlotModel plotModel;
        public DebugGraphService()
        {
          if (plotModel == null)
            {
                plotModel = new PlotModel();
                CurrentIndex = 0;
            }
        }

        public List<DataPoint> GetDataPoints()
        {
            Random random = new Random();
            DataPoint[] dataPoints = new DataPoint[25];
            for (int i = 0; i < dataPoints.Length; i++)
            {
                dataPoints[i] = new DataPoint(i+CurrentIndex,random.Next(100,105));
            }
            CurrentIndex += 25;
            return dataPoints.ToList();
        }

        public PlotModel GetPlotModel()
        {
            
            plotModel.Title = "Привет";
       
            var s1 = new TwoColorAreaSeries
            {
                Title = "Power at ",
                TrackerFormatString = "{2:0} сек",
                Color = OxyColors.Black,
                Color2 = OxyColors.Red,
                Fill = OxyColors.Transparent,
                Fill2 = OxyColors.Transparent,
                //Color = OxyColors.Black,
                //Color2 = OxyColors.Brown,
                //MarkerFill = OxyColors.Red,
                ////  Fill = OxyColors.Tomato,
                ////Fill2 = OxyColors.LightBlue,
                //MarkerFill2 = OxyColors.Blue,
                //MarkerStroke = OxyColors.Brown,
                //MarkerStroke2 = OxyColors.Black,
                //StrokeThickness = 2,
                Limit = 130,
             

                //   MarkerType = MarkerType.Circle,
                // MarkerSize = 3,
            };
            

            var temperatures = new int[500];

            Random random = new Random();
            for (int i = 0; i < temperatures.Length; i++)
            {
                if (i < 200)
                {
                    temperatures[i] = 130;

                }
                if (i >= 200 && i < 250)
                {
                    temperatures[i] = 250;
                }
                if (i >= 250 && i < 300)
                {
                    temperatures[i] = 100;
                }
                if (i >= 300 && i < 500)
                {
                    temperatures[i] = 130;
                }




                s1.Points.Add(new DataPoint(i + 1, temperatures[i]));
            }
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "P", Unit = "Вт", ExtraGridlines = new[] { 0.0 } });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "t", Unit = "с" });

            var series = new TwoColorAreaSeries()
            {
                DataFieldX = "1",
                DataFieldY = "2",
            };
           // PlotModel.InvalidatePlot(true);
            return plotModel;
        }
    }
}
