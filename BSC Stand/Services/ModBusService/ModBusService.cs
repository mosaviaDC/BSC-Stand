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
using System.IO.Ports;
using NModbus.Serial;

namespace BSC_Stand.Services
{
    internal class ModBusService: IModbusService
    {
        private  IModbusMaster owenController;
        private  TcpClient owenControllerTCPCLient;
        private  IModbusSerialMaster V27ModbusController;
        IModbusFactory _modbusFactory;

        private SerialPort U27SerialPort;
        private SerialPort I27SerialPort;
        private SerialPort U100SerialPort;
        private SerialPort I100SerialPort;

        private StatusBarViewModel _statusBarViewModel;
        

        private bool isBusy = false;

        public  ModBusService(StatusBarViewModel statusBarViewModel)
        {
            _statusBarViewModel = statusBarViewModel;
          
        }


        public async Task<(string,bool)> InitConnections()
        {
                isBusy = true;
                _statusBarViewModel.SetNewTask(100);
                _modbusFactory = new ModbusFactory();
                owenControllerTCPCLient?.Dispose();
                _statusBarViewModel.UpdateTaskProgress(25);
                owenControllerTCPCLient = new TcpClient();
     
                 string ConnectionStatus = "";
     
            try
            {
                InitSerialPorts();
                _statusBarViewModel.UpdateTaskProgress(50);
               // await owenControllerTCPCLient.ConnectAsync("10.0.6.10", 502);
        
                _statusBarViewModel.UpdateTaskProgress(75);
               // owenController = _modbusFactory.CreateMaster(owenControllerTCPCLient);
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
       
        public Single Read27BusVoltage()
        {
                var result =  V27ModbusController.ReadInputRegisters(1, 7, 2); //Почему не работает в async&
                if (result != null)
                {
                    byte[] bytes = new byte[result.Length * sizeof(ushort)]; //4 byte или 32 бита

                    byte[] Voltage = BitConverter.GetBytes(result[0]); //первые 8 битов или первый byte
                    Buffer.BlockCopy(Voltage, 0, bytes, 0, Voltage.Length);

                    Voltage = BitConverter.GetBytes(result[1]); //второй байт или вторые 8 битов
                    Buffer.BlockCopy(Voltage, 0, bytes, 2, Voltage.Length);
                    // Debug.WriteLine(BitConverter.ToSingle(bytes, 0));
                    return BitConverter.ToSingle(bytes, 0);
                    //   return 0;
                }
                else return 0;
        }


        private void InitSerialPorts()
        {
            U27SerialPort = new SerialPort();
            U27SerialPort.PortName = "COM1";
            U27SerialPort.BaudRate = 9600;
            U27SerialPort.DataBits = 8;
            U27SerialPort.StopBits = StopBits.One;
            U27SerialPort.Open();
            SerialPortAdapter serialPortAdapter = new SerialPortAdapter(U27SerialPort);
            V27ModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
            
        }

    }
}
