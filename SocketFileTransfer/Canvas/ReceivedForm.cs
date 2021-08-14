using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Principal;
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
            StartHotspot();
            if (CheckHotSpot() || CheckEthernet())
            {
                StartBrodcast();
                LblMsg.Text = "waiting for user to connect";
            }
            else
                LblMsg.Text = "Faild To start Your hotspot. Please do it maually or connect your self with a network cable which is connected with router.";
        }

        private void StartHotspot()
        {
            var id = WindowsIdentity.GetCurrent();
            var p = new WindowsPrincipal(id);
            if (p.IsInRole(WindowsBuiltInRole.Administrator))
            {
                var startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
                startInfo.Verb = "runas";
                Process.Start(startInfo);

                Application.Exit();
            }
            else
            {
                var processStartInfo = new ProcessStartInfo("cmd.exe")
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                var process = Process.Start(processStartInfo);
                process.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=" + Dns.GetHostName());
                process.StandardInput.WriteLine("netsh wlan start hosted network");
                process.StandardInput.Close();
            }
        }

        private bool CheckHotSpot() =>
            NetworkInterface.GetAllNetworkInterfaces()
                .Any(a => a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                          && a.OperationalStatus == OperationalStatus.Up
                          && a.Name.ToUpper().Contains("Local Area Connection".ToUpper()));

        private bool CheckEthernet() =>
            NetworkInterface.GetAllNetworkInterfaces()
                .Any(a => a.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                          && a.OperationalStatus == OperationalStatus.Up
                          && a.Name.ToUpper() == "Ethernet".ToUpper());

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

            try
            {
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
            catch
            {
                _clients[currentAdded].Close();
                _clients[currentAdded].Dispose();
                _clientStreams[currentAdded].Close();
                _clientStreams[currentAdded].Dispose();
                _clientsData.RemoveAt(currentAdded);
                _clientStreams.RemoveAt(currentAdded);
                _clients.RemoveAt(currentAdded);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            OnTransmissionIpFound.Raise(this, new ConnectionDetails
            {
                TypeOfConnect = TypeOfConnect.None,
                EndPoint = null
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
