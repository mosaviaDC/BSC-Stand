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
            int k = 0;
            float V27max = 0;
            float V27min = 0;
            float I27max = 0;
            float I27min = 0;
            float ExpTimeVMax = 0;
            float ExpTimeVMin = 0;
            long TimeStampVMax = 0;
            long TimeStampVMin = 0;
            float ExpTimeIMax = 0;
            float ExpTimeIMin = 0;
            long TimeStampIMax = 0;
            long TimeStampIMin = 0;

            {
                var param = readingParams[0];
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
            }

            foreach (var param in readingParams)
            {
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

            // Creating PDF
            PdfDocument document = new PdfDocument(FileName);
            document.AddPage();
            document.AddPage();
            document.AddPage();
            document.Close();
            document.Save(FileName);

            document = PdfSharp.Pdf.IO.PdfReader.Open(FileName);
            PdfPage[] pages = {
                document.Pages[0],
                document.Pages[1],
                document.Pages[2]
            };

            // Export to PNG
            Stream[] streams = {
                new MemoryStream(),
                new MemoryStream(),
                new MemoryStream()
            };

            // CONSTS
            const int IMG_WIDTH = 600;
            const int IMG_HEIGHT = 550;
            double PAGE_WIDTH = pages[0].Width;
            double PAGE_HEIGHT = pages[0].Height;
            var TABLE_TOP = IMG_HEIGHT - (PAGE_HEIGHT / 2) + 70;

            // Export Charts to PNG for 3 pages
            var pngExporter = new PngExporter { Width = IMG_WIDTH, Height = IMG_HEIGHT, Dpi = 70 };
            bool[,] series_states =
            {
                { false, false, true, true, false, false, false, false, false, false, false, false },
                { false, false, false, false, true, true, false, false, false, false, false, false },
                { true, true, true, true, true, true, true, true, true, true, true, true }
            };


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    PlotModel1.Series[j].IsVisible = series_states[i, j];
                }
                pngExporter.Export(PlotModel1, streams[i]);
            }

            // Вставка PNG в PDF
            
            // Section section = document.AddPage()
            // Get an XGraphics object for drawing
            XGraphics[] gfxs = {
                XGraphics.FromPdfPage(pages[0]),
                XGraphics.FromPdfPage(pages[1]),
                XGraphics.FromPdfPage(pages[2])
            };

            XImage[] images =
            {
                XImage.FromStream(streams[0]),
                XImage.FromStream(streams[1]),
                XImage.FromStream(streams[2])
            };
            gfxs[0].DrawImage(images[0], 30, 30, IMG_WIDTH - 60, IMG_HEIGHT);
            gfxs[1].DrawImage(images[1], 30, 30, IMG_WIDTH - 60, IMG_HEIGHT);
            gfxs[2].DrawImage(images[2], 30, 30, IMG_WIDTH - 60, IMG_HEIGHT);

            //Draw Table

            // Create a font
            XFont font = new XFont("Times New Roman", 20, XFontStyle.BoldItalic, new XPdfFontOptions(PdfFontEncoding.Unicode));

            //Draw Title
            gfxs[0].DrawString("График для шины 27В", font, XBrushes.Black, new XRect(0, -PAGE_HEIGHT / 2 + 20, PAGE_WIDTH, PAGE_HEIGHT), XStringFormats.Center);
            gfxs[0].DrawString("Данные для шины 27В", font, XBrushes.Black, new XRect(0, TABLE_TOP - 30, PAGE_WIDTH, PAGE_HEIGHT), XStringFormats.Center);
            gfxs[1].DrawString("График для шины 100В", font, XBrushes.Black, new XRect(0, -PAGE_HEIGHT / 2 + 20, PAGE_WIDTH, PAGE_HEIGHT), XStringFormats.Center);
            gfxs[1].DrawString("Данные для шины 100В", font, XBrushes.Black, new XRect(0, TABLE_TOP - 30, PAGE_WIDTH, PAGE_HEIGHT), XStringFormats.Center);
            gfxs[2].DrawString("Сводный график", font, XBrushes.Black, new XRect(0, -PAGE_HEIGHT / 2 + 20, PAGE_WIDTH, PAGE_HEIGHT), XStringFormats.Center);
            gfxs[2].DrawString("Данные", font, XBrushes.Black, new XRect(0, TABLE_TOP - 30, PAGE_WIDTH, PAGE_HEIGHT), XStringFormats.Center);

            // Init to draw TABLE
            font = new XFont("Times New Roman", 10, XFontStyle.BoldItalic, new XPdfFontOptions(PdfFontEncoding.Unicode));
            XPen pen = new XPen(XColors.Black, 1);
            string[,] lines = {
                {"", "Напряжение шины 27В","Сила тока шины 27В"},
                {"Минимальное значение", Convert.ToString(V27min), Convert.ToString(I27min)},
                {"Секунда эксперимента", Convert.ToString(ExpTimeVMin), Convert.ToString(ExpTimeIMin)},
                {"Время фиксации значения(Unix TimeStamp UTC +3)", Convert.ToString(TimeStampVMin), Convert.ToString(TimeStampIMin)},
                {"Максимальное значение", Convert.ToString(V27max), Convert.ToString(I27max)},
                {"Секунда эксперимента", Convert.ToString(ExpTimeVMax), Convert.ToString(ExpTimeIMax)},
                {"Время фиксации значения(Unix TimeStamp UTC +3)", Convert.ToString(TimeStampVMax), Convert.ToString(TimeStampIMax)},
            };

            // Draw the TABLE
            var x = 30;
            
            for (int col = 0; col < 3; col++)
            {
                var max_col_width = 0;
                for (int row = 0; row < 7; row++)
                {
                    var line = lines[row, col];
                    var line_width = line.Length * 6 + 15;
                    if (line_width > max_col_width)
                    {
                        max_col_width = line_width;
                    }
                }
                
                var y = TABLE_TOP;
                gfxs[0].DrawRectangle(XBrushes.LightGray, new XRect(x, TABLE_TOP + PAGE_HEIGHT / 2 - 15, max_col_width, 30));
                for (int row = 0; row < 7; row++)
                {
                    var line = lines[row, col];
                    gfxs[0].DrawRectangle(pen, new XRect(x, TABLE_TOP + PAGE_HEIGHT / 2 - 15, max_col_width, 30 * (row + 1)));
                    gfxs[0].DrawString(line, font, XBrushes.Black, new XRect(x + 10, y, max_col_width, PAGE_HEIGHT), XStringFormats.CenterLeft);
                    y += 30;
                }
                x += max_col_width;
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
