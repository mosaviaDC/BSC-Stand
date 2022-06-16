using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.SkiaSharp;
namespace BSC_Stand.Services
{
    public interface IFileExportService
    {
        public void ExportToPDF(string FileName, OxyPlot.PlotModel plotModel);

        public void ExportToXLSX(string csvFileName, string ExcelFileName);
    }
}
