using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace BSC_Stand.ViewModels
{
    internal class StatusBarViewModel: ViewModels.Base.ViewModelBase
    {

        private string _RamUsageText;

        public string RamUsageText
        {
            get
            {
                return _RamUsageText;
            }
            set
            {
                Set(ref _RamUsageText, value);
            }
        }


        private int _MaxStatusBarValue;
        public int MaxStatusBarValue {
            get => _MaxStatusBarValue;
            set => Set(ref _MaxStatusBarValue, value);
      }


        private int _CurrentProggreValue;
        private int TotalPhysicalMemoryMb;

        public int CurrentProggresValue
        {
            get => _CurrentProggreValue;


            set => Set(ref _CurrentProggreValue, value);
        }

        private string _CurrentTaskName;
        public string CurrentTaskName
        {
            get => _CurrentTaskName;
            set => Set(ref _CurrentTaskName, value);
           
        }




        public void SetNewTask(int MaxValue = 100, string TaskName ="")
        {
            CurrentProggresValue = 0;
            MaxStatusBarValue = MaxValue; 
            CurrentTaskName = TaskName;
       
        }
        public void UpdateTaskProgress(int Value)
        {
           CurrentProggresValue = Value;
            if (CurrentProggresValue == MaxStatusBarValue)
            {
            
                CurrentTaskName = CurrentTaskName + ":Выполнено";
                CurrentProggresValue = 0;
                
            }
        }


        public StatusBarViewModel()
        {
            CurrentProggresValue = 0;
      
          var   UpdateDataTimer = new DispatcherTimer();
            UpdateDataTimer.Interval = TimeSpan.FromMilliseconds(100);
            UpdateDataTimer.Tick += UpdateDataTimer_Tick;
            UpdateDataTimer.Start();
            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                TotalPhysicalMemoryMb = (int)  Convert.ToDouble(result["TotalVisibleMemorySize"]);
                TotalPhysicalMemoryMb = TotalPhysicalMemoryMb / 1024;
            }


        }

        private void UpdateDataTimer_Tick(object? sender, EventArgs e)
        {
            var result = (Process.GetCurrentProcess().PrivateMemorySize64 / 1024) / 1024;

            RamUsageText = $"RAM Usage {result}Mb / {TotalPhysicalMemoryMb}Mb";
      
        }
    }
}
