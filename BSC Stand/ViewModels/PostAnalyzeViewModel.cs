using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;
using BSC_Stand.Services;
using OxyPlot;
using OxyPlot.Series;

namespace BSC_Stand.ViewModels
{
    class PostAnalyzeViewModel:ViewModels.Base.ViewModelBase
    {
        private readonly IFileDialog _fileDialogService;
        private readonly IFileLoggerService _fileLoggerService;
        private readonly IUserDialogWindowService _userDialogWindowService;
        private readonly LineSeries lineSeries;

        public PlotModel model { get; set; }


      public ICommand ImportLogFileCommand { get; set; }

     public async void ImportLogFileCommandExecute(object p)
        {
          string CurrentOpenedFileName=  _fileDialogService.OpenFileDialog("Открыть файл");

            if (CurrentOpenedFileName != null)
            {

                if (CurrentOpenedFileName.EndsWith(".csv"))
                {
                    var r = await _fileLoggerService.ReadLogs(CurrentOpenedFileName);
                    int i = 0;
                    lineSeries.Points.Clear();
                   await Task.Factory.StartNew(() =>
                    {
                        foreach (var log in r)
                        {
                            lineSeries.Points.Add(new DataPoint(log.ExpTime, log.AKIPAValue));
                            i++;
                        }
                        Debug.WriteLine(lineSeries.Points.Count);
                        model.InvalidatePlot(true);
                    });
                   
                 
                }
                else
                {
                    _userDialogWindowService.ShowErrorMessage("Файлы журналов имеют формат .csv"); 
                }



            }
        }

        public PostAnalyzeViewModel(IFileDialog fileDialogService, IFileLoggerService fileLoggerService, IUserDialogWindowService userDialogWindowService)
        {
            _fileDialogService = fileDialogService;
            _fileLoggerService = fileLoggerService;
            _userDialogWindowService = userDialogWindowService;

            lineSeries = new LineSeries();
            lineSeries.Points.Add(new DataPoint(0, 0));
            model = new PlotModel();
            model.Series.Add(lineSeries);
            ImportLogFileCommand = new ActionCommand(ImportLogFileCommandExecute);
            
        }

      
    }
}
