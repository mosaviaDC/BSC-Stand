using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BSC_Stand.Services.FileDialogService;
using System.Windows.Input;

namespace BSC_Stand.ViewModels
{
    internal class MenuWindowViewModel:ViewModels.Base.ViewModelBase
    {
        #region Properties
        private PerformanceCounter RamCounter;
        #endregion
        private IFileDialog _fileDialogService;
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
        

        #endregion

        #region Services
        #endregion

        public MenuWindowViewModel(IFileDialog  fileDialogService)
        {
            _fileDialogService = fileDialogService;
        
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(UpdatePerformance) ;
            RamCounter = new PerformanceCounter("Memory", "Available Mbytes", true);
            timer.Start();
            _RamUsageText = $"Ram Usage: {RamCounter.NextValue()}";
        }

        private void UpdatePerformance(object sender, EventArgs e)
        {
           
        }

       
    }
}
