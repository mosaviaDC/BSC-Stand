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
using NModbus.IO;
using System.Globalization;

namespace BSC_Stand.Services
{
    internal class ModBusService : IModbusService
    {
        private IModbusMaster owenController;
        private TcpClient owenControllerTCPCLient;
        private SerialPortAdapter serialPortAdapter;
        private IModbusSerialMaster V27ModbusController;
        private IModbusSerialMaster I27ModbusController;


        IModbusFactory _modbusFactory;

        private SerialPort U27SerialPort;
        private SerialPort I27SerialPort;
        private SerialPort U100SerialPort;
        private SerialPort I100SerialPort;

        private SerialPort AkipSerialPort;

        private SerialPort ITCSerialPort;


        private StatusBarViewModel _statusBarViewModel;
        private readonly CultureInfo culture;
        private bool ConnectStatus = false;
        private bool isBusy = false;

        public ModBusService(StatusBarViewModel statusBarViewModel)
        {
            _statusBarViewModel = statusBarViewModel;
            culture = new CultureInfo("en-Us");
        }

        public (string, bool) InitConnections()
        {
            isBusy = true;

            string ConnectionStatus = "";
            _statusBarViewModel.SetNewTask(100);
            _modbusFactory = new ModbusFactory();
            ConnectStatus = false;
            try
            {
                _statusBarViewModel.UpdateTaskProgress(50);
                ConnectStatus = InitAkipPort() && InitITCPort(); /*InitV27BusPort() && InitI27BusPort() &&*/ //InitITCPort();
                _statusBarViewModel.UpdateTaskProgress(100);
            }
            catch (Exception ex)
            {
                _statusBarViewModel.UpdateTaskProgress(100);
                ConnectionStatus += $"{ex.Message}";
                isBusy = false;
            }
            isBusy = false;
            return (ConnectionStatus, ConnectStatus);


        }




        public async Task<ushort[]> ReadDataFromOwenController()
        {

            return null;
        }

        public bool GetConnectStatus()
        {
            return this.ConnectStatus;
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

        public async Task<Single> Read27BusVoltage()
        {
            try
            {
                ushort[] result = new ushort[2];

                result = await V27ModbusController.ReadInputRegistersAsync(1, 7, 2);

                if (result != null)
                {
                    byte[] bytes = new byte[result.Length * sizeof(ushort)]; //4 byte или 32 бита

                    byte[] Voltage = BitConverter.GetBytes(result[0]); //первые 8 битов или первый byte
                    Buffer.BlockCopy(Voltage, 0, bytes, 0, Voltage.Length);
                    Voltage = BitConverter.GetBytes(result[1]); //второй байт или вторые 8 битов
                    Buffer.BlockCopy(Voltage, 0, bytes, 2, Voltage.Length);

                    return BitConverter.ToSingle(bytes, 0);
                }
                else return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }




        }


        /// <summary>
        /// Инициализация порта для преобразователя напряжения шины 27В
        /// </summary>
        private bool InitV27BusPort()
        {
            if (V27ModbusController != null)
            {
                V27ModbusController.Transport.ReadTimeout = 1500;
                V27ModbusController.Transport.WriteTimeout = 1000;
                Debug.WriteLine("*");


                if (V27ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            U27SerialPort = new SerialPort();
            {

                U27SerialPort.Close();
                U27SerialPort.PortName = "COM1";
                U27SerialPort.BaudRate = 9600;
                U27SerialPort.DataBits = 8;
                U27SerialPort.StopBits = StopBits.One;
                U27SerialPort.Open();

                serialPortAdapter = new SerialPortAdapter(U27SerialPort);

                V27ModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
                V27ModbusController.Transport.WriteTimeout = 1000;
                V27ModbusController.Transport.ReadTimeout = 1500;

                if (V27ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    U27SerialPort.Close();
                    U27SerialPort.Dispose();
                    return false;
                }
            }


        }

        private bool InitI27BusPort()
        {
            //if (I27SerialPort == null)
            //    I27SerialPort = new SerialPort();
            //I27SerialPort.Close();
            //I27SerialPort.PortName = "COM2";
            //I27SerialPort.BaudRate = 9600;
            //I27SerialPort.DataBits = 8;
            //I27SerialPort.StopBits = StopBits.One;
            //I27SerialPort.Open();

            //serialPortAdapter = new SerialPortAdapter(I27SerialPort);

            //I27ModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
            //I27ModbusController.Transport.ReadTimeout = 100;
            //if (I27ModbusController.ReadInputRegisters(1, 7, 2) != null)
            //{
            //    return true;
            //}
            //else
            //{
            //    I27SerialPort.Close();
            //    I27SerialPort.Dispose();
            //    return t;
            //}
            return true;

        }

        private async Task<bool> InitOwenController()
        {
            owenControllerTCPCLient?.Dispose();
            owenControllerTCPCLient = new TcpClient();
            await owenControllerTCPCLient.ConnectAsync("10.0.6.10", 502);
            _statusBarViewModel.UpdateTaskProgress(75);
            owenController = _modbusFactory.CreateMaster(owenControllerTCPCLient);
            return owenControllerTCPCLient.Connected;
        }


        private bool InitITCPort()
        {
            if (ITCSerialPort != null)
            {
                ITCSerialPort.Close();
            }
            ITCSerialPort = new SerialPort()
            {
                BaudRate = 9600,
                PortName = "COM3",
                StopBits = StopBits.One,


            };
            ITCSerialPort.Close();
            ITCSerialPort.WriteTimeout = 2000;
            ITCSerialPort.ReadTimeout = 2000;
            ITCSerialPort.Open();

            if (ReadIDN(ITCSerialPort) != null)
            {
                Debug.WriteLine(ReadIDN(ITCSerialPort));
                return true;
            }
            else
            {
                ITCSerialPort.Close();
                ITCSerialPort.Dispose();
                return false;
            }
        }
        private bool InitAkipPort()
        {
            if (AkipSerialPort != null)
            {
                AkipSerialPort.Close();
            }

            AkipSerialPort = new SerialPort()
            {
                BaudRate = 9600,
                PortName = "COM4",
                StopBits = StopBits.One,


            };
            AkipSerialPort.Close();
            AkipSerialPort.WriteTimeout = 2000;
            AkipSerialPort.ReadTimeout = 2000;
            AkipSerialPort.Open();


            if (ReadIDN(AkipSerialPort) != null)
            {
              
                return true;
            }
            else
            {
                AkipSerialPort.Close();
                AkipSerialPort.Dispose();
                return false;
            }
        }








        #region SCPI region

        public void ExitCommand()
        {
            AkipSerialPort.WriteLine($@"Power 0
                                        INPUT 0
                                        SYST:LOC
                                        ");
            ITCSerialPort.WriteLine($@"Power 0
                                       INPUT 0
                                       SYST:LOC");
        }
       
        private string ReadIDN(SerialPort port)
        {
            port.WriteLine(@"
                *CLS
                *IDN?");
            return port.ReadLine();
        }


        public bool SetAKIPPowerValue(double value)
        {
            if (AkipSerialPort != null)
            {
                
                string query = ($@"SYSTEM:REM
                            Mode Power
                            INPUT 1
                            Power {value.ToString("G2",culture)}
                            Power?");
                AkipSerialPort.WriteLine(query);
                Debug.WriteLine(AkipSerialPort.ReadLine());
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<float[]> ReadElectroninLoadParams()
        {
            float[] results = new float[6];
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    ITCSerialPort.WriteLine($@"MEASURE:CURRENT?"); 
                    results[0]= Single.Parse(ITCSerialPort.ReadLine(), culture);//Current Voltage

                    ITCSerialPort.WriteLine("MEASURE:VOLTAGE?");
                    results[1] = Single.Parse(ITCSerialPort.ReadLine(), culture); //Current Power
                    
                    ITCSerialPort.WriteLine("MEASURE:POWER?");
                    results[2] = Single.Parse(ITCSerialPort.ReadLine(), culture); //Current Amperage

                    Debug.WriteLine($"ITC {results[0]} W:{results[1]} A:{results[2]}");

                    AkipSerialPort.WriteLine($@"MEASURE:CURRENT?");
                    results[3] = Single.Parse(AkipSerialPort.ReadLine(), culture);//Current Amperage

                    AkipSerialPort.WriteLine("MEASURE:VOLTAGE?");
                    results[4] = Single.Parse(AkipSerialPort.ReadLine(), culture); //Current Power

                    AkipSerialPort.WriteLine("MEASURE:POWER?");
                    results[5] = Single.Parse(AkipSerialPort.ReadLine(), culture); //Current Amperage

                    Debug.WriteLine($"AKIP A:{results[3]} V:{results[4]} W:{results[5]}");

                }
                catch
                {

                }
            });
            return null;

        }


        public bool SetITCPowerValue (double value)
        {
            if (ITCSerialPort != null)
            {
             
              
                string query = ($@"SYSTEM:REM
                            Mode Power
                            INPut 1
                            Power {value.ToString("G2",culture)}
                            Power?");
    
                ITCSerialPort.WriteLine(query);
             
                return true;
            }
            else
            {
                return false;
            }
        
        }

        #endregion

    }
}
