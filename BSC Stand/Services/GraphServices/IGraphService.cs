using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
namespace BSC_Stand.Services
{
    internal interface IGraphService
    {
        PlotModel GetPlotModel();

        List<DataPoint> GetDataPoints();


    }
}
