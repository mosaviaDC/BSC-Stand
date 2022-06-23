﻿using OfficeOpenXml;
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


            float[] comparingParams = new float[12];
            float[] maxParams = new float[12];
            float[] minParams = new float[12];
            float[] ExpTimeMax = new float[12];
            float[] ExpTimeMin = new float[12];
            long[] TimeStampMax = new long[12];
            long[] TimeStampMin = new long[12];

            {
                var param = readingParams[0];
                maxParams[0] = param.ITCAValue;
                maxParams[1] = param.ITCVValue;
                maxParams[2] = param.ITCWValue;

                maxParams[3] = param.AKIPAValue;
                maxParams[4] = param.AKIPVValue;
                maxParams[5] = param.AKIPWValue;

                maxParams[6] = param.V27Value;
                maxParams[7] = param.I27Value;

                maxParams[8] = param.V100Value;
                maxParams[9] = param.I100Value;

                maxParams[10] = param.IBXATemperature;
                maxParams[11] = param.BSCTemperature;

                minParams[0] = param.ITCAValue;
                minParams[1] = param.ITCVValue;
                minParams[2] = param.ITCWValue;

                minParams[3] = param.AKIPAValue;
                minParams[4] = param.AKIPVValue;
                minParams[5] = param.AKIPWValue;

                minParams[6] = param.V27Value;
                minParams[7] = param.I27Value;

                minParams[8] = param.V100Value;
                minParams[9] = param.I100Value;

                minParams[10] = param.IBXATemperature;
                minParams[11] = param.BSCTemperature;

                for (int i = 0; i < 12; i++)
                {
                    ExpTimeMax[i] = param.ExpTime;
                    ExpTimeMin[i] = param.ExpTime;
                    TimeStampMax[i] = param.TimeStamp;
                    TimeStampMin[i] = param.TimeStamp;
                }
            }

            foreach (var param in readingParams)
            {
                comparingParams[0] = param.ITCAValue;
                comparingParams[1] = param.ITCVValue;
                comparingParams[2] = param.ITCWValue;

                comparingParams[3] = param.AKIPAValue;
                comparingParams[4] = param.AKIPVValue;
                comparingParams[5] = param.AKIPWValue;

                comparingParams[6] = param.V27Value;
                comparingParams[7] = param.I27Value;

                comparingParams[8] = param.V100Value;
                comparingParams[9] = param.I100Value;

                comparingParams[10] = param.IBXATemperature;
                comparingParams[11] = param.BSCTemperature;

                for (int i = 0; i < 12; i++)
                {
                    if (comparingParams[i] >= maxParams[i])
                    {
                        maxParams[i] = comparingParams[i];
                        ExpTimeMax[i] = param.ExpTime;
                        TimeStampMax[i] = param.TimeStamp;
                    }
                    if (comparingParams[i] <= minParams[i])
                    {
                        minParams[i] = comparingParams[i];
                        ExpTimeMin[i] = param.ExpTime;
                        TimeStampMin[i] = param.TimeStamp;
                    }
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
            const int IMG_HEIGHT = 650;
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
            font = new XFont("Consolas", 10, XFontStyle.BoldItalic, new XPdfFontOptions(PdfFontEncoding.Unicode));
            XPen pen = new XPen(XColors.Black, 1);

            string[,,] lines = {
                {   //Page 1
                    {"Параметр", "Max", "Секунда", "Время", "Min", "Секунда", "Время"},
                    {"Напряжение шины 27В", Convert.ToString(maxParams[6]),
                                            Convert.ToString(ExpTimeMax[6]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMax[6])),
                                            Convert.ToString(minParams[6]),
                                            Convert.ToString(ExpTimeMin[6]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMin[6]))},
                    {"Напряжение шины 27В", Convert.ToString(maxParams[7]),
                                            Convert.ToString(ExpTimeMax[7]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMax[7])),
                                            Convert.ToString(minParams[7]),
                                            Convert.ToString(ExpTimeMin[7]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMin[7]))}
                },
                {   //Page 2
                    {"Параметр", "Max", "Секунда", "Время", "Min", "Секунда", "Время"},
                    {"Напряжение шины 100В", Convert.ToString(maxParams[8]),
                                            Convert.ToString(ExpTimeMax[8]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMax[8])),
                                            Convert.ToString(minParams[8]),
                                            Convert.ToString(ExpTimeMin[8]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMin[8]))},
                    {"Напряжение шины 100В", Convert.ToString(maxParams[9]),
                                            Convert.ToString(ExpTimeMax[9]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMax[9])),
                                            Convert.ToString(minParams[9]),
                                            Convert.ToString(ExpTimeMin[9]),
                                            Convert.ToString(UnixTimeStampToDateTime(TimeStampMin[9]))}
                }
            };

            for (int page = 0; page < 2; page++)
            {
                // Draw the TABLE
                var x = 30;

                for (int col = 0; col < 7; col++)
                {
                    var max_col_width = 0;
                    for (int row = 0; row < 3; row++)
                    {
                        var line = lines[page, row, col];
                        var line_width = (int) (line.Length * 5.5) + 7;
                        if (line_width > max_col_width)
                        {
                            max_col_width = line_width;
                        }
                    }

                    var y = TABLE_TOP;
                    gfxs[page].DrawRectangle(XBrushes.LightGray, new XRect(x, TABLE_TOP + PAGE_HEIGHT / 2 - 15, max_col_width, 30));
                    for (int row = 0; row < 3; row++)
                    {
                        var line = lines[page, row, col];
                        gfxs[page].DrawRectangle(pen, new XRect(x, TABLE_TOP + PAGE_HEIGHT / 2 - 15, max_col_width, 30 * (row + 1)));
                        gfxs[page].DrawString(line, font, XBrushes.Black, new XRect(x + 3, y, max_col_width, PAGE_HEIGHT), XStringFormats.CenterLeft);
                        y += 30;
                    }
                    x += max_col_width;
                }

            }


            document.Save(FileName);

        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
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
