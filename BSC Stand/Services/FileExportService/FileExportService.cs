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
using OxyPlot.SkiaSharp;
using BSC_Stand.Models;

namespace BSC_Stand.Services
{
    public class FileExportService : IFileExportService
    {
        public void ExportToPDF(string FileName, OxyPlot.PlotModel PlotModel1, string CSVFileName, List<ReadingParams> readingParams)
        {
            // Calculating Max and Min
            int i = 0;
            float V27max = 0;
            float V27min = 0;
            float I27max = 0;
            float I27min = 0;
            float ExpTimeVMax;
            float ExpTimeVMin;
            long TimeStampVMax;
            long TimeStampVMin;
            float ExpTimeIMax;
            float ExpTimeIMin;
            long TimeStampIMax;
            long TimeStampIMin;
            foreach (var param in readingParams)
            {
                if (i == 0)
                {
                    V27max = param.V27Value;
                    V27min = param.V27Value;
                    I27max = param.I27Value;
                    I27min = param.I27Value;
                    ExpTimeVMax = param.ExpTime;
                    ExpTimeVMin = param.ExpTime;
                    TimeStampVMax = param.TimeStamp;
                    TimeStampVMin = param.TimeStamp;
                    ExpTimeIMax = param.ExpTime;
                    ExpTimeIMin = param.ExpTime;
                    TimeStampIMax = param.TimeStamp;
                    TimeStampIMin = param.TimeStamp;
                } else {
                    if (param.V27Value >= V27max)
                    {
                        V27max = param.V27Value;
                        ExpTimeVMax = param.ExpTime;
                        TimeStampVMax = param.TimeStamp;
                    }
                    if (param.V27Value <= V27min)
                    {
                        V27min = param.V27Value;
                        ExpTimeVMin = param.ExpTime;
                        TimeStampVMin = param.TimeStamp;
                    }
                    if (param.I27Value >= I27max)
                    {
                        I27max = param.I27Value;
                        ExpTimeIMax = param.ExpTime;
                        TimeStampIMax = param.TimeStamp;
                    }
                    if (param.I27Value <= I27min)
                    {
                        I27min = param.I27Value;
                        ExpTimeIMin = param.ExpTime;
                        TimeStampIMin = param.TimeStamp;
                    }
                }
            }

            PdfDocument document = new PdfDocument(FileName);
            PdfPage page1 = document.InsertPage(0);
            document.Close();
            document.Save(FileName);

            // Export to PNG
            var stream = new MemoryStream();
            var pngExporter = new PngExporter { Width = 600, Height = 700, Dpi = 110 };
            pngExporter.Export(PlotModel1, stream);

            // Вставка PNG в PDF
            document = PdfSharp.Pdf.IO.PdfReader.Open(FileName);
            page1 = document.Pages[0];
            // Section section = document.AddPage()
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page1);

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, 0, 0, 600, 700);

            //Draw Table

            // Create a font
            XFont font = new XFont("Times New Roman", 10, XFontStyle.BoldItalic, new XPdfFontOptions(PdfFontEncoding.Unicode));

            // Draw the text
            var y = 350;
            foreach (string line in System.IO.File.ReadLines($@"{CSVFileName}"))
            {
                gfx.DrawString($"Напряжение шины 27В    Сила тока шины 27В  Секунда эксперимента    Время фиксации значения(Unix TimeStamp UTC +3)", font, XBrushes.Black,
                    new XRect(0, y, page1.Width, page1.Height),
                    XStringFormats.Center);

                y += 30;
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
