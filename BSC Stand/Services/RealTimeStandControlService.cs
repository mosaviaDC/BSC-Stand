using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
using BSC_Stand.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BSC_Stand.Services
{
    internal class RealTimeStandControlService 
    {

        private static DispatcherTimer expiremtTimer { get; set; }

        private ObservableCollection<ConfigurationMode> V27configurationModes;

        private ObservableCollection<ConfigurationMode> V100configurationModes;

        private bool isExpirementPepformed;
        int V27BusDuration;
        int V100BusDuration;
        public RealTimeStandControlService(BSCControlViewModel bSCControlViewModel, StandConfigurationViewModel standConfigurationViewModel)
        {
            isExpirementPepformed = false;
            expiremtTimer = new DispatcherTimer();
            expiremtTimer.Interval = TimeSpan.FromMilliseconds(250);
            expiremtTimer.Tick += TimerEventHandler;
         
            V27configurationModes = standConfigurationViewModel.Bus27ConfigurationModes;
            V100configurationModes = standConfigurationViewModel.Bus100ConfigurationModes;
           
        }
        /// <summary>
        /// Обновление параметров (пересчет duration)
        /// </summary>
        public void UpdateExpiremntParams()
        {

            foreach (var p in V27configurationModes)
            {
                V27BusDuration += p.Duration;
            }
            foreach (var p in V100configurationModes)
            {
                V100BusDuration += p.Duration;
            }
            Debug.WriteLine($"V27Duration {V27BusDuration}  V100Duration {V100BusDuration}");
         

           


        }
        public void StartExpirent()
        {
           
            if (!isExpirementPepformed)
            {
                isExpirementPepformed = true;
                UpdateExpiremntParams();
                expiremtTimer.Start();
       
            }
            else if (isExpirementPepformed)
            {
                Debug.WriteLine("Эксперимент уже активен");
            }
          
        }
        public void StopExpirement()
        {
            isExpirementPepformed = true;
        }


        public void TimerEventHandler(object sender, EventArgs e)
        {
            Debug.WriteLine(this.GetHashCode());
            
        }





    }
}
