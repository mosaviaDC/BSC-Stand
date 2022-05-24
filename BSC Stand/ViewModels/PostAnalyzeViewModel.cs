using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;

namespace BSC_Stand.ViewModels
{
    class PostAnalyzeViewModel:ViewModels.Base.ViewModelBase
    {
       public PlotModel model { get; set; } 




        public PostAnalyzeViewModel()
        {
            LineSeries lineSeries = new LineSeries();
            model = new PlotModel();
            model.Series.Add(lineSeries);
            
        }
    }
}
