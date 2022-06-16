using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BSC_Stand.Services
{
    public class FileExportService : IFileExportService
    {
        public void ExportToPDF(string FileName, OxyPlot.PlotModel PlotModel1)
        {
            using (var stream = File.Create($"{FileName}"))
            {
                var pdfExporter = new OxyPlot.SkiaSharp.PdfExporter() { Width = 620, Height = 877 };

                pdfExporter.Export(PlotModel1, stream);
            }


        }

        public void ExportToXLSX(string csvFileName, string excelFileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


    

            string worksheetsName = "TEST";

            bool firstRowIsHeader = false;

            var format = new ExcelTextFormat();
            format.Delimiter = ',';
            format.EOL = "\r";              // DEFAULT IS "\r\n";
                                            // format.TextQualifier = '"';

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                package.Save();
            }









            //var sheet = package.Workbook.Worksheets.Add("My Sheet");
            //var format = new ExcelTextFormat();
            //format.TextQualifier = '"';
            //format.SkipLinesBeginning = 2;
            //format.SkipLinesEnd = 1;

            //var range = sheet.Cells["A1"].LoadFromText(new FileInfo(@$"{csvFileName}"), format, TableStyles.Medium27, true);


            //Debug.WriteLine("Hello");

            //package.Save();





        }






    }
}
