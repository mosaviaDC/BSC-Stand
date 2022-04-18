using OwenioNet.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Infastructure.Owen
{
    internal class OwenTCPClientAdapter : IStreamResource
    {
        private TcpClient _tcpClient;
        private string _ipAdress;
        private int _port;
        public OwenTCPClientAdapter(string ipAdress, int Port)
        {
            _tcpClient = new TcpClient();
            _ipAdress = ipAdress;  
            _port = Port;
            _tcpClient.Connect(_ipAdress, _port);
        }


        public bool IsOpened => _tcpClient.Connected;

        public int ReadTimeout { get => _tcpClient.GetStream().WriteTimeout; set => _tcpClient.GetStream().WriteTimeout = value; }
        public int WriteTimeout { get => _tcpClient.GetStream().WriteTimeout; set => _tcpClient.GetStream().WriteTimeout = value; }

        public void Close()
        {
            _tcpClient.Close();
        }

        public void DiscardInBuffer()
        {
            _tcpClient.GetStream().Flush();
        }

        public void Dispose()
        {
            // Dispose()
            _tcpClient.Close();
            _tcpClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Open()
        {

          
            Debug.WriteLine("Open TCP Connection");
        }

        public int Read(byte[] buffer, int offset, int count)
        {
          //  Debug.WriteLine("Read");
           return _tcpClient.GetStream().Read(buffer, offset, count);
        }  

        public void Write(byte[] buffer, int offset, int count)
        {
            Debug.WriteLine("Write");
            _tcpClient.GetStream().Write(buffer, offset, count);
        }
    }
}
