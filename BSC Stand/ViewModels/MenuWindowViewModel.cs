using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BSC_Stand.Services;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;

namespace BSC_Stand.ViewModels
{
    internal class MenuWindowViewModel : ViewModels.Base.ViewModelBase
    {
        #region Properties
        private PerformanceCounter RamCounter;
        private string CurrentOpenedFileName = null;
        #endregion
        #region Services
        private readonly IFileDialog _fileDialogService;
        private readonly IProjectConfigurationService _projectConfigurationService;
        private readonly StandConfigurationViewModel _standConfigurationViewModel;
        #endregion
        private string _Title;
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }














        private string _RamUsageText;

        public string RamUsageText
        {
            get
            {
                
                return _RamUsageText;
            }
            set
            {
          
                OnPropertyChanged("RamUsageText");
                Set(ref _RamUsageText, value);
            }
        }
       

        #region Commands
        public ICommand OpenFileCommand { get; set; }


        private async void OpenFileCommandExecute(object p)
        {
            CurrentOpenedFileName = _fileDialogService.OpenFileDialog();
            if (CurrentOpenedFileName != null)
            {
                var result = await _projectConfigurationService.GetProjectConfiguration(CurrentOpenedFileName);
                if (result != null)
                {
                    _standConfigurationViewModel.UpdateConfigurationModes(result.V27BusConfigurationModes, result.V100BusConfigurationModes);
                    Title += $" {CurrentOpenedFileName}";
                }
            }
           
        }

        private bool CanOpenFileCommandExecuted(object p)
        {
            return true;
        }

        public ICommand SaveFileCommand { get; set; }   

        private async void SaveFileCommandExecute(object p)
        {
            //Если горячая клавиша и есть имя файла
            if (p != null && CurrentOpenedFileName !=null)
            {
                await _projectConfigurationService.SaveProjectConfiguration(CurrentOpenedFileName, _standConfigurationViewModel.Bus27ConfigurationModes, _standConfigurationViewModel.Bus100ConfigurationModes);




            }
            else
            {
                CurrentOpenedFileName = _fileDialogService.SaveFileDialog();
                if (CurrentOpenedFileName != null)

                    await _projectConfigurationService.SaveProjectConfiguration(CurrentOpenedFileName, _standConfigurationViewModel.Bus27ConfigurationModes, _standConfigurationViewModel.Bus100ConfigurationModes);
            }


         
        }
        private bool CanSaveFileCommandExecuted(object p)
        {
            return true;
        }




        #endregion

        #region Services
        #endregion

        public MenuWindowViewModel(IFileDialog  fileDialogService, IProjectConfigurationService projectConfigurationService,StandConfigurationViewModel standConfigurationViewModel)
        {
            #region Services
            _fileDialogService = fileDialogService;
            _projectConfigurationService = projectConfigurationService;
            _standConfigurationViewModel = standConfigurationViewModel;
            #endregion
            #region Commands
            SaveFileCommand = new ActionCommand(SaveFileCommandExecute, CanSaveFileCommandExecuted);
            OpenFileCommand = new ActionCommand(OpenFileCommandExecute, CanOpenFileCommandExecuted);
            #endregion
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(UpdatePerformance) ;
            RamCounter = new PerformanceCounter("Memory", "Available Mbytes", true);
            timer.Start();
            _Title = "ЭО БСК";
            _RamUsageText = $"Ram Usage: {RamCounter.NextValue()}";
        }

        private void UpdatePerformance(object sender, EventArgs e)
        {
            RamUsageText = $"Ram Usage: {RamCounter.NextValue()/100}";
        }

       
    }
}
