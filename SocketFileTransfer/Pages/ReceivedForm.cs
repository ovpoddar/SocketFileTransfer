using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace SocketFileTransfer.Pages
{
    public partial class ReceivedForm : Form
    {

        #region Connect To Another pc and handel the file transfar
        private Socket _sarverSocket;
        private Socket _clientSocket;
        private byte[] _buffer;

        public void OpenPortToConnect(IPEndPoint endPoint)
        {
            try
            {
                _sarverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _sarverSocket.Bind(endPoint);
                _sarverSocket.Listen(10);
                _sarverSocket.BeginAccept(AcceptClient, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AcceptClient(IAsyncResult ar)
        {
            _clientSocket = _sarverSocket.EndAccept(ar);
            _buffer = new byte[_clientSocket.ReceiveBufferSize];
            


            _sarverSocket.BeginAccept(AcceptClient, null);
        }

        private void SendMessage(string message, FileTypes fileTypes)

        #endregion
    }
}
