using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;
using BSC_Stand.Models;
using BSC_Stand.Services;
using OxyPlot;
using iTextSharp;
using OxyPlot.Series;
namespace BSC_Stand.ViewModels
{
    class PostAnalyzeViewModel:ViewModels.Base.ViewModelBase
    {
        private readonly StatusBarViewModel _statusBarViewModel;
        private readonly IFileDialog _fileDialogService;
        private readonly IFileLoggerService _fileLoggerService;
        private readonly IFileExportService _fileExportService;
        private readonly IUserDialogWindowService _userDialogWindowService;
        private List<ReadingParams> importResult;
        private string CurrentOpenedFileName;

        public LineSeries V27Series { get; set; }
        public LineSeries I27Series { get; set; }
        public LineSeries V100Series { get; set; }
        public LineSeries I100Series { get; set; }
        public LineSeries TIBXASeries { get; set; }
        public LineSeries TBSCSeries { get; set; }
        public LineSeries ITCVSeries { get; set; }
        public LineSeries ITCASeries { get; set; }
        public LineSeries ITCWSeries { get; set; }
        public LineSeries AKIPVSeries { get; set; }
        public LineSeries AKIPASeries { get; set; }
        public LineSeries AKIPWSeries { get; set; }
        public PlotModel PlotModel1 { get; set; }


      public ICommand ImportLogFileCommand { get; set; }

      public ICommand ExportFileToXLSXCommand { get; set; }
      public ICommand ExportFileToPDFCommand { get; set; }


      public void ImportLogFileCommandExecute(object p)
        {
           CurrentOpenedFileName = null;
           CurrentOpenedFileName = _fileDialogService.OpenCSVFileDialog();

            if (CurrentOpenedFileName != null)
            {

                if (CurrentOpenedFileName.EndsWith(".csv"))
                {
                  
                    Thread thread = new Thread(ImportLogs);
                    thread.IsBackground = true;
                    thread.Start(CurrentOpenedFileName);
                   
                }
                else
                {
                    _userDialogWindowService.ShowErrorMessage("Файлы журналов имеют формат .csv");
                }

            }
        }


    public void ExportFileToXLSXCommandExecute(object p)
        {
            string FileName = _fileDialogService.SaveXLSXileDialog();

            if (FileName != null)
            {
                _fileExportService.ExportToXLSX(CurrentOpenedFileName, FileName);

            }
            else
            {
                _userDialogWindowService.ShowErrorMessage("Необходимо указать путь к файлу");
            }

        }


     public void ExportFileToPDFCommandExecute(object p)
        {
            string FileName = _fileDialogService.SavePDFFileDialog();

            

            if(FileName != null)
            {
                _fileExportService.ExportToPDF(FileName, this.PlotModel1,CurrentOpenedFileName,importResult);

            }
            else
            {
                _userDialogWindowService.ShowErrorMessage("Необходимо указать путь к файлу");
            }
        }

    public bool CanExportFileToPDFCommandExecuted(object p)
        {

            if (importResult!=null &&  importResult.Count > 0)
            {
                return true;
            }
            else
            {
                _userDialogWindowService.ShowErrorMessage("Для экспорта отчета необходимо выбрать файл");
                return false;
            }
        }

        public PostAnalyzeViewModel(IFileDialog fileDialogService, IFileLoggerService fileLoggerService, IUserDialogWindowService userDialogWindowService, StatusBarViewModel statusBarViewModel, IFileExportService fileExportService)
        {
            _statusBarViewModel = statusBarViewModel;
            _fileDialogService = fileDialogService;
            _fileLoggerService = fileLoggerService;
            _fileExportService = fileExportService;
            _userDialogWindowService = userDialogWindowService;           
            InitGraphSeries();
            ImportLogFileCommand = new ActionCommand(ImportLogFileCommandExecute);
            ExportFileToPDFCommand = new ActionCommand(ExportFileToPDFCommandExecute,CanExportFileToPDFCommandExecuted);
            ExportFileToXLSXCommand = new ActionCommand(ExportFileToXLSXCommandExecute);
        }



        private async void ImportLogs(object? CurrentOpenedFileName)
        {
           
            importResult = await _fileLoggerService.ReadLogs((string)CurrentOpenedFileName);
            ClearGraphSerires();
            int i = 0;
            _statusBarViewModel.SetNewTask(importResult.Count);
            foreach (var readingParams in importResult)
            {

                ITCVSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCVValue));
                ITCASeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCAValue));
                ITCWSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCWValue));

                AKIPASeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPAValue));
                AKIPVSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPVValue));
                AKIPWSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPWValue));

                V27Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V27Value));
                I27Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I27Value));

                V100Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V100Value));
                I100Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I100Value));

                TIBXASeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.IBXATemperature));
                TBSCSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.BSCTemperature));
                i++;
                _statusBarViewModel.UpdateTaskProgress(i);
            }
            return;
        }







        private void InitGraphSeries()
        {

            PlotModel1 = new PlotModel();

            {

                V27Series = new LineSeries
                {
                    Title = "V 27",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек",
                    Color = OxyColors.Blue,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };



                I27Series = new LineSeries
                {
                    Title = "I 27",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.DarkBlue,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true

                };


                V100Series = new LineSeries
                {
                    Title = "V 100",
                    TrackerFormatString = "{4:0} В {2:0} сек",
                    Color = OxyColors.DarkOrange,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                I100Series = new LineSeries
                {
                    Title = "I 100",
                    TrackerFormatString = "{4:0} A {2:0} сек",
                    Color = OxyColors.OrangeRed,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };


                TIBXASeries = new LineSeries
                {
                    Title = "T℃  ИБХА",
                    TrackerFormatString = "{4:0} T℃  {2:0} сек {0}",
                    Color = OxyColors.Green,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                TBSCSeries = new LineSeries
                {
                    Title = "T℃  ЭОБСК",
                    TrackerFormatString = "{4:0} T℃  {2:0} сек {0}",
                    Color = OxyColors.ForestGreen,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                ITCVSeries = new LineSeries
                {
                    Title = "V IT8516C+",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.Brown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCASeries = new LineSeries
                {
                    Title = "A IT8516C+",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.RosyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCWSeries = new LineSeries
                {
                    Title = "W IT8516C+",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.SandyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };


                AKIPVSeries = new LineSeries
                {
                    Title = "V АКИП",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.Violet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPASeries = new LineSeries
                {
                    Title = "A АКИП",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPWSeries = new LineSeries
                {
                    Title = "W АКИП",
                    TrackerFormatString = "{4:0.###} W {2:0.##} сек {0}",
                    Color = OxyColors.DarkViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };
            }



            PlotModel1.Series.Add(TIBXASeries);
            PlotModel1.Series.Add(TBSCSeries);

            PlotModel1.Series.Add(V27Series);
            PlotModel1.Series.Add(I27Series);

            PlotModel1.Series.Add(V100Series);
            PlotModel1.Series.Add(I100Series);

            PlotModel1.Series.Add(ITCVSeries);
            PlotModel1.Series.Add(ITCASeries);
            PlotModel1.Series.Add(ITCWSeries);

            PlotModel1.Series.Add(AKIPVSeries);
            PlotModel1.Series.Add(AKIPASeries);
            PlotModel1.Series.Add(AKIPWSeries);


            PlotModel1.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",
                LegendFontSize = 14
            });


        }


        private void ClearGraphSerires()
        {
            V27Series.Points.Clear();
            I27Series.Points.Clear();
            V100Series.Points.Clear();
            I100Series.Points.Clear();
            TIBXASeries.Points.Clear();
            TBSCSeries.Points.Clear();
            ITCVSeries.Points.Clear();
            ITCASeries.Points.Clear();
            ITCWSeries.Points.Clear();
            AKIPVSeries.Points.Clear();
            AKIPASeries.Points.Clear();
            AKIPWSeries.Points.Clear();

            PlotModel1.InvalidatePlot(true);
        }

      
    }
}
