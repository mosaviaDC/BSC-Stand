﻿using System;
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
using NModbus;
using BSC_Stand.Views.Windows;

namespace BSC_Stand.ViewModels
{
    internal class MenuWindowViewModel : ViewModels.Base.ViewModelBase
    {


        #region Properties
        private PerformanceCounter RamCounter;
        private string CurrentOpenedFileName = null;
        private readonly IWindowService _windowService;
        #endregion
        #region Services
        private readonly IFileDialog _fileDialogService;
        private readonly IProjectConfigurationService _projectConfigurationService;
        private readonly StandConfigurationViewModel _standConfigurationViewModel;
        private readonly IModbusService _modbusService;

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
            _windowService.ShowWindow<StandParamsControl>(this);
           
        }



        #endregion

        #region Services
        #endregion

        public MenuWindowViewModel(IFileDialog fileDialogService, IProjectConfigurationService projectConfigurationService, StandConfigurationViewModel standConfigurationViewModel, IWindowService windowService)
        {
            #region Services
            _fileDialogService = fileDialogService;
            _projectConfigurationService = projectConfigurationService;
            _standConfigurationViewModel = standConfigurationViewModel;
            _windowService = windowService;
            //_modbusService = modbusService;
            #endregion
            #region Commands
            SaveFileCommand = new ActionCommand(SaveFileCommandExecute, CanSaveFileCommandExecuted);
            OpenFileCommand = new ActionCommand(OpenFileCommandExecute, CanOpenFileCommandExecuted);
            CheckFileCommand = new ActionCommand(CheckFile);
            OpenPeriodStandParamsControlWindowCommand = new ActionCommand(OpenPeriodStandParamsControlWindowCommandExecute);
            #endregion
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(UpdatePerformance);
            RamCounter = new PerformanceCounter("Memory", "Available Mbytes", true);
            timer.Start();
            _Title = "ЭО БСК";
            _RamUsageText = $"Ram Usage: {RamCounter.NextValue()}";

     

        }




            private async void UpdatePerformance(object sender, EventArgs e)
            { 

            RamUsageText = $"Ram Usage: {RamCounter.NextValue() / 100}";

            }
         

        }

    }