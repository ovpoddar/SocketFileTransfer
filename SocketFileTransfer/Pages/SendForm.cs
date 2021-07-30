using SocketFileTransfer.CustomControl;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace SocketFileTransfer.Pages
{
    public partial class SendForm : Form
    {
        public EventHandler<ConnectionDetails> OnTransmissionIpFound;
        private Thread Workerthread { get; set; }
        private Thread Loopingthread { get; set; }
        private ArrayList IpAdressList { get; set; } = new ArrayList();
        private bool _canScan = true;
        public SendForm()
        {
            InitializeComponent();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // TODO: later date work on lan
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            IpAdressList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
        }

        private void SendForm_Load(object sender, EventArgs e)
        {
            StartScan();
        }

        private void StartScan()
        {
            Workerthread = new Thread(() =>
            {
                foreach (string ipadress in IpAdressList)
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
            });
            Workerthread.Start();

            Loopingthread = new Thread(() =>
            {
                while (_canScan && !Workerthread.IsAlive)
                {
                    Workerthread.Join();
                    Workerthread.Start();
                }
            });
            Loopingthread.Start();
        }


        private readonly List<NetworkStream> _streams = new List<NetworkStream>();
        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            var ip = (string)e.UserState;
            if (e.Reply.Status == IPStatus.Success)
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
                    return;
                }
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
                var random = new Random();
                listBox1.Items.Add($"{model.Ip} {name}");
            }));
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _canScan = false;
            var ip = listBox1.SelectedItem.ToString().Split(' ')[0];
            var message = Encoding.ASCII.GetBytes("@@Connected");
            _streams[listBox1.SelectedIndex].Write(message, 0, message.Length);
            _streams[listBox1.SelectedIndex].Flush();

            OnTransmissionIpFound.Raise(this, new ConnectionDetails
            {
                EndPoint = IPEndPoint.Parse(ip + ":1400"),
                TypeOfConnect = TypeOfConnect.Send
            });
            for (var i = 0; i < _streams.Count; i++)
            {
                _streams[i].Close();
                _streams[i].Dispose();
            }

            Dispose();
        }

        private class TransfarModel
        {
            public byte[] Buffer { get; set; }
            public string Ip { get; set; }
            public int Id { get; set; }
            public TcpClient Client { get; set; }
        }
    }

}
