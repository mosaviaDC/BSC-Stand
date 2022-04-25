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

        private static DispatcherTimer V27expirementTimer { get; set; }
        private static DispatcherTimer V100expirementTimer { get; set; }
        private DateTime StartTime { get;set; }

        private DateTime V27NextConfigTime { get; set; }

        private int V27ConfigIndex = 0;

        private DateTime V100NextConfigTime { get; set; }

        private int V100ConfigIndex = 0;

        private ObservableCollection<ConfigurationMode> V27configurationModes;

        private ObservableCollection<ConfigurationMode> V100configurationModes;

        private bool isExpirementPepformed;
    
        public RealTimeStandControlService(BSCControlViewModel bSCControlViewModel, StandConfigurationViewModel standConfigurationViewModel)
        {
            isExpirementPepformed = false;
            V27expirementTimer = new DispatcherTimer();
            V27expirementTimer.Interval = TimeSpan.FromMilliseconds(250);
            V27expirementTimer.Tick += V27TimerEventHandler;

            V100expirementTimer = new DispatcherTimer();
            V100expirementTimer.Interval = TimeSpan.FromMilliseconds(250);
            V100expirementTimer.Tick += V100TimerEventHandler;



            V27configurationModes = standConfigurationViewModel.Bus27ConfigurationModes;
            V100configurationModes = standConfigurationViewModel.Bus100ConfigurationModes;
           
        }
        /// <summary>
        /// Обновление параметров (пересчет duration)
        /// </summary>
        public void UpdateExpiremntParams()
        {
         
            StartTime = DateTime.Now;
            V27NextConfigTime = StartTime;
            V27NextConfigTime = V27NextConfigTime.AddSeconds(V27configurationModes[V27ConfigIndex].Duration);
            V27ConfigIndex++;
            V100NextConfigTime = StartTime;
            V100NextConfigTime = V100NextConfigTime.AddSeconds(V100configurationModes[V100ConfigIndex].Duration);
            V100ConfigIndex++;
            Debug.WriteLine($"Send Command to modbus (V27) {DateTime.Now}");
            Debug.WriteLine($"Send Command to modbus (V100) {DateTime.Now}");
            V27expirementTimer.Start();
            V100expirementTimer.Start();
        }
        public void StartExpirent()
        {
           
            if (!isExpirementPepformed)
            {
                isExpirementPepformed = true;
                UpdateExpiremntParams();
            
       
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


        public void V27TimerEventHandler(object sender, EventArgs e)
        {
         
            if (DateTime.Now - V27NextConfigTime >= TimeSpan.FromMilliseconds(0) && (DateTime.Now - V27NextConfigTime <= TimeSpan.FromMilliseconds(500)))
            {
            
                if (V27ConfigIndex >= V27configurationModes.Count)
                {
                    V27expirementTimer.Stop();
                    Debug.WriteLine($"Send Command to modbus (V27) {DateTime.Now}") ;
                    Debug.WriteLine($"V27 expirement Stop {DateTime.Now}");
                    return;
                }
                
                {
                    Debug.WriteLine($"Send Command to modbus (V27) {DateTime.Now}");
                    V27NextConfigTime = V27NextConfigTime.AddSeconds(V27configurationModes[V27ConfigIndex].Duration);
                    V27ConfigIndex++;
                }
            }

        }

        public void V100TimerEventHandler(object sender, EventArgs e)
        {

            if (DateTime.Now - V100NextConfigTime >= TimeSpan.FromMilliseconds(0) && (DateTime.Now - V100NextConfigTime <= TimeSpan.FromMilliseconds(500)))
            {

                if (V100ConfigIndex >= V100configurationModes.Count)
                {
                    V100expirementTimer.Stop();
                    Debug.WriteLine($"Send Command to modbus (V100) {DateTime.Now}");
                    Debug.WriteLine($"V100 expirement Stop {DateTime.Now}");
                    return;
                }
                
                {
                    Debug.WriteLine($"Send Command to modbus (V100) {DateTime.Now}");
                    V100NextConfigTime = V100NextConfigTime.AddSeconds(V100configurationModes[V100ConfigIndex].Duration);
                    V100ConfigIndex++;
                }
            }

        }





    }
}
