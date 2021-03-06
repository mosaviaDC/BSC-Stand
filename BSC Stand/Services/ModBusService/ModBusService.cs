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
using System.Configuration;

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

        private const int OWEN_PORT = 502;
        private const string OWEN_IP = "192.168.0.15";
        private string ITC_PORT_NAME = ConfigurationManager.AppSettings["ITC_PORT_NAME"];
        private string AKIP_PORT_NAME = ConfigurationManager.AppSettings["AKIP_PORT_NAME"];
        private string ICharger_PORT_NAME = ConfigurationManager.AppSettings["ICharger_PORT_NAME"];
        private float TempKoef;
        public ModBusService(StatusBarViewModel statusBarViewModel)
        {
            _statusBarViewModel = statusBarViewModel;
            culture = new CultureInfo("en-Us");


            TempKoef = Single.Parse(ConfigurationManager.AppSettings["TEMPKOEF"], CultureInfo.InvariantCulture);

        }

        public (string, bool) InitConnections()
        {
            isBusy = true;

            string ConnectionStatus = $" ";
            _statusBarViewModel.SetNewTask(9, "Проверка подключения");
            _modbusFactory = new ModbusFactory();
            ConnectStatus = false;

            //Charger
            _statusBarViewModel.UpdateTaskProgress(1);
            try
            {

                ConnectStatus = InitICharger();
            }
            catch (Exception ex)
            {
                ConnectStatus = false;

            }
            finally
            {
                if (!ConnectStatus)
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к понижающему конвертуру \n";
            }

            //Akip Port
            _statusBarViewModel.UpdateTaskProgress(2);
            try
            {
                ConnectStatus = ConnectStatus && InitAkipPort();
            }
            catch (Exception ex)
            {
                ConnectStatus = false;

            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к АКИП-1381\n";
                }
            }


            //ITC Port
            _statusBarViewModel.UpdateTaskProgress(3);
            try
            {

                ConnectStatus = ConnectStatus && InitITCPort();
            }
            catch (Exception ex)
            {
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к ITC8515C\n";
                }
            }




            //I100 Bus Port
            _statusBarViewModel.UpdateTaskProgress(5);
            try
            {

                ConnectStatus = ConnectStatus && InitI100BusPort();
            }
            catch (Exception ex)
            {

                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к измерителю силы тока шины 100В\n";
                }
            }

            ////V27 Bus Port
            _statusBarViewModel.UpdateTaskProgress(6);
            try
            {

                ConnectStatus = ConnectStatus && InitV27BusPort();
            }
            catch (Exception ex)
            {
                //   ConnectionStatus += $"Ошибка при подключении к преобразователю напряжения шины 27В { ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к измерителю напряжения шины 27В\n";
                }
            }


            //I27 Bus Port
            _statusBarViewModel.UpdateTaskProgress(7);
            try
            {

                ConnectStatus = ConnectStatus && InitI27BusPort();
            }
            catch (Exception ex)
            {
                // ConnectionStatus += $"Ошибка при подключении к преобразователю силы тока шины 27В { ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к измерителю напряжения шины 27В\n";
                }
            }


            //Owen Controller
            _statusBarViewModel.UpdateTaskProgress(8);
            try
            {

                ConnectStatus = ConnectStatus && InitOwenController();
            }
            catch (Exception ex)
            {
                // ConnectionStatus += $"Ошибка при подключении к ОВЕН {ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к ПЛК ОВЕН\n";
                }
            }


            _statusBarViewModel.UpdateTaskProgress(8);
            try
            {

                ConnectStatus = ConnectStatus && InitV100BusPort();
            }
            catch (Exception ex)
            {
                //ConnectionStatus += $"Ошибка при подключении к преобразователю напряжения шины 100В{ex.Message}";
                ConnectStatus = false;
            }
            finally
            {
                if (!ConnectStatus)
                {
                    ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к измерителю напряжения шины 100В\n";
                }
            }

            //PowerSupplyPower
            //try
            //{

            //    ConnectStatus = ConnectStatus && InitPowerSupplyPort();
            //}
            //catch (Exception ex)
            //{
            //    //ConnectionStatus += $"Ошибка при подключении к источнику питания{ex.Message}";
            //    ConnectStatus = false;
            //}
            //finally
            //{
            //    if (!ConnectStatus)
            //    {
            //        ConnectionStatus = ConnectionStatus + $"Ошибка при подключении к источнику питания\n";
            //    }
            //}








            _statusBarViewModel.UpdateTaskProgress(9);

            isBusy = false;
            return (ConnectionStatus, ConnectStatus);
        }


        public float[] ReadDataFromOwenController()
        {
            // var result = new ushort[4];
            // var res1 = new ushort[2]
            // {
            //       result[0],
            //       result[1],
            // }
            // ;
            // var res2 = new ushort[2]
            //{
            //       result[2],
            //       result[3]

            //};
            // try
            // {

            //     result = owenController.ReadHoldingRegisters(1, 0, 4);
            //     res1 = new ushort[2]
            //    {
            //       result[0],
            //       result[1],
            //    }
            //    ;
            //     res2 = new ushort[2]
            //   {
            //       result[2],
            //       result[3],

            //   };
            // }
            // catch (Exception ex)
            // {

            //     Debug.WriteLine(ex.Message);
            // }


            // return new float[]
            // {
            //     /// 0.83 = 300/
            //     getValueByBytesResult(res2)*TempKoef-273.15f,
            //     getValueByBytesResult(res1)*TempKoef-273.15f
            // };
            return new float[]
            {
               -1,
               -1,
               -1,
               -1
            };
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

        public Single Read27BusVoltage()
        {
            ushort[] result = new ushort[2];
            try
            {
                result = V27ModbusController.ReadInputRegisters(1, 7, 2);
                // Debug.WriteLine($" V27 Volt{result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                // Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);
        }


        public Single Read27BusAmperage()
        {

            ushort[] result = new ushort[2];
            try
            {
                result = I27ModbusController.ReadInputRegisters(1, 7, 2);
                // Debug.WriteLine($" A27Amperage{result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                // Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);

        }


        public float[] ReadPowerSupplyParams()
        {
            var result = new ushort[4];
            var res1 = new ushort[2]
            {
                   result[0],
                   result[1],
            }
            ;
            var res2 = new ushort[2]
           {
                   result[2],
                   result[3]

           };
            try
            {
                //Возможно не HoldingRegisters
                result = PowerSupplyModbusController.ReadHoldingRegisters(1, 5, 7); //0x0A05 - он же VSet и 0x0A07 - он же ISet   
                res1 = new ushort[2]
               {
                   result[0],
                   result[1],
               }
               ;
                res2 = new ushort[2]
              {
                   result[2],
                   result[3],

              };
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return new float[]
                {
                    -1,
                    -1
                };
            }


            return new float[]
            {

                getValueByBytesResult(res2),
                getValueByBytesResult(res1)
            };
        }
        


        public Single Read100BusVoltage()
        {

            ushort[] result = new ushort[2];
            try
            {
                result =  V100ModbusController.ReadInputRegisters(1, 7, 2);
                // Debug.WriteLine($" V100 Volt{result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                // Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);


        }


        public Single Read100BusAmperage()
        {
            ushort[] result = new ushort[2];
            try
            {
                result = I100ModbusController.ReadInputRegisters(1, 7, 2);
                // Debug.WriteLine($" A100 {result} {DateTime.Now} {DateTime.Now.Millisecond}");
            }
            catch (Exception ex)
            {

                // Debug.WriteLine(ex.Message);
            }
            return getValueByBytesResult(result);

        }


        #region InitUSRVirtualPorts
        private bool InitV27BusPort()
        {
            if (V27ModbusController != null)
            {
                V27ModbusController.Transport.ReadTimeout = 1500;
                V27ModbusController.Transport.WriteTimeout = 2000;
                // Debug.WriteLine("*");


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
                V27ModbusController.Transport.WriteTimeout = 2000;
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
            if (I27ModbusController != null)
            {
                I27ModbusController.Transport.ReadTimeout = 1500;
                I27ModbusController.Transport.WriteTimeout = 2000;
                // Debug.WriteLine("*");


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
                I27ModbusController.Transport.WriteTimeout = 2000;
                I27ModbusController.Transport.ReadTimeout = 1500;

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
                PortName = ICharger_PORT_NAME,
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
                V100ModbusController.Transport.ReadTimeout = 1500;
                V100ModbusController.Transport.WriteTimeout = 2000;
               

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
                V100ModbusController.Transport.WriteTimeout = 2000;
                V100ModbusController.Transport.ReadTimeout = 1500;

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
                I100ModbusController.Transport.ReadTimeout = 1500;
                I100ModbusController.Transport.WriteTimeout = 2000;
                


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
                I100ModbusController.Transport.ReadTimeout = 1500;

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
                PowerSupplyModbusController.Transport.ReadTimeout = 1500;
                PowerSupplyModbusController.Transport.WriteTimeout = 500;
            


                if (PowerSupplyModbusController.ReadInputRegisters(1,5, 1) != null)
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
                PowerSupplyModbusController.Transport.ReadTimeout = 1500;

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
            //    owenControllerTCPCLient?.Dispose();
            //    owenControllerTCPCLient = new TcpClient();
            //    owenControllerTCPCLient.Connect(OWEN_IP, OWEN_PORT);
            //    _statusBarViewModel.UpdateTaskProgress(75);
            //    owenController = _modbusFactory.CreateMaster(owenControllerTCPCLient);
            //    return owenControllerTCPCLient.Connected;
            return true;
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
                PortName = ITC_PORT_NAME,
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
                PortName = AKIP_PORT_NAME,
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
                IChargerSerialPort.Write(":01w12=0,\n");
                IChargerSerialPort.ReadLine();

                //PowerSupplyModbusController.WriteSingleRegisterAsync(1, 1, 0); //Настройка напряжения 0x0A01,устанавливаем как ноль
                //PowerSupplyModbusController.WriteSingleRegisterAsync(1, 3, 0; //Предел тока 0x0A03б,устанавливаем как ноль
                // PowerSupplyModbusController.WriteSingleCoils(1, 13, false);//Вылючение входа
                //PowerSupplyModbusController.WriteSingleCoil(1, 0, false);//Выключение дист режима;
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
                // Debug.WriteLine(ex.Message);
            }
        }

        public float[] ReadITCSerialPort()
        {
            float[] results = new float[3];
            try
            {
                lock (ITCSerialPort)
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

                // Debug.WriteLine(ex.Message);

                results[0] = -1;
                results[1] = -1;
                results[2] = -1;

            }

            return results;


        }
        public float[] ReadAkipSerialPort()
        {
            float[] results = new float[3];
            try
            {
                lock (AkipSerialPort)
                {
                    //TODO еще раз проверить параметры

                    AkipSerialPort.WriteLine("MEASURE:VOLTAGE?");
                    results[2] = Single.Parse(AkipSerialPort.ReadLine(), culture);  //3

                    AkipSerialPort.WriteLine($@"MEASURE:CURRENT?");
                    results[1] = Single.Parse(AkipSerialPort.ReadLine(), culture); //4

                    AkipSerialPort.WriteLine("MEASURE:POWER?");
                    results[0] = Single.Parse(AkipSerialPort.ReadLine(), culture); //5

                }

            }
            catch (Exception ex)
            {

                // Debug.WriteLine(ex.Message);

                results[0] = -1;
                results[1] = -1;
                results[2] = -1;

            }

            return results;


        }

        private string ReadICharger(SerialPort port)
        {
            //   port.Write(":01w20=3000,00100,\n");
            port.Write(":01w12=0,\n");
            return port.ReadLine();
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
                lock (AkipSerialPort)
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
                lock (ITCSerialPort)
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

        public async Task<bool> SetPowerSupplyValue(double AValue, double VValue)
        {
            Debug.WriteLine($"Amperage {AValue} Voltage{VValue}");
            await PowerSupplyModbusController.WriteSingleCoilAsync(1, 0, true);// Перевеод в диистанционный режим
            await PowerSupplyModbusController.WriteSingleCoilAsync(1, 13, true);//Включение входа
            await PowerSupplyModbusController.WriteSingleRegisterAsync(1, 1, (ushort) VValue); //Настройка напряжения 0x0A01
            await PowerSupplyModbusController.WriteSingleRegisterAsync(1, 3, (ushort) AValue); //Предел тока 0x0A03
            //Возможно, нужно использовать другие функции(Coil,Single,MultiplyRegisters)
            return true;
        }

        public async Task<bool> SetIchargerValue(string value)
        {
            bool result = false;
            return await Task.Run(() =>
            {
                lock (IChargerSerialPort)
                {

                    //   Debug.WriteLine(value);

                    //  IChargerSerialPort.Write(":01w10=01000,\n");

                   
                    

                    //string query = ($":01w11={value},\n");

                    //IChargerSerialPort.Write(query);
                    //IChargerSerialPort.ReadLine();

                   
                    IChargerSerialPort.Write($":01w12={value},\n");
                    IChargerSerialPort.ReadLine();
                    //IChargerSerialPort.Open();
                    result = true;


                    return result;
                }
            });



           
        }

        #endregion

    }
}
