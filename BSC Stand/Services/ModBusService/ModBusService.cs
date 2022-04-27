using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NModbus;
using NModbus.Device;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace BSC_Stand.Services
{
    internal class ModBusService: IModbusService
    {
        private readonly IModbusMaster owenController;

        private readonly TcpClient owenControllerTCPCLient;


        public ModBusService()
        {
            IModbusFactory modbusFactory = new ModbusFactory();
            try
            {
                owenControllerTCPCLient = new TcpClient("10.0.6.10", 502);
               
                owenController = modbusFactory.CreateMaster(owenControllerTCPCLient);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Не удалось подключиться к ОВЕН");

            }
        }

        public async Task<ushort[]> ReadDataFromOwenController()
        {
            return null;

          //  return await owenController.ReadHoldingRegistersAsync(1, 0, 2);
          
        }
        public bool GetOwenConnectionStatus()
        {
            return owenControllerTCPCLient.Connected;
        }

       
    }
}
