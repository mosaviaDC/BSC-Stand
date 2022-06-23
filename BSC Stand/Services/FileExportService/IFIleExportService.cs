using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSC_Stand.Models;
using OxyPlot;
namespace BSC_Stand.Services
{
    public interface IFileExportService
    {
     

        public void ExportToXLSX(string csvFileName, string ExcelFileName);

        public void ExportToPDF(string FileName, OxyPlot.PlotModel PlotModel1, string CSVFileName, List<ReadingParams> readingParams);
    }
}
