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



        private async void ImportLogs(object? CurrentOpenedFileName)
        {
           
            importResult = await _fileLoggerService.ReadLogs((string)CurrentOpenedFileName);
            Label = Path.GetFileName((string)CurrentOpenedFileName);
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

            //Generic Plot Model

            GenericPlotModel = new PlotModel();
           

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
                    Title = "T°C  ИБХА",
                    TrackerFormatString = "{4:0} T°C  {2:0} сек {0}",
                    Color = OxyColors.Green,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                TBSCSeries = new LineSeries
                {
                    Title = "T°C  ЭОБСК",
                    TrackerFormatString = "{4:0} T°C  {2:0} сек {0}",
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
            
            GenericPlotModel.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",
                LegendFontSize = 14
            });

            //Bus27 Plot Model

            Bus27PlotModel = new PlotModel();
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

                Bus27PlotModel.Series.Add(V27Series);
                Bus27PlotModel.Series.Add(I27Series);

                Bus27PlotModel.Legends.Add(new OxyPlot.Legends.Legend()
                {
                    LegendTitle = "",
                    LegendFontSize = 14
                });

            };

            //Bus100 Plot Model

            Bus100PlotModel = new PlotModel();
            {
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

                Bus100PlotModel.Series.Add(V100Series);
                Bus100PlotModel.Series.Add(I100Series);

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

            GenericPlotModel.InvalidatePlot(true);
        }

      
    }
}
