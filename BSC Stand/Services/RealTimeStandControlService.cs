using BSC_Stand.Models;
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

        public delegate void V27Msg(CommandParams commandParams);
        public event V27Msg _V27MsgEvent;

        public delegate void V100Msg(CommandParams commandParamse);
        public event V100Msg _V100MsgEvent;

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
            _V27MsgEvent += bSCControlViewModel.SendV27ModBusCommand;
            _V100MsgEvent += bSCControlViewModel.SendV100ModBusCommand;
            //V100Msg v100Msg = bSCControlViewModel.SendV100ModBusCommand;
            //V27Msg v27Msg = bSCControlViewModel.SendV100ModBusCommand;
          

        }
        /// <summary>
        /// Обновление параметров (пересчет duration)
        /// </summary>
        public void UpdateExpiremntParams()
        {

            if (V100configurationModes.Count > 0 || V27configurationModes.Count > 0)
            {

                StartTime = DateTime.Now;
                V27NextConfigTime = StartTime;
                V27NextConfigTime = V27NextConfigTime.AddSeconds(V27configurationModes[V27ConfigIndex].Duration);
                V27ConfigIndex++;
                V100NextConfigTime = StartTime;
                V100NextConfigTime = V100NextConfigTime.AddSeconds(V100configurationModes[V100ConfigIndex].Duration);
                V100ConfigIndex++;
                _V27MsgEvent?.Invoke(new CommandParams(V27configurationModes[0],0));
                _V100MsgEvent?.Invoke(new CommandParams(V100configurationModes[0],0));
                V27expirementTimer.Start();
                V100expirementTimer.Start();
            }
            else
            {
                Debug.WriteLine("Пустой конфиг");
            }
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
                    //_V27MsgEvent?.Invoke(V27configurationModes[V27ConfigIndex]);
             
                    Debug.WriteLine($"V27 expirement Stop {DateTime.Now}");
                    return;
                }
                
                {
                    Debug.WriteLine($"Send Command to modbus (V27) {DateTime.Now}");
                    V27NextConfigTime = V27NextConfigTime.AddSeconds(V27configurationModes[V27ConfigIndex].Duration);
                    _V27MsgEvent?.Invoke(new (V27configurationModes[V27ConfigIndex],V27ConfigIndex));
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
                //    _V100MsgEvent?.Invoke(V100configurationModes[V100ConfigIndex]);
                    Debug.WriteLine($"V100 expirement Stop {DateTime.Now}");
                    return;
                }
                
                {
                    _V100MsgEvent?.Invoke(new CommandParams(V100configurationModes[V100ConfigIndex], V100ConfigIndex));
                    V100NextConfigTime = V100NextConfigTime.AddSeconds(V100configurationModes[V100ConfigIndex].Duration);
                    V100ConfigIndex++;
                }
            }

        }

        
        public double GetCurrentSecond()
        {
            var r = DateTime.Now.Day;
            return r;

        }


    }
}
