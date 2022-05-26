using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services
{

   
    class SCPIService
    {
        private SerialPort serialPort;
       

        public void Init()
        {
           
            
                serialPort = new SerialPort();
                {
                    serialPort.BaudRate = 9600;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.PortName = "COM3";
                    serialPort.Open();
                    serialPort.WriteTimeout = 2000;
                    serialPort.ReadTimeout = 2000;

                }
            




        }

        public void Write(string str)
        {
            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
            serialPort.WriteLine(str);
            Debug.WriteLine(serialPort.ReadLine());
            //serialPort.DataReceived += SerialPort_DataReceived;
            //serialPort.ErrorReceived += SerialPort_ErrorReceived;
            //mbSession.RawIO.Write(str);
            //Debug.WriteLine(mbSession.RawIO.ReadString());

        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Debug.WriteLine(e.EventType);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }
    }
}
