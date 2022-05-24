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

using NModbus.IO;
using NModbus.Device;
using NModbus;
using BSC_Stand.Views.Windows;
using System.Windows;

namespace BSC_Stand.ViewModels
{
    internal class MenuWindowViewModel : ViewModels.Base.ViewModelBase
    {


        #region Properties
        private PerformanceCounter RamCounter;
        private string CurrentOpenedFileName = null;
        private readonly IWindowService _windowService;
        private readonly StatusBarViewModel _statusBarViewModel;
        #endregion
        #region Services
        private readonly IFileDialog _fileDialogService;
        private readonly IProjectConfigurationService _projectConfigurationService;
        private readonly StandConfigurationViewModel _standConfigurationViewModel;
        private readonly BSCControlViewModel _BSCControlViewModel;
        private readonly IModbusService _modbusService;

        #endregion
        private string _Title;
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

      

        private int _SelectedIndex;
        public int SelectedIndex
        {

            get
            {
                return _SelectedIndex;
            }
            set
            {
               
                Set(ref _SelectedIndex,value);
            }
        }


        #region Commands
        public ICommand OpenFileCommand { get; set; }


        private  async void OpenFileCommandExecute(object p)
        {
           
            CurrentOpenedFileName = _fileDialogService.OpenFileDialog();
            if (CurrentOpenedFileName != null)
            {
             
                var result = await _projectConfigurationService.GetProjectConfiguration(CurrentOpenedFileName);
                _statusBarViewModel.UpdateTaskProgress(100);
                if (result != null)
                {
                    _standConfigurationViewModel.UpdateConfigurationModes(result.V27BusConfigurationModes, result.V100BusConfigurationModes, result.V27BusCyclogramRepeatCount, result.V100BusCyclogramRepeatCount);
                    Title = $"{CurrentOpenedFileName} - ЭО БСК";
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
            Title = $"{CurrentOpenedFileName}* - ЭО БСК";
        }


        private async void SaveFileCommandExecute(object p)
        {

            Debug.WriteLine(_standConfigurationViewModel.V27BusCyclogramRepeatCount);

            //Если горячая клавиша и есть имя файла
            if (p != null && CurrentOpenedFileName != null)
            {
                await _projectConfigurationService.SaveProjectConfiguration(CurrentOpenedFileName, _standConfigurationViewModel.Bus27ConfigurationModes, _standConfigurationViewModel.Bus100ConfigurationModes, _standConfigurationViewModel.V27BusCyclogramRepeatCount, _standConfigurationViewModel.V100BusCyclogramRepeatCount);

                Title = $"{CurrentOpenedFileName} - ЭО БСК";


            }
            else
            {
                CurrentOpenedFileName = _fileDialogService.SaveFileDialog();
                if (CurrentOpenedFileName != null)

                    await _projectConfigurationService.SaveProjectConfiguration(CurrentOpenedFileName, _standConfigurationViewModel.Bus27ConfigurationModes, _standConfigurationViewModel.Bus100ConfigurationModes, _standConfigurationViewModel.V27BusCyclogramRepeatCount, _standConfigurationViewModel.V100BusCyclogramRepeatCount);
                Title = $"{CurrentOpenedFileName} - ЭО БСК";
            }



        }
        private bool CanSaveFileCommandExecuted(object p)
        {
            return true;
        }


        public ICommand OpenPeriodStandParamsControlWindowCommand { get; set; }
        public void OpenPeriodStandParamsControlWindowCommandExecute(object p)
        {
            if (Helpers.Helpers.IsWindowOpen<StandParamsControl>())
            {
                return;
            }else
            {
                _windowService.ShowWindow<StandParamsControl>(this);
                return;
            }
          
           
        }



        #endregion

        #region Services
        #endregion

        public MenuWindowViewModel(IFileDialog fileDialogService, IProjectConfigurationService projectConfigurationService, StandConfigurationViewModel standConfigurationViewModel, IWindowService windowService, BSCControlViewModel bSCControlViewModel, StatusBarViewModel statusBarViewModel)
        {
            #region Services
            _fileDialogService = fileDialogService;
            _projectConfigurationService = projectConfigurationService;
            _standConfigurationViewModel = standConfigurationViewModel;
            _BSCControlViewModel = bSCControlViewModel;
            _windowService = windowService;
            _statusBarViewModel = statusBarViewModel;
            //_modbusService = modbusService;
            #endregion
            #region Commands
            SaveFileCommand = new ActionCommand(SaveFileCommandExecute, CanSaveFileCommandExecuted);
            OpenFileCommand = new ActionCommand(OpenFileCommandExecute, CanOpenFileCommandExecuted);
            CheckFileCommand = new ActionCommand(CheckFile);
            OpenPeriodStandParamsControlWindowCommand = new ActionCommand(OpenPeriodStandParamsControlWindowCommandExecute);
            #endregion
            //timer.Start();
            _Title = "ЭО БСК";
       
           

        }




           

        }

    }