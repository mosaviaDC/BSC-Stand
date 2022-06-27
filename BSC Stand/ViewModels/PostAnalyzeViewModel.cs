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
    class PostAnalyzeViewModel : ViewModels.Base.ViewModelBase
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
        public LineSeries TetronVSeries { get; set; }
        public LineSeries TetronASeries { get; set; }
        public LineSeries TetronWSeries { get; set; }

        public LineSeries V27Series2 { get; set; }
        public LineSeries I27Series2 { get; set; }

        public LineSeries V100Series3 { get; set; }
        public LineSeries I100Series3 { get; set; }

        public PlotModel GenericPlotModel { get; set; }
        public PlotModel Bus27PlotModel { get; set; }
        public PlotModel Bus100PlotModel { get; set; }

        public int _SelectedGraphIndex;
        public int SelectedGraphIndex
        {
            get => _SelectedGraphIndex;
            set => Set(ref _SelectedGraphIndex, value);
        }

        private string _Label;

        public string Label
        {
            get
            {
                return _Label;
            }
            set
            {
                Set(ref _Label, value);
            }
        }


        public ICommand ImportLogFileCommand { get; set; }
        public ICommand ExportFileToXLSXCommand { get; set; }
        public ICommand ExportFileToPDFCommand { get; set; }
        public ICommand ResetPlotScaleCommand { get; set; }
        public ICommand ShowHideOxyPlotLegendCommand { get; set; }
        public ICommand ZoomInPlotCOommand { get; set; }
        public ICommand ZoomOutPlotCOommand { get; set; }

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

                _fileExportService.ExportToPDF(FileName, GenericPlotModel, Bus27PlotModel, Bus100PlotModel, CurrentOpenedFileName, importResult);

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

        private void ResetPlotScaleCommandExecute(object p)
        {
            switch (SelectedGraphIndex)
            {

                case 0:
                    GenericPlotModel.ResetAllAxes();
                    GenericPlotModel.InvalidatePlot(true);
                    break;
                case 1:
                    Bus27PlotModel.ResetAllAxes();
                    Bus27PlotModel.InvalidatePlot(true);
                    break;
                default:
                    Bus100PlotModel.ResetAllAxes();
                    Bus100PlotModel.InvalidatePlot(true);
                    break;

            }
        }

        private void ShowHideOxyPlotLegendCommandExecute(object p)
        {
            switch (SelectedGraphIndex)
            {

                case 0:
                    GenericPlotModel.IsLegendVisible = !GenericPlotModel.IsLegendVisible;
                    GenericPlotModel.InvalidatePlot(true);
                    break;
                case 1:
                    Bus27PlotModel.IsLegendVisible = !Bus27PlotModel.IsLegendVisible;
                    Bus27PlotModel.InvalidatePlot(true);
                    break;
                default:
                    Bus100PlotModel.IsLegendVisible = !Bus100PlotModel.IsLegendVisible;
                    Bus100PlotModel.InvalidatePlot(true);
                    break;
            }
        }

        private void ZoomInPlotCOommandExecute(object p)
        {
            switch (SelectedGraphIndex)
            {

                case 0:
                    GenericPlotModel.ZoomAllAxes(2);
                    GenericPlotModel.InvalidatePlot(true);
                    break;
                case 1:
                    Bus27PlotModel.ZoomAllAxes(2);
                    Bus27PlotModel.InvalidatePlot(true);
                    break;
                default:
                    Bus100PlotModel.ZoomAllAxes(2);
                    Bus100PlotModel.InvalidatePlot(true);
                    break;
            }
        }

        private void ZoomOutPlotCOommandExecute(object p)
        {
            switch (SelectedGraphIndex)
            {

                case 0:
                    GenericPlotModel.ZoomAllAxes(0.5);
                    GenericPlotModel.InvalidatePlot(true);
                    break;
                case 1:
                    Bus27PlotModel.ZoomAllAxes(0.5);
                    Bus27PlotModel.InvalidatePlot(true);
                    break;
                default:
                    Bus100PlotModel.ZoomAllAxes(0.5);
                    Bus100PlotModel.InvalidatePlot(true);
                    break;
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
            ExportFileToXLSXCommand = new ActionCommand(ExportFileToXLSXCommandExecute, CanExportFileToPDFCommandExecuted);
            ResetPlotScaleCommand = new ActionCommand(ResetPlotScaleCommandExecute);
            ShowHideOxyPlotLegendCommand = new ActionCommand(ShowHideOxyPlotLegendCommandExecute);
            ZoomInPlotCOommand = new ActionCommand(ZoomInPlotCOommandExecute);
            ZoomOutPlotCOommand = new ActionCommand(ZoomOutPlotCOommandExecute);
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        private async void ImportLogs(object? CurrentOpenedFileName)
        {
           
            importResult = await _fileLoggerService.ReadLogs((string)CurrentOpenedFileName);
            Label = Path.GetFileName((string)CurrentOpenedFileName);
            ClearGraphSerires();
            int i = 0;
            _statusBarViewModel.SetNewTask(importResult.Count);
            foreach (var readingParams in importResult)
            {
                //ITCVSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(UnixTimeStampToDateTime(readingParams.TimeStamp)), readingParams.ITCVValue));

                double time_param = readingParams.ExpTime;

                ITCVSeries.Points.Add(new DataPoint(time_param, readingParams.ITCVValue));
                ITCASeries.Points.Add(new DataPoint(time_param, readingParams.ITCAValue));
                ITCWSeries.Points.Add(new DataPoint(time_param, readingParams.ITCWValue));
                
                AKIPASeries.Points.Add(new DataPoint(time_param, readingParams.AKIPAValue));
                AKIPVSeries.Points.Add(new DataPoint(time_param, readingParams.AKIPVValue));
                AKIPWSeries.Points.Add(new DataPoint(time_param, readingParams.AKIPWValue));

                V27Series.Points.Add(new DataPoint(time_param, readingParams.V27Value));
                I27Series.Points.Add(new DataPoint(time_param, readingParams.I27Value));

                V27Series2.Points.Add(new DataPoint(time_param, readingParams.V27Value));
                I27Series2.Points.Add(new DataPoint(time_param, readingParams.I27Value));

                V100Series.Points.Add(new DataPoint(time_param, readingParams.V100Value));
                I100Series.Points.Add(new DataPoint(time_param, readingParams.I100Value));

                V100Series3.Points.Add(new DataPoint(time_param, readingParams.V100Value));
                I100Series3.Points.Add(new DataPoint(time_param, readingParams.I100Value));

                TIBXASeries.Points.Add(new DataPoint(time_param, readingParams.IBXATemperature));
                TBSCSeries.Points.Add(new DataPoint(time_param, readingParams.BSCTemperature));

                TetronASeries.Points.Add(new DataPoint(time_param, readingParams.TetronVValue));
                TetronASeries.Points.Add(new DataPoint(time_param, readingParams.TetronAValue));
                TetronASeries.Points.Add(new DataPoint(time_param, readingParams.TetronWValue));
                i++;
                _statusBarViewModel.UpdateTaskProgress(i);
            }

            GenericPlotModel.ResetAllAxes();
            GenericPlotModel.InvalidatePlot(true);
            Bus27PlotModel.ResetAllAxes();
            Bus27PlotModel.InvalidatePlot(true);
            Bus100PlotModel.ResetAllAxes();
            Bus100PlotModel.InvalidatePlot(true);

            return;
        }







        private void InitGraphSeries()
        {

            //Generic Plot Model

            GenericPlotModel = new PlotModel();
            GenericPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis());



            {

                V27Series = new LineSeries
                {
                    Title = "V 27",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.Blue,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };



                I27Series = new LineSeries
                {
                    Title = "I 27",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.DarkBlue,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true

                };


                V100Series = new LineSeries
                {
                    Title = "V 100",
                    TrackerFormatString = "{0}\n{4:0} В\n{2:0} сек",
                    Color = OxyColors.DarkOrange,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                I100Series = new LineSeries
                {
                    Title = "I 100",
                    TrackerFormatString = "{0}\n{4:0} A\n{2:0} сек",
                    Color = OxyColors.OrangeRed,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };


                TIBXASeries = new LineSeries
                {
                    Title = "T°C  ИБХА",
                    TrackerFormatString = "{0}\n{4:0} T°C\n{2:0} сек",
                    Color = OxyColors.Green,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                TBSCSeries = new LineSeries
                {
                    Title = "T°C  ЭОБСК",
                    TrackerFormatString = "{0}\n{4:0} T°C\n{2:0} сек",
                    Color = OxyColors.ForestGreen,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                ITCVSeries = new LineSeries
                {
                    Title = "V IT8516C+",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.Brown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCASeries = new LineSeries
                {
                    Title = "A IT8516C+",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.RosyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCWSeries = new LineSeries
                {
                    Title = "W IT8516C+",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.SandyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };


                AKIPVSeries = new LineSeries
                {
                    Title = "V АКИП",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.Violet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPASeries = new LineSeries
                {
                    Title = "A АКИП",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPWSeries = new LineSeries
                {
                    Title = "W АКИП",
                    TrackerFormatString = "{0}\n{4:0.###} W\n{2:0.##} сек",
                    Color = OxyColors.DarkViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronVSeries = new LineSeries
                {
                    Title = "V Тетрон",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.LightGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronASeries = new LineSeries
                {
                    Title = "A Тетрон",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.Gray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronWSeries = new LineSeries
                {
                    Title = "W Тетрон",
                    TrackerFormatString = "{0}\n{4:0.###} W\n{2:0.##} сек",
                    Color = OxyColors.DarkGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

            }

            GenericPlotModel.Series.Add(TIBXASeries);
            GenericPlotModel.Series.Add(TBSCSeries);

            GenericPlotModel.Series.Add(V27Series);
            GenericPlotModel.Series.Add(I27Series);

            GenericPlotModel.Series.Add(V100Series);
            GenericPlotModel.Series.Add(I100Series);

            GenericPlotModel.Series.Add(ITCVSeries);
            GenericPlotModel.Series.Add(ITCASeries);
            GenericPlotModel.Series.Add(ITCWSeries);

            GenericPlotModel.Series.Add(AKIPVSeries);
            GenericPlotModel.Series.Add(AKIPASeries);
            GenericPlotModel.Series.Add(AKIPWSeries);

            GenericPlotModel.Series.Add(TetronVSeries);
            GenericPlotModel.Series.Add(TetronASeries);
            GenericPlotModel.Series.Add(TetronWSeries);

            GenericPlotModel.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",
                LegendFontSize = 14
            });

            //Bus27 Plot Model

            Bus27PlotModel = new PlotModel();

            Bus27PlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis());

            {
                V27Series2 = new LineSeries
                {
                    Title = "V 27",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    
                    Color = OxyColors.Blue,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                I27Series2 = new LineSeries
                {
                    Title = "I 27",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.DarkBlue,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true

                };

                Bus27PlotModel.Series.Add(V27Series2);
                Bus27PlotModel.Series.Add(I27Series2);

                Bus27PlotModel.Legends.Add(new OxyPlot.Legends.Legend()
                {
                    LegendTitle = "",
                    LegendFontSize = 14
                });

            };

            //Bus100 Plot Model

            Bus100PlotModel = new PlotModel();

            Bus100PlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis());

            {
                V100Series3 = new LineSeries
                {
                    Title = "V 100",
                    TrackerFormatString = "{0}\n{4:0} В\n{2:0} сек",
                    Color = OxyColors.DarkOrange,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                I100Series3 = new LineSeries
                {
                    Title = "I 100",
                    TrackerFormatString = "{0}\n{4:0} A\n{2:0} сек",
                    Color = OxyColors.OrangeRed,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                Bus100PlotModel.Series.Add(V100Series3);
                Bus100PlotModel.Series.Add(I100Series3);

                Bus100PlotModel.Legends.Add(new OxyPlot.Legends.Legend()
                {
                    LegendTitle = "",
                    LegendFontSize = 14
                });

            };


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
            TetronVSeries.Points.Clear();
            TetronASeries.Points.Clear();
            TetronWSeries.Points.Clear();

            V27Series2.Points.Clear();
            I27Series2.Points.Clear();

            V100Series3.Points.Clear();
            I100Series3.Points.Clear();

            GenericPlotModel.InvalidatePlot(true);
        }

      
    }
}
