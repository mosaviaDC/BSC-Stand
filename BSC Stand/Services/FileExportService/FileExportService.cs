using OfficeOpenXml;
using OfficeOpenXml.Table;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
namespace BSC_Stand.Services
{
    public class FileExportService : IFileExportService
    {
        public void ExportToPDF(string FileName, OxyPlot.PlotModel PlotModel1,string CSVFileName)
        {

            
            PdfDocument document = new PdfDocument(FileName);
            PdfPage page = document.AddPage();



            document.Close();
            document.Save(FileName);



           
            using (var stream = File.OpenWrite($"{FileName}"))
            {


                

                var pdfExporter = new OxyPlot.SkiaSharp.PdfExporter() { Width = 620, Height = 877 };
                pdfExporter.Export(PlotModel1, stream);
                pdfExporter.Export(PlotModel1, stream);
                
                

            }

            document = PdfSharp.Pdf.IO.PdfReader.Open(FileName);

            var newPage = document.InsertPage(0);



           //Section section = document.AddPage()
 









            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(newPage);

            // Create a font
            XFont font = new XFont("Times New Roman", 20, XFontStyle.BoldItalic, new XPdfFontOptions(PdfFontEncoding.Unicode));

            // Draw the text
            int i = 0;
            foreach (string line in System.IO.File.ReadLines($@"{CSVFileName}"))
            {
                gfx.DrawString($"{line}", font, XBrushes.Black,
      new XRect(0, i, newPage.Width, newPage.Height),
      XStringFormats.Center);

                i += 30;

            }

      

            document.Save(FileName);

        }



            public void ExportToXLSX(string csvFileName, string excelFileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string worksheetsName = $"{DateTime.Now.ToFileTime()}";

            bool firstRowIsHeader = true;

            var format = new ExcelTextFormat();
            format.Delimiter = ',';
            format.EOL = "\r";
            format.Encoding = Encoding.UTF8;
                                            

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                package.Save();
            }

        //    Process.Start(excelFileName);

        }






    }
}
