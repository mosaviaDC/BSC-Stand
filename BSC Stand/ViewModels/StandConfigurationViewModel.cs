using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BSC_Stand.Infastructure.Commands;
using BSC_Stand.Models.StandConfigurationModels;

namespace BSC_Stand.ViewModels
{
    class StandConfigurationViewModel:ViewModels.Base.ViewModelBase
    {
        #region Properties

        private  ProgrammablePowerSupplyModule _selectedConfigModule;
        public ProgrammablePowerSupplyModule SelectedConfigModule
        {
            get => _selectedConfigModule;
            set => Set(ref _selectedConfigModule, value);
        }
        public ObservableCollection<ProgrammablePowerSupplyModule> programmablePowerSupplyModules { get; set; }



        private ProgrammablePowerSupplyModule _Tetron15016C;
        public ProgrammablePowerSupplyModule Tetron15016C 
        {
            get => _Tetron15016C;

            set => Set(ref _Tetron15016C, value);
        }

        private ProgrammablePowerSupplyModule _AKIP_1311;
        public ProgrammablePowerSupplyModule AKIP_1311
        {
            get => _AKIP_1311;
            set => Set(ref _AKIP_1311, value);
        }

        private ProgrammablePowerSupplyModule _AKIP_1311_4;
        public ProgrammablePowerSupplyModule AKIP_1311_4
        {
            get => _AKIP_1311_4;
            set => Set(ref _AKIP_1311_4, value);
        }




        #endregion

        #region Commands
       public ICommand SelectedItemCommand { get; }
       private void SelectedItemCommandExecute(object p)
        {
            Debug.WriteLine("**");
        }
       private bool CanSelectedItemCommandExecuted(object p )
        {
            return true;
        }


        #endregion

        #region Services
        #endregion Services

        public StandConfigurationViewModel()
        {
            
            _Tetron15016C = new ProgrammablePowerSupplyModule();
            _AKIP_1311 = new ProgrammablePowerSupplyModule("АКИП-1311");
            _AKIP_1311_4 = new ProgrammablePowerSupplyModule("АКИП-1311/4");
            _selectedConfigModule = _Tetron15016C;
            programmablePowerSupplyModules = new ObservableCollection<ProgrammablePowerSupplyModule>();
            programmablePowerSupplyModules.Add(_Tetron15016C);
            programmablePowerSupplyModules.Add(_AKIP_1311);
            programmablePowerSupplyModules.Add(_AKIP_1311_4);
            #region Commands
            SelectedItemCommand = new ActionCommand(SelectedItemCommandExecute);
            SelectedItemCommand.Execute(null);
            #endregion

        }





    }
}
