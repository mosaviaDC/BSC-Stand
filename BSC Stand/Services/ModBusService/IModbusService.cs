﻿using NModbus;
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
        public  Task<(string, bool)> InitConnections();
        public bool GetBusyStatus();
        public Single Read27BusVoltage();
    }
}
