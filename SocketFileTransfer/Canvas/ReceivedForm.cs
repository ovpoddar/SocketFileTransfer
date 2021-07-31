using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using SocketFileTransfer.Extensions;
using SocketFileTransfer.Models;

namespace SocketFileTransfer.Canvas
{
    public partial class ReceivedForm : Form
    {
        public EventHandler<ConnectionDetails> OnTransmissionIpFound;
        private readonly List<byte[]> _clientsData = new List<byte[]>();
        private readonly List<NetworkStream> _clientStreams = new List<NetworkStream>();
        private readonly List<TcpClient> _clients = new List<TcpClient>();
        private int _currentAdded;

        public ReceivedForm()
        {
            InitializeComponent();
        }

        public void StartBroadcast()
        {
            var tcpListener = new TcpListener(IPAddress.Any, 1400);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(BrodcastSignal, tcpListener);
        }

        private void BrodcastSignal(IAsyncResult ar)
        {
            var tcpListener = (TcpListener)ar.AsyncState;
            var client = tcpListener.EndAcceptTcpClient(ar);
            var buffer = new byte[client.ReceiveBufferSize];
            var stream = client.GetStream();

            _clients.Add(client);
            _clientsData.Add(buffer);
            _clientStreams.Add(stream);
            _currentAdded = _clientStreams.Count - 1;

            SendSelfInformation(stream);
            var bytes = new byte[client.ReceiveBufferSize];
            stream.BeginRead(bytes, 0, bytes.Length, DataReceived, _currentAdded);

            tcpListener.BeginAcceptTcpClient(BrodcastSignal, tcpListener);

        }

        private void SendSelfInformation(NetworkStream stream)
        {
            var message = Encoding.ASCII.GetBytes(Dns.GetHostName());
            stream.Write(message);
            stream.Flush();
        }

        private void DataReceived(IAsyncResult ar)
        {
            var currentAdded = (int)ar.AsyncState;
            var received = _clientStreams[currentAdded].EndRead(ar);

            if (received == 0)
                return;
            
            var message = Encoding.ASCII.GetString(_clientsData[currentAdded], 0, received);
           
            if (message != "@@Connected") return;
           
            OnTransmissionIpFound.Raise(this, new ConnectionDetails
            {
                EndPoint = (IPEndPoint)_clients[currentAdded].Client.RemoteEndPoint,
                TypeOfConnect = TypeOfConnect.Received
            });

            for (var i = 0; i < _currentAdded; i++)
            {
                _clients[i].Close();
                _clients[i].Dispose();
                _clientStreams[i].Close();
                _clientStreams[i].Dispose();
            }
            Dispose();
        }
    }
}
