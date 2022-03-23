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

namespace BSC_Stand.ViewModels
{
    internal class StandVizualizationViewModel:ViewModels.Base.ViewModelBase
    {
        public PlotModel testPlotModel  { get; private set; }
        public StandVizualizationViewModel(IFileLogger fileLogger)
        {
            fileLogger.Log("а");
      
            testPlotModel = new PlotModel();
            var verticalAxis = new LinearAxis { Position = AxisPosition.Left,  Minimum = 0, Maximum = 1000 };
            
            var horizontalAxis = new LinearAxis { Position = AxisPosition.Bottom, Maximum = 1000};
            testPlotModel.Axes.Add(verticalAxis);

            testPlotModel.Axes.Add(horizontalAxis);
            testPlotModel.Series.Add(new FunctionSeries(x => Math.Sin(x * Math.PI * 4) * Math.Sin(x * Math.PI * 4) * Math.Sqrt(x) * 100, 0, 1000, 100));
        }

    }
}
