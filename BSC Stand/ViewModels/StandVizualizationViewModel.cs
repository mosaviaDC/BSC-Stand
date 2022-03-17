using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace BSC_Stand.ViewModels
{
    internal class StandVizualizationViewModel:ViewModels.Base.ViewModelBase
    {
        public PlotModel testPlotModel  { get; private set; }
        public StandVizualizationViewModel()
        {
          
            testPlotModel = new PlotModel();
           
            
            var verticalAxis = new LinearAxis { Position = AxisPosition.Left,  Minimum = 0, Maximum = 1000 };
            
            var horizontalAxis = new LinearAxis { Position = AxisPosition.Right };
            testPlotModel.Axes.Add(verticalAxis);
            testPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            testPlotModel.Series.Add(new FunctionSeries(x => Math.Sin(x * Math.PI * 4) * Math.Sin(x * Math.PI * 4) * Math.Sqrt(x) * 1000, 0, 1, 1000));
        }

    }
}
