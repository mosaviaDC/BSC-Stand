using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.ViewModels
{
    internal class StatusBarViewModel: ViewModels.Base.ViewModelBase
    {




        private int _MaxStatusBarValue;
        public int MaxStatusBarValue {
            get => _MaxStatusBarValue;
            set => Set(ref _MaxStatusBarValue, value);
      }


        private int _CurrentProggreValue;
        public int CurrentProggresValue
        {
            get => _CurrentProggreValue;


            set => Set(ref _CurrentProggreValue, value);
        }




        public void SetNewTask(int MaxValue =100)
        {
            CurrentProggresValue = 0;
            MaxStatusBarValue = MaxValue; 
       
        }
        public void UpdateTaskProgress(int Value)
        {
            if (Value == MaxStatusBarValue)
            {
                SetNewTask(100);
            }
            else 
           CurrentProggresValue = Value;
        }


        public StatusBarViewModel()
        {
            CurrentProggresValue = 0;

        }

    }
}
