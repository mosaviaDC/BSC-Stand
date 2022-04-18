using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BSC_Stand.Services;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using OwenioNet;
using OwenioNet.DataConverter;
using BSC_Stand.Infastructure.Owen;
using NModbus.IO;
using NModbus.Device;


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
                    _standConfigurationViewModel.UpdateConfigurationModes(result.V27BusConfigurationModes, result.V100BusConfigurationModes, result.V27BusCyclogramRepeatCount, result.V100BusCyclogramRepeatCount);
                    Title = $"ЭО БСК {CurrentOpenedFileName}";
                }
            }

        }

        private bool CanOpenFileCommandExecuted(object p)
        {
            return true;
        }

        public ICommand SaveFileCommand { get; set; }


        public ICommand CheckFileCommand { get; set; }

        private void CheckFile(object p)
        {
            Title = $"ЭО БСК {CurrentOpenedFileName} *";
        }


        private async void SaveFileCommandExecute(object p)
        {
            //Если горячая клавиша и есть имя файла
            if (p != null && CurrentOpenedFileName != null)
            {
                await _projectConfigurationService.SaveProjectConfiguration(CurrentOpenedFileName, _standConfigurationViewModel.Bus27ConfigurationModes, _standConfigurationViewModel.Bus100ConfigurationModes, _standConfigurationViewModel.V27BusCyclogramRepeatCount, _standConfigurationViewModel.V100BusCyclogramRepeatCount);

                Title = $"ЭО БСК {CurrentOpenedFileName}";


            }
            else
            {
                CurrentOpenedFileName = _fileDialogService.SaveFileDialog();
                if (CurrentOpenedFileName != null)

                    await _projectConfigurationService.SaveProjectConfiguration(CurrentOpenedFileName, _standConfigurationViewModel.Bus27ConfigurationModes, _standConfigurationViewModel.Bus100ConfigurationModes, _standConfigurationViewModel.V27BusCyclogramRepeatCount, _standConfigurationViewModel.V100BusCyclogramRepeatCount);
                Title = $"ЭО БСК {CurrentOpenedFileName}";
            }



        }
        private bool CanSaveFileCommandExecuted(object p)
        {
            return true;
        }




        #endregion

        #region Services
        #endregion

        public MenuWindowViewModel(IFileDialog fileDialogService, IProjectConfigurationService projectConfigurationService, StandConfigurationViewModel standConfigurationViewModel)
        {
            #region Services
            _fileDialogService = fileDialogService;
            _projectConfigurationService = projectConfigurationService;
            _standConfigurationViewModel = standConfigurationViewModel;
            #endregion
            #region Commands
            SaveFileCommand = new ActionCommand(SaveFileCommandExecute, CanSaveFileCommandExecuted);
            OpenFileCommand = new ActionCommand(OpenFileCommandExecute, CanOpenFileCommandExecuted);
            CheckFileCommand = new ActionCommand(CheckFile);
            #endregion
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(UpdatePerformance);
            RamCounter = new PerformanceCounter("Memory", "Available Mbytes", true);
            timer.Start();
            _Title = "ЭО БСК";
            _RamUsageText = $"Ram Usage: {RamCounter.NextValue()}";


 



            using (var owenProtocol = OwenProtocolMaster.Create(new OwenTCPClientAdapter("10.0.6.10", 502), null))
            {
                owenProtocol.OwenRead(0x265, OwenioNet.Types.AddressLengthType.Bits11, "ab.L");
                owenProtocol.OwenWrite(0x265, OwenioNet.Types.AddressLengthType.Bits11, "ab.L", new byte[] { 0x45, 0x87 });
              
            }

        
            }       

            private void UpdatePerformance(object sender, EventArgs e)
        {
         
            RamUsageText = $"Ram Usage: {RamCounter.NextValue() / 100}";
            }


        }

    }