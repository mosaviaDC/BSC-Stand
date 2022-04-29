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
using BSC_Stand.ViewModels;

namespace BSC_Stand.Services
{
    internal class ModBusService: IModbusService
    {
        private  IModbusMaster owenController;

        private  TcpClient owenControllerTCPCLient;
        private StatusBarViewModel _statusBarViewModel;

        private bool isBusy = false;

        public ModBusService(StatusBarViewModel statusBarViewModel)
        {
            _statusBarViewModel = statusBarViewModel;
         
        }


        public async Task<(string,bool)> InitConnections()
        {
            
                isBusy = true;
                 _statusBarViewModel.SetNewTask(100);
                IModbusFactory modbusFactory = new ModbusFactory();
                owenControllerTCPCLient?.Dispose();
                _statusBarViewModel.UpdateTaskProgress(25);
                 owenControllerTCPCLient = new TcpClient();
                 string ConnectionStatus = "";
     
            try
            {
                await owenControllerTCPCLient.ConnectAsync("10.0.6.10", 502);
                _statusBarViewModel.UpdateTaskProgress(75);
                owenController = modbusFactory.CreateMaster(owenControllerTCPCLient);
                _statusBarViewModel.UpdateTaskProgress(100);
                isBusy = false;
         
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _statusBarViewModel.UpdateTaskProgress(100);
                ConnectionStatus = $"{ex.Message}";
                isBusy = false;
             

            }
            return (ConnectionStatus, owenControllerTCPCLient.Connected);




        }

        public async Task<ushort[]> ReadDataFromOwenController()
        {
            if (owenControllerTCPCLient.Connected)
            {
                 return await owenController.ReadHoldingRegistersAsync(1, 0, 2);
            }

            return null;
        }
        public bool GetOwenConnectionStatus()
        {
            if (owenControllerTCPCLient != null)
            return owenControllerTCPCLient.Connected;
          else return false;
        }

        public bool GetBusyStatus()
        {
            return isBusy;
        }
       
    }
}
