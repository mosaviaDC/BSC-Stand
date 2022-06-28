using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services
{
    internal interface IModbusService
    {
        public Task<ushort[]> ReadDataFromOwenController();
        public bool GetOwenConnectionStatus();
        public  (string, bool) InitConnections();
        public bool GetConnectStatus();
        public bool GetBusyStatus();
        public Task<Single> Read27BusVoltage();
        public Task<Single> Read27BusAmperage();

        public Task<Single> Read100BusVoltage();
        public Task<Single> Read100BusAmperage();

        public Task<bool> SetAKIPPowerValue(double value);
        public Task<bool> SetITCPowerValue(double value);
        public Task<bool> SetIchargerValue(string value);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// [0] = ITC Voltage (V)
        /// [1] = ITC Power (W)
        /// [2] = ITC Amperage (A)
        /// [3] = Akip Power (W)
        /// [4] = Akip Amperage (A)
        /// [5] = Akip Voltage (V)
        /// </returns>
        public Task<float[]> ReadITCSerialPort();

        public Task<float[]> ReadAkipSerialPort();

        public Task<float[]> ReadPowerSupplyParams();
        public void ExitCommand();
        }
    }

