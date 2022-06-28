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

        private IModbusSerialMaster PowerSupplyModbusController;

        private IModbusSerialMaster V100ModbusController;
        private IModbusSerialMaster I100ModbusController;

        IModbusFactory _modbusFactory;
        /// <summary>
        /// Преобразователь напряжения шины 27В
        /// </summary>
        private SerialPort U27SerialPort;
        private SerialPort I27SerialPort;
        private SerialPort U100SerialPort;
        private SerialPort I100SerialPort;

        private SerialPort IChargerSerialPort;

        private SerialPort AkipSerialPort;

        private SerialPort ITCSerialPort;

        private SerialPort PowerSupplySerialPort;


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

            //Charger
            _statusBarViewModel.UpdateTaskProgress(12);
            try
            {

                ConnectStatus = true;
            } 
            catch (Exception ex)
            {
               
                ConnectStatus = false;
              
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к Juntek \n";

                }
               
            }

            //Akip Port
            _statusBarViewModel.UpdateTaskProgress(24);
            try
            {
          
                ConnectStatus = ConnectStatus && InitAkipPort();
            }
            catch (Exception ex)
            {
                ConnectStatus = false;
                ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к АКИП {ex.Message}";
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к АКИП \n";

                }
            }

            //ITC Port
            _statusBarViewModel.UpdateTaskProgress(36);
            try
            {
              
                ConnectStatus = ConnectStatus && InitITCPort();
            }
            catch (Exception ex)
            {
                ConnectionStatus =  $"Ошибка при подключении к ITC8515C+ {ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к ITC8516C+ \n";

                }
            }


            //V100 Bus Port
            _statusBarViewModel.UpdateTaskProgress(48);
            try
            {
              
                ConnectStatus = ConnectStatus && InitV100BusPort();
            }
            catch (Exception ex)
            {
                ConnectionStatus += $"Ошибка при подключении к преобразователю напряжения шины 100В{ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к преобразователю напряжения шины 100В\n";

                }
            }

            //I100 Bus Port
            _statusBarViewModel.UpdateTaskProgress(60);
            try
            {
               
                ConnectStatus = ConnectStatus && InitI100BusPort();
            }
            catch (Exception ex)
            {
                ConnectionStatus += $"Ошибка при подключении к преобразователю силы тока шины 100В {ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к преобразователю силы тока шины 100В\n";

                }
            }
            //V27 Bus Port
            _statusBarViewModel.UpdateTaskProgress(72);
            try
            {
               
                ConnectStatus = ConnectStatus && InitV27BusPort();
            }
            catch (Exception ex)
            {
                ConnectionStatus += $"Ошибка при подключении к преобразователю напряжения шины 27В { ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к преобразователю напряжения шины 27В\n";

                }
            }

            //I27 Bus Port
            _statusBarViewModel.UpdateTaskProgress(84);
            try
            {

                ConnectStatus = ConnectStatus && InitI27BusPort();
            }
            catch (Exception ex)
            {
                ConnectionStatus += $"Ошибка при подключении к преобразователю силы тока шины 27В { ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к преобразователю силы тока шины 27В \n";

                }
            }

            //Owen Controller
            _statusBarViewModel.UpdateTaskProgress(95);
            try
            {
               
                ConnectStatus = ConnectStatus && InitOwenController();
            }
            catch (Exception ex)
            {
                ConnectionStatus += $"Ошибка при подключении к ОВЕН {ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus += $"Ошибка при подключении к ОВЕН \n";

                }
            }

            _statusBarViewModel.UpdateTaskProgress(100);

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

        private float getValueByBytesResult(ushort[] result)
        {
            if (result == null) return -1;

            byte[] bytes = new byte[result.Length * sizeof(ushort)]; //4 byte или 32 бита
            byte[] Voltage = BitConverter.GetBytes(result[0]); //первые 8 битов или первый byte

            Buffer.BlockCopy(Voltage, 0, bytes, 0, Voltage.Length);
            Voltage = BitConverter.GetBytes(result[1]); //второй байт или вторые 8 битов
            Buffer.BlockCopy(Voltage, 0, bytes, 2, Voltage.Length);

            return BitConverter.ToSingle(bytes, 0);
        }

        public async Task<Single> Read27BusVoltage()
        {
            ushort[] result = new ushort[2];
            try
            {
                result =  V100ModbusController.ReadInputRegisters(1, 7, 2);
                Debug.WriteLine($" V27 Volt{result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);
        }


        public async Task<Single> Read27BusAmperage()
        {

            ushort[] result = new ushort[2];
            try
            {
                result =  V100ModbusController.ReadInputRegisters(1, 7, 2);
                Debug.WriteLine($" A27Amperage{result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);

            /*return await Task.Factory.StartNew(() =>
            {
                ushort[] result = new ushort[2];
                try
                {
                    result = V100ModbusController.ReadInputRegisters(1, 7, 2);
                    Debug.WriteLine($" A27Amperage{result} {DateTime.Now} {DateTime.Now.Millisecond}");
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
                return getValueByBytesResult(result);

            });
            */

        }


        public async Task<float[]> ReadPowerSupplyParams()
        {
          
           return await Task.Factory.StartNew(() =>
            {
                float[] result = new float[3];
                result[0] = -1;
                result[1] = -1;
                result[2] = -1;

                return result;
            });

            return null;
        }


        public async Task<Single> Read100BusVoltage()
        {

            ushort[] result = new ushort[2];
            try
            {
                result =  V100ModbusController.ReadInputRegisters(1, 7, 2);
                Debug.WriteLine($" V100 Volt{result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);


        }


        public async Task<Single> Read100BusAmperage()
        {
            ushort[] result = new ushort[2];
            try
            {
                result = V100ModbusController.ReadInputRegisters(1, 7, 2);
                Debug.WriteLine($" A100 {result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);

        }


        #region InitUSRVirtualPorts
        private bool InitV27BusPort()
        {
            if (V27ModbusController != null)
            {
                V27ModbusController.Transport.ReadTimeout = 750;
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
                U27SerialPort.PortName = "COM13";
                U27SerialPort.BaudRate = 9600;
                U27SerialPort.DataBits = 8;
                U27SerialPort.StopBits = StopBits.One;
                U27SerialPort.Open();

                serialPortAdapter = new SerialPortAdapter(U27SerialPort);

                V27ModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
                V27ModbusController.Transport.WriteTimeout = 1000;
                V27ModbusController.Transport.ReadTimeout = 750;

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
            if (I27ModbusController != null)
            {
                I27ModbusController.Transport.ReadTimeout = 750;
                I27ModbusController.Transport.WriteTimeout = 1000;
                Debug.WriteLine("*");


                if (I27ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            I27SerialPort = new SerialPort();
            {

                I27SerialPort.Close();
                I27SerialPort.PortName = "COM11";
                I27SerialPort.BaudRate = 9600;
                I27SerialPort.DataBits = 8;
                I27SerialPort.StopBits = StopBits.One;
                I27SerialPort.Open();

                serialPortAdapter = new SerialPortAdapter(I27SerialPort);

                I27ModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
                I27ModbusController.Transport.WriteTimeout = 1000;
                I27ModbusController.Transport.ReadTimeout = 750;

                if (I27ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    I27SerialPort.Close();
                    I27SerialPort.Dispose();
                    return false;
                }
            }

        }

        private bool InitICharger()
        {
            if (IChargerSerialPort != null)
            {
                IChargerSerialPort.Close();
            }
            IChargerSerialPort = new SerialPort()
            {
                BaudRate = 9600,
                PortName = "COM8",
                StopBits = StopBits.One


            };
            IChargerSerialPort.Close();
            IChargerSerialPort.WriteTimeout = 200;
            IChargerSerialPort.ReadTimeout = 200;
            IChargerSerialPort.Open();

            if (ReadICharger(IChargerSerialPort) != null)
            {

                return true;
            }
            else
            {
                IChargerSerialPort.Close();
                IChargerSerialPort.Dispose();
                return false;
            }


        }


        private bool InitV100BusPort()
        {
            if (V100ModbusController != null)
            {
                V100ModbusController.Transport.ReadTimeout = 750;
                V100ModbusController.Transport.WriteTimeout = 1000;
               

                if (V100ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            U100SerialPort = new SerialPort();
            {

                U100SerialPort.Close();
                U100SerialPort.PortName = "COM12";
                U100SerialPort.BaudRate = 9600;
                U100SerialPort.DataBits = 8;
                U100SerialPort.StopBits = StopBits.One;
                U100SerialPort.Open();

                serialPortAdapter = new SerialPortAdapter(U100SerialPort);

                V100ModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
                V100ModbusController.Transport.WriteTimeout = 1000;
                V100ModbusController.Transport.ReadTimeout = 750;

                if (V100ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    U100SerialPort.Close();
                    U100SerialPort.Dispose();
                    return false;
                }
            }




        }
        private bool InitI100BusPort()
        {
            if (I100ModbusController != null)
            {
                I100ModbusController.Transport.ReadTimeout = 750;
                I100ModbusController.Transport.WriteTimeout = 1000;
                


                if (I100ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            I100SerialPort = new SerialPort();
            {

                I100SerialPort.Close();
                I100SerialPort.PortName = "COM10";
                I100SerialPort.BaudRate = 9600;
                I100SerialPort.DataBits = 8;
                I100SerialPort.StopBits = StopBits.One;
                I100SerialPort.Open();

                serialPortAdapter = new SerialPortAdapter(I100SerialPort);

                I100ModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
                I100ModbusController.Transport.WriteTimeout = 500;
                I100ModbusController.Transport.ReadTimeout = 750;

                if (I100ModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    I100SerialPort.Close();
                    I100SerialPort.Dispose();
                    return false;
                }
            }
        }

        private bool InitPowerSupplyPort()
        {
            if (PowerSupplyModbusController != null)
            {
                PowerSupplyModbusController.Transport.ReadTimeout = 750;
                PowerSupplyModbusController.Transport.WriteTimeout = 500;
            


                if (PowerSupplyModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            PowerSupplySerialPort = new SerialPort();
            {

                PowerSupplySerialPort.Close();
                PowerSupplySerialPort.PortName = "*";
                PowerSupplySerialPort.BaudRate = 9600;
                PowerSupplySerialPort.DataBits = 8;
                PowerSupplySerialPort.StopBits = StopBits.One;
                PowerSupplySerialPort.Open();

                serialPortAdapter = new SerialPortAdapter(PowerSupplySerialPort);

                PowerSupplyModbusController = _modbusFactory.CreateRtuMaster(serialPortAdapter);
                PowerSupplyModbusController.Transport.WriteTimeout = 500;
                PowerSupplyModbusController.Transport.ReadTimeout = 750;

                if (PowerSupplyModbusController.ReadInputRegisters(1, 7, 2) != null)
                {
                    return true;
                }
                else
                {
                    PowerSupplySerialPort.Close();
                    PowerSupplySerialPort.Dispose();
                    return false;
                }
            }
        }
        #endregion


        private bool InitOwenController()
        {
            owenControllerTCPCLient?.Dispose();
            owenControllerTCPCLient = new TcpClient();
             owenControllerTCPCLient.Connect("192.168.0.15", 502);
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
                PortName = "COM4",
                StopBits = StopBits.One
             

            };
            ITCSerialPort.Close();
            ITCSerialPort.WriteTimeout = 200;
            ITCSerialPort.ReadTimeout = 200;
            ITCSerialPort.Open();

            if (ReadIDN(ITCSerialPort) != null)
            {
          
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
                PortName = "COM3",
                StopBits = StopBits.One

            };
            AkipSerialPort.Close();
            AkipSerialPort.WriteTimeout = 200;
            AkipSerialPort.ReadTimeout = 200;
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
            try
            {
                if (AkipSerialPort != null  && ITCSerialPort != null)
                {

                    AkipSerialPort.WriteLine($@"Power 0
                                        INPUT 0
                                        SYST:LOC
                                        ");
                    ITCSerialPort.WriteLine($@"Power 0
                                       INPUT 0
                                       SYST:LOC");

                }
             
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<float[]> ReadITCSerialPort()
        {
            float[] results = new float[3];
            try
            {
                lock (this)
                {
                    //TODO еще раз проверить параметры

                    ITCSerialPort.WriteLine($@"MEASURE:CURRENT?");
                    results[0] = Single.Parse(ITCSerialPort.ReadLine(), culture);

                    ITCSerialPort.WriteLine("MEASURE:VOLTAGE?");
                    results[1] = Single.Parse(ITCSerialPort.ReadLine(), culture);

                    ITCSerialPort.WriteLine("MEASURE:POWER?");
                    results[2] = Single.Parse(ITCSerialPort.ReadLine(), culture);

                }


            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);

                results[0] = -1;
                results[1] = -1;
                results[2] = -1;

            }

            return results;


        }
        public async Task<float[]> ReadAkipSerialPort()
        {
            float[] results = new float[3];
            try
            {
                lock (this)
                {
                    //TODO еще раз проверить параметры

                    AkipSerialPort.WriteLine("MEASURE:VOLTAGE?");
                    results[0] = Single.Parse(AkipSerialPort.ReadLine(), culture);  //3

                    AkipSerialPort.WriteLine($@"MEASURE:CURRENT?");
                    results[1] = Single.Parse(AkipSerialPort.ReadLine(), culture); //4

                    AkipSerialPort.WriteLine("MEASURE:POWER?");
                    results[2] = Single.Parse(AkipSerialPort.ReadLine(), culture); //5

                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);

                results[0] = -1;
                results[1] = -1;
                results[2] = -1;

            }

            return results;


        }

        private string ReadICharger(SerialPort port)
        {
            
            port.WriteLine(@":01r33=0");
            string r = port.ReadLine();
            Debug.WriteLine(r);
            return r;
        }

        private string ReadIDN(SerialPort port)
        {
            port.WriteLine(@"
                *CLS
                *IDN?");
            return port.ReadLine();
            
        }
        public async Task<bool> SetAKIPPowerValue(double value)
        {
            bool result = false;
           return await Task.Run(() =>
            {
                lock (this)
                {
                    if (AkipSerialPort != null)
                    {
                        AkipSerialPort.Close();
                        AkipSerialPort.Open();
                        string query = ($@"SYSTEM:REM
                            Mode Power
                            INPUT 1
                            Power {value.ToString("G2", culture)}
                            Power?");
                        AkipSerialPort.WriteLine(query);
                        AkipSerialPort.Close();
                        AkipSerialPort.Open();
                        result = true;
                     
                    }
                    else
                    {
                        result = false;
                    }

                }
                return result;
            });
           
        }
        public async Task<bool> SetITCPowerValue(double value)
        {
            bool result = false;    
          return  await Task.Run(() =>
            {
                lock (this)
                {
                    if (ITCSerialPort != null)
                    {
                        ITCSerialPort.Close();
                        ITCSerialPort.Open();
                        string query = ($@"SYSTEM:REM
                            Mode Power
                            INPut 1
                            Power {value.ToString("G2", culture)}
                            Power?");

                        ITCSerialPort.WriteLine(query);
                        ITCSerialPort.Close();
                        ITCSerialPort.Open();
                        result = true;
                      
                    }

                    else
                    {
                        result = false;
                    }

                }
                return result;
            });
            
        }


        public async Task<bool> SetIchargerValue(string value)
        {
            bool result = false;
            return await Task.Run(() =>
            {
                lock (this)
                {
                    if (IChargerSerialPort != null)
                    {
                        IChargerSerialPort.Close();
                        IChargerSerialPort.Open();
                        string query = ($"01w11={value}");

                        IChargerSerialPort.WriteLine(query);
                        IChargerSerialPort.Close();
                        IChargerSerialPort.Open();
                        result = true;
                        Debug.WriteLine(query);

                    }

                    else
                    {
                        result = false;
                    }

                }
                return result;
            });



           
        }

        #endregion

    }
}
