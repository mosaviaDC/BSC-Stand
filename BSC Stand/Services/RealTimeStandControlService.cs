using BSC_Stand.Models;
using BSC_Stand.Models.StandConfigurationModels;
using BSC_Stand.Models.StandConfigurationModels;
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

        private static DispatcherTimer PowerSupplyExpirementTimer { get; set; }
        private DateTime StartTime { get;set; }

        private DateTime V27NextConfigTime { get; set; }

        private int V27ConfigIndex = 0;

        private DateTime V100NextConfigTime { get; set; }

        private int V100ConfigIndex = 0;

        private DateTime PowerSupplyNextConfigTime { get; set; }

        private int PowerSupplyConfigIndex = 0;

        private int ExperimentDurationCount;

        private double ExperimentDuration;


        private int PowerSupplyExpRepeatCount { get; set; }

        private int V27ExpRepeatCount { get; set; }

        private int V100ExpRepeatCount { get; set; }

        private ObservableCollection<ElectronicConfigMode> V27configurationModes;

        private ObservableCollection<ElectronicConfigMode> V100configurationModes;

        private ObservableCollection<PowerSupplyConfigMode> PowerSupplyConfigModes;

        private readonly IUserDialogWindowService _userDialogWindowService;

        private readonly StandConfigurationViewModel _standConfigurationViewModel;

        private bool isExpirementPepformed;
        private List<ElectronicConfigMode> V27tempModes;
        private List<ElectronicConfigMode> V100tempModes;
        private List<PowerSupplyConfigMode> PowerSupplyTempModes;

        public delegate void V27Msg(CommandParams commandParams);
        public event V27Msg _V27MsgEvent;

        public delegate void V100Msg(CommandParams commandParamse);
        public event V100Msg _V100MsgEvent;


        public delegate void PowerSupplyMsg(CommandParams commandParamse);
        public event PowerSupplyMsg _PowerSupplyMsgEvent;

        public RealTimeStandControlService(BSCControlViewModel bSCControlViewModel, StandConfigurationViewModel standConfigurationViewModel, IUserDialogWindowService userDialogWindowService)
        {
            _userDialogWindowService = userDialogWindowService;
            _standConfigurationViewModel = standConfigurationViewModel;
            isExpirementPepformed = false;
            V27expirementTimer = new DispatcherTimer();
            V27expirementTimer.Interval = TimeSpan.FromMilliseconds(250);
            V27expirementTimer.Tick += V27TimerEventHandler;

            V100expirementTimer = new DispatcherTimer();
            V100expirementTimer.Interval = TimeSpan.FromMilliseconds(250);
            V100expirementTimer.Tick += V100TimerEventHandler;

            PowerSupplyExpirementTimer = new DispatcherTimer();
            PowerSupplyExpirementTimer.Interval = TimeSpan.FromMilliseconds(250);
            PowerSupplyExpirementTimer.Tick += PowerSupplyEventHandler;


            V27configurationModes = standConfigurationViewModel.Bus27ConfigurationModes;
            V100configurationModes = standConfigurationViewModel.Bus100ConfigurationModes;
            PowerSupplyConfigModes = standConfigurationViewModel.PowerSupplyConfigurationModes;
            PowerSupplyExpRepeatCount = standConfigurationViewModel.PowerSupplyCyclogramRepeatCount;
            V27ExpRepeatCount = standConfigurationViewModel.V27BusCyclogramRepeatCount;
            V100ExpRepeatCount = standConfigurationViewModel.V100BusCyclogramRepeatCount;


            _V27MsgEvent += bSCControlViewModel.SendV27ModBusCommand;
            _V100MsgEvent += bSCControlViewModel.SendV100ModBusCommand;
            _PowerSupplyMsgEvent += bSCControlViewModel.SendPowerSupplyCommand;
   
          

        }
        /// <summary>
        /// Обновление параметров (пересчет duration)
        /// </summary>
        public void UpdateExpiremntParams()
        {

            if (V100configurationModes.Count > 0 || V27configurationModes.Count > 0 || PowerSupplyConfigModes.Count > 0)
            {
                V27ExpRepeatCount = _standConfigurationViewModel.V27BusCyclogramRepeatCount;
                V100ExpRepeatCount = _standConfigurationViewModel.V100BusCyclogramRepeatCount;
                PowerSupplyExpRepeatCount = _standConfigurationViewModel.PowerSupplyCyclogramRepeatCount;


                StartTime = DateTime.Now;

                V27NextConfigTime = StartTime;
                V27NextConfigTime = V27NextConfigTime.AddSeconds(V27configurationModes[V27ConfigIndex].Duration);
                V27ConfigIndex++;

                V100NextConfigTime = StartTime;
                V100NextConfigTime = V100NextConfigTime.AddSeconds(V100configurationModes[V100ConfigIndex].Duration);
                V100ConfigIndex++;

                PowerSupplyNextConfigTime = StartTime;
                PowerSupplyNextConfigTime = PowerSupplyNextConfigTime.AddSeconds(PowerSupplyConfigModes[PowerSupplyConfigIndex].Duration);
                PowerSupplyConfigIndex++;

                _V27MsgEvent?.Invoke(new CommandParams(V27configurationModes[0], 0, false));
                _V100MsgEvent?.Invoke(new CommandParams(V100configurationModes[0], 0, false));
                _PowerSupplyMsgEvent?.Invoke(new CommandParams(PowerSupplyConfigModes[0], 0, false));
                //TO DO расчитать для трех



                //ExperimentDurationCount = V100configurationModes.Count > V27configurationModes.Count ? V100configurationModes.Count : V27configurationModes.Count;

                //ExperimentDurationCount = ExperimentDurationCount > PowerSupplyConfigModes.Count ? ExperimentDurationCount : PowerSupplyConfigModes.Count;

                //Max Duration of Experiment

                double ExperimentDuration1 = 0;
                double ExperimentDuration2 = 0;
                double ExperimentDuration3 = 0;

             
                V27tempModes = V27configurationModes.ToList();
                
                for (int i = 1; i < V27ExpRepeatCount; i++)
                {
                
                    foreach (var p in V27tempModes)
                    {
                        V27configurationModes.Add(p);
                    }
                

                }

                V100tempModes = V100configurationModes.ToList();

                for (int i = 1; i < V100ExpRepeatCount; i++)
                {

                    foreach (var p in V100tempModes)
                    {
                       V100configurationModes.Add(p);
                    }
                   

                }

                PowerSupplyTempModes = PowerSupplyConfigModes.ToList();

                for (int i = 1; i < PowerSupplyExpRepeatCount; i++)
                {

                    foreach (var p in PowerSupplyTempModes)
                    {
                       PowerSupplyConfigModes.Add(p);
                    }


                }
               
                foreach (ElectronicConfigMode mode in V27configurationModes)
                {
                    ExperimentDuration1 += mode.Duration;
                }

                foreach (ElectronicConfigMode mode in V100configurationModes)
                {
                    ExperimentDuration2 += mode.Duration;
                }

                foreach (PowerSupplyConfigMode mode in PowerSupplyConfigModes)
                {
                    ExperimentDuration3 += mode.Duration;
                }

                ExperimentDuration = Math.Max(ExperimentDuration1, Math.Max(ExperimentDuration2, ExperimentDuration3));


                V27expirementTimer.Start();
                V100expirementTimer.Start();
                PowerSupplyExpirementTimer.Start();
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
                V27ConfigIndex = 0;
                V100ConfigIndex = 0;
                PowerSupplyConfigIndex=0;
                UpdateExpiremntParams();
            }
            else if (isExpirementPepformed)
            {
                _userDialogWindowService.ShowErrorMessage("Эксперимент уже запущен");
            }
          
        }
        public void StopExpirement()
        {
            isExpirementPepformed = false;
            V27expirementTimer.Stop();
            V100expirementTimer.Stop();
            PowerSupplyExpirementTimer.Stop();
            V27configurationModes.Clear();
            foreach(var p in V27tempModes)
            {
                V27configurationModes.Add(p);
            }
            V100configurationModes.Clear();
            foreach (var p in V100tempModes)
            {
                V100configurationModes.Add(p);
            }
            PowerSupplyConfigModes.Clear();
            foreach(var p in PowerSupplyTempModes)
            {
                PowerSupplyConfigModes.Add(p);
            }

        }


        public void V27TimerEventHandler(object sender, EventArgs e)
        {

            //if (DateTime.Now - V27NextConfigTime >= TimeSpan.FromMilliseconds(0) && (DateTime.Now - V27NextConfigTime <= TimeSpan.FromMilliseconds(500)))
            if (DateTime.Now >= V27NextConfigTime)
            {
                if (V27ConfigIndex == V27configurationModes.Count)
                {
                    
                    V27expirementTimer.Stop();
                 //   _V27MsgEvent?.Invoke(new  (V27configurationModes[V27ConfigIndex-1],V27ConfigIndex,true));
                     if (DateTime.Now - StartTime >= TimeSpan.FromSeconds(ExperimentDuration))
                     {
                        _V27MsgEvent?.Invoke(new(V27configurationModes[V27ConfigIndex - 1], V27ConfigIndex, true));
                     }
                    Debug.WriteLine($"V27 expirement Stop {DateTime.Now}");
                    return;
                }
                
                {
                    Debug.WriteLine($"Send Command to modbus (V27) {DateTime.Now}");
                    V27NextConfigTime = V27NextConfigTime.AddSeconds(V27configurationModes[V27ConfigIndex].Duration);
                    _V27MsgEvent?.Invoke(new (V27configurationModes[V27ConfigIndex],V27ConfigIndex,false));
                    V27ConfigIndex++;
                }
            }

        }

        public void V100TimerEventHandler(object sender, EventArgs e)
        {

            //if (DateTime.Now - V100NextConfigTime >= TimeSpan.FromMilliseconds(0) && (DateTime.Now - V100NextConfigTime <= TimeSpan.FromMilliseconds(500)))
            if (DateTime.Now >= V100NextConfigTime)
            {

                if (V100ConfigIndex == V100configurationModes.Count)
                {
                    V100expirementTimer.Stop();
                    //if (V100ConfigIndex == ExperimentDurationCount)
                    if (DateTime.Now - StartTime >= TimeSpan.FromSeconds(ExperimentDuration))
                    {
                        _V100MsgEvent?.Invoke(new(V100configurationModes[V100ConfigIndex - 1], V100ConfigIndex, true));

                    }
                    Debug.WriteLine($"V100 expirement Stop {DateTime.Now}");
                    return;
                }
                
                {
                    _V100MsgEvent?.Invoke(new CommandParams(V100configurationModes[V100ConfigIndex], V100ConfigIndex,false));
                    V100NextConfigTime = V100NextConfigTime.AddSeconds(V100configurationModes[V100ConfigIndex].Duration);
                    V100ConfigIndex++;
                }
            }

        }


        public void PowerSupplyEventHandler(object sender, EventArgs e)
        {

            //if (DateTime.Now - PowerSupplyNextConfigTime >= TimeSpan.FromMilliseconds(0) && (DateTime.Now - PowerSupplyNextConfigTime <= TimeSpan.FromMilliseconds(500)))
            if (DateTime.Now >= PowerSupplyNextConfigTime)
            {

                if (PowerSupplyConfigIndex == PowerSupplyConfigModes.Count)
                {
                    PowerSupplyExpirementTimer.Stop();
                    //if (PowerSupplyConfigIndex == ExperimentDurationCount)
                    if (DateTime.Now - StartTime >= TimeSpan.FromSeconds(ExperimentDuration))
                    {
                        _PowerSupplyMsgEvent?.Invoke(new(PowerSupplyConfigModes[PowerSupplyConfigIndex - 1], PowerSupplyConfigIndex, true));

                    }
                    Debug.WriteLine($"Power Supply expirement Stop {DateTime.Now}");
                    return;
                }

                {
                    _PowerSupplyMsgEvent?.Invoke(new CommandParams(PowerSupplyConfigModes[PowerSupplyConfigIndex], PowerSupplyConfigIndex,false));
                    PowerSupplyNextConfigTime = PowerSupplyNextConfigTime.AddSeconds(PowerSupplyConfigModes[PowerSupplyConfigIndex].Duration);
                    PowerSupplyConfigIndex++;
                }
            }

        }








        public double GetCurrentSecond()
        {
            var r = DateTime.Now.Day;
            return r;

        }

        public bool GetExperimentStatus()
        {
            return isExpirementPepformed;
        }


    }
}
