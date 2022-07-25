using BSC_Stand.Infastructure.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using System.Xml;
using System.Windows;
using BSC_Stand.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BSC_Stand.ViewModels
{
    internal class ConnectionCheckListViewModel : ViewModels.Base.ViewModelBase
    {

        public ObservableCollection<bool> CurrentProgressList { get; set; }
        private string _LastDateString;
        public string LastDateString 
        { get=> _LastDateString; 
           
            set => Set(ref _LastDateString, value);
        }




        private bool _TemperatureBSKStatus;
        public bool TemperatureBSKStatus
        {
            get
            {
                return _TemperatureBSKStatus;
            }
            set
            {
                Set(ref _TemperatureBSKStatus, value);
                CurrentProgressList[0] = false;
                CurrentProgressList[1] = true;
            }

        }

        private bool _TemperatureIBXAStatus;
        public bool TemperatureIBXAStatus
        {
            get { return _TemperatureIBXAStatus; }
            set
            {
                Set(ref _TemperatureIBXAStatus, value);

                CurrentProgressList[1] = false;
                CurrentProgressList[2] = true;
            }
        }

        private bool _V27Status;
        public bool V27Status
        {

            get => _V27Status;
            set
            {

                Set(ref _V27Status, value);

                CurrentProgressList[2] = false;
                CurrentProgressList[3] = true;
            }
        }


        private bool _I27Status;
        public bool I27Status
        {

            get => _I27Status;
            set
            {

                Set(ref _I27Status, value);

                CurrentProgressList[3] = false;
                CurrentProgressList[4] = true;
            }
        }


        private bool _V100Status;
        public bool V100Status
        {

            get => _V100Status;
            set
            {

                Set(ref _V100Status, value);

                CurrentProgressList[4] = false;
                CurrentProgressList[5] = true;
            }
        }


        private bool _I100Status;
        public bool I100Status
        {

            get => _I100Status;
            set
            {

                Set(ref _I100Status, value);

                CurrentProgressList[5] = false;
                CurrentProgressList[6] = true;
            }
        }

        private bool _AkipStatus;

        public bool AkipStatus
        {
            get => _AkipStatus;
            set {
                Set(ref _AkipStatus, value);
                CurrentProgressList[6] = false;
                CurrentProgressList[7] = true;
            }
        }
        private bool _IT8516CStatus;

        public bool IT8516CStatus
        {
            get => _IT8516CStatus;
            set
            {
                Set(ref _AkipStatus, value);
                CurrentProgressList[7] = false;
                CurrentProgressList[8] = true;
            }
        }
        private bool _TetronStatus;

        public bool TetronStatus
        {
            get => _TetronStatus;
            set
            {
                Set(ref _TetronStatus, value);

                CurrentProgressList[8] = false;

                FinishCheckProcedure();
               
            }
        }

        private readonly IUserDialogWindowService _userDialogWindowService;
        private readonly IModbusService _modBusService;

        public ICommand StartCheckProcedure { get; set; }

        private void StartCheckProcedureExecute(object p)
        {
            //TODO отправить комманды проверки на источник питания
            if (_modBusService.GetConnectStatus())
            {
                _modBusService.SetAKIPPowerValue(5);
                _modBusService.SetITCPowerValue(10);
              //  _modBusService.SetPowerSupplyValue(0, 2);


                for (int i = 0; i < CurrentProgressList.Count; i++)
                {
                    CurrentProgressList[i] = false;
                }
                TemperatureBSKStatus = false;
                TemperatureIBXAStatus = false;
                AkipStatus = false;
                V27Status = false;
                I27Status = false;
                V100Status = false;
                I100Status = false;
                IT8516CStatus = false;
                TetronStatus = false;
                CurrentProgressList[0] = true;
            }
            else
            {
                _userDialogWindowService.ShowErrorMessage("Для выполнения периодической проверки необходимо подключение ко всем устройствам");
            }

        }
        private bool CanStartCheckProcedureExecuted(object p)
        {
            return true;
        }
        

        public ConnectionCheckListViewModel(IModbusService modbusService, IUserDialogWindowService userDialogWindowService)
        {
            _userDialogWindowService = userDialogWindowService;
            _modBusService = modbusService;
              StartCheckProcedure = new ActionCommand(StartCheckProcedureExecute, CanStartCheckProcedureExecuted);

            CurrentProgressList = new ObservableCollection<bool>
          {
              false,
              false,
              false,
              false,
              false,
              false,
              false,
              false,
              false
          };
            //DateTime dateTime;
            //var result = DateTime.TryParse(ConfigurationManager.AppSettings["LastCheckDate"], out dateTime);
          _LastDateString = $"Дата последней проверки {Properties.Settings.Default.LastCheckDateTime} ";
     //     _LastDateString = $"Дата последней проверки {dateTime}";

        }


        private void FinishCheckProcedure()
        {
            for (int i = 0; i < CurrentProgressList.Count; i++)
            {
                CurrentProgressList[i] = false;
            }

            Properties.Settings.Default.LastCheckDateTime = DateTime.Now;
            Properties.Settings.Default.Save();
            LastDateString = $"Дата последней проверки {Properties.Settings.Default.LastCheckDateTime} ";
        }


    }
}
