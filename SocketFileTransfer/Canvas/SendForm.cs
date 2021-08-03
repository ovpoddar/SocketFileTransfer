using NativeWifi;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas
{
    public partial class SendForm : Form
    {
        public EventHandler<ConnectionDetails> OnTransmissionIpFound;
        private readonly List<NetworkStream> _streams = new List<NetworkStream>();
        private bool _canScan = true;
        public SendForm()
        {
            InitializeComponent();
        }

        private void SendForm_Load(object sender, EventArgs e)
        {
            StartScan();
        }

        private void StartScan()
        {
            var thread = new Thread(() =>
            {
                while (_canScan)
                {
                    var client = new WlanClient();

                    foreach (var wlaninterfaces in client.Interfaces)
                    {
                        foreach (var networks in wlaninterfaces.GetAvailableNetworkList(Wlan.WlanGetAvailableNetworkFlags.IncludeAllManualHiddenProfiles))
                        {
                            if (networks.profileName == string.Empty)
                                continue;
                            Invoke(new MethodInvoker(() =>
                            {
                                var user = $"{Encoding.ASCII.GetString(networks.dot11Ssid.SSID, 0, (int)networks.dot11Ssid.SSIDLength)} {TransfarMedia.WIFI}";
                                if (!listBox1.Items.Contains(user))
                                    listBox1.Items.Add(user);
                            }));
                        }
                    }

                    Thread.Sleep(500);

                    var obtainIps = GetRouterIp();

                    foreach (var ipadress in obtainIps)
                    {
                        var ipRange = ipadress.Split('.');
                        for (var i = 0; i < 255; i++)
                        {
                            var testIP = ipRange[0] + '.' + ipRange[1] + '.' + ipRange[2] + '.' + i.ToString();
                            var ping = new Ping();
                            ping.PingCompleted += Ping_PingCompleted;
                            ping.SendAsync(testIP, 100, testIP);
                            ping.Dispose();
                        }
                    }

                    Thread.Sleep(1000);
                }
            });
            thread.Start();
        }

        // && ni.Name == "Ethernet"
        // this line will excluse VMS if you want to sacn for vms too then uncomment this line
        // Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
        // the where check will exclude all the ipv6 adressed
        private List<string> GetRouterIp() =>
            NetworkInterface.GetAllNetworkInterfaces()
            .Where(e =>
                e.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                e.Name == "Ethernet" &&
                e.OperationalStatus == OperationalStatus.Up)
            .SelectMany(e =>
                e.GetIPProperties().UnicastAddresses
                .Where(i =>
                    i.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(x => x.Address.ToString())).ToList();

        // && ni.Name == "Wi-Fi"
        // this line will excluse Hotspots if you want to sacn for Hotspots too then uncomment this line
        // Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
        // the where check will exclude all the ipv6 adressed
        private List<string> GetWifiIp() =>
            NetworkInterface.GetAllNetworkInterfaces()
                .Where(e =>
                    e.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                    e.Name == "Wi-Fi" &&
                    e.OperationalStatus == OperationalStatus.Up)
                .SelectMany(e =>
                    e.GetIPProperties().UnicastAddresses
                        .Where(i =>
                            i.Address.AddressFamily == AddressFamily.InterNetwork)
                        .Select(x =>
                        x.Address.ToString()))
            .ToList();

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            var ip = (string)e.UserState;

            if (e.Reply == null || e.Reply.Status != IPStatus.Success) return;

            CheckIP(ip);
        }

        private void CheckIP(string ip)
        {
            var client = new TcpClient();
            try
            {
                client.Connect(ip, 1400);
                _streams.Add(client.GetStream());
                var current = _streams.Count - 1;
                var model = new TransfarModel
                {
                    Buffer = new byte[client.ReceiveBufferSize],
                    Ip = ip,
                    Id = current,
                    Client = client,
                };
                _streams[current].BeginRead(model.Buffer, 0, model.Buffer.Length, DataReceved, model);
            }
            catch
            {
                client.Dispose();
            }
        }

        private void DataReceved(IAsyncResult ar)
        {
            var model = (TransfarModel)ar.AsyncState;
            var receved = _streams[model.Id].EndRead(ar);

            if (model.Client.ReceiveBufferSize < 0)
                return;

            var name = Encoding.ASCII.GetString(model.Buffer, 0, receved);
            Invoke(new MethodInvoker(() =>
            {
                var user = $"{model.Ip} {name} {TransfarMedia.Ethernet}";
                if (!listBox1.Items.Contains(user))
                    listBox1.Items.Add(user);
            }));
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _canScan = false;

            if (string.IsNullOrWhiteSpace(listBox1.SelectedItem.ToString()))
                return;

            var ip = listBox1.SelectedItem.ToString().Split(' ')[0];

            var type = listBox1.SelectedItem.ToString().Split(' ')[^1];

            if (type == "Ethernet")
            {
                var message = Encoding.ASCII.GetBytes("@@Connected");

                _streams[listBox1.SelectedIndex].Write(message, 0, message.Length);
                _streams[listBox1.SelectedIndex].Flush();

                OnTransmissionIpFound.Raise(this, new ConnectionDetails
                {
                    EndPoint = IPEndPoint.Parse(ip + ":1400"),
                    TypeOfConnect = TypeOfConnect.Send
                });

                foreach (var t in _streams)
                {
                    t.Close();
                    t.Dispose();
                }


                Dispose();
            }
            else if (type == "WIFI")
            {
                var client = new WlanClient();
                foreach (var wlanInterface in client.Interfaces)
                {
                    wlanInterface.ConnectSynchronously(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, ip, 5000);
                }

                var wifiIp = GetWifiIp();

                foreach (var ipaddr in wifiIp)
                {
                    CheckIP(ipaddr);
                }

                if (_streams.Count == 0)
                    MessageBox.Show($"{ip} network is not accessable. please start the application on {ip} in received mode.");
                else
                {
                    var message = Encoding.ASCII.GetBytes("@@Connected");

                    _streams.FirstOrDefault().Write(message, 0, message.Length);
                    _streams.FirstOrDefault().Flush();

                    OnTransmissionIpFound.Raise(this, new ConnectionDetails
                    {
                        EndPoint = IPEndPoint.Parse(wifiIp.First() + ":1400"),
                        TypeOfConnect = TypeOfConnect.Send
                    });

                    foreach (var t in _streams)
                    {
                        t.Close();
                        t.Dispose();
                    }


                    Dispose();
                }
            }
        }
    }

}
