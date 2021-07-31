using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

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

        public void StartBrodcast()
        {
            var tcpListner = new TcpListener(IPAddress.Any, 1400);
            tcpListner.Start();
            tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);
        }

        private void BrodcastSignal(IAsyncResult ar)
        {
            var tcpListner = (TcpListener)ar.AsyncState;
            var client = tcpListner.EndAcceptTcpClient(ar);
            var buffer = new byte[client.ReceiveBufferSize];
            var stream = client.GetStream();

            _clients.Add(client);
            _clientsData.Add(buffer);
            _clientStreams.Add(stream);
            _currentAdded = _clientStreams.Count - 1;

            SendSelfInformation(stream);
            var bytes = new byte[client.ReceiveBufferSize];
            stream.BeginRead(bytes, 0, bytes.Length, DataReceived, _currentAdded);

            tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);

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
            var receved = _clientStreams[currentAdded].EndRead(ar);
            if (receved == 0)
            {
                return;
            }
            var message = Encoding.ASCII.GetString(_clientsData[currentAdded], 0, receved);
            if (message == "@@Connected")
            {
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
}
