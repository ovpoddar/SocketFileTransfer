using SocketFileTransfer.Model;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SocketFileTransfer.Pages
{
    public partial class SendForm : Form
    {
        #region TestScan

        //private Thread Workerthread { get; set; }
        //private ArrayList IpAdressList { get; set; } = new ArrayList();
        //public SendForm()
        //{
        //    InitializeComponent();

        //    // TODO: make sure the interface up when the application start

        //    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        //    {
        //        // TODO: work with only wifi for now
        //        // TODO: later date work on lan
        //        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up)
        //        {
        //            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
        //            {
        //                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
        //                {
        //                    IpAdressList.Add(ip.Address.ToString());
        //                }
        //            }
        //        }
        //    }
        //}

        //private void SendForm_Load(object sender, EventArgs e)
        //{
        //    StartScan();
        //}

        //private void StartScan()
        //{
        //    Workerthread = new Thread(() =>
        //    {
        //        foreach (string ipadress in IpAdressList)
        //        {
        //            string[] ipRange = ipadress.Split('.');
        //            for (int i = 0; i < 255; i++)
        //            {
        //                string testIP = ipRange[0] + '.' + ipRange[1] + '.' + ipRange[2] + '.' + i.ToString();
        //                var ping = new Ping();
        //                ping.PingCompleted += Ping_PingCompleted;
        //                ping.SendAsync(testIP, 100, testIP);
        //                ping.Dispose();
        //            }

        //        }
        //    });
        //    Workerthread.Start();
        //}


        //private TcpClient _client1;
        //private NetworkStream _stream;
        //private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        //{
        //    var ip = (string)e.UserState;
        //    if (e.Reply.Status == IPStatus.Success)
        //    {
        //        try
        //        {
        //            _client1 = new TcpClient();
        //            _client1.Connect(ip, 1400);

        //            var model = new TransfarModel
        //            {
        //                Buffer = new byte[_client1.ReceiveBufferSize],
        //                Ip = ip
        //            };
        //            _stream = _client1.GetStream();
        //            _stream.BeginRead(model.Buffer, 0, model.Buffer.Length, DataReceved, model);
        //        }
        //        catch
        //        {
        //            return;
        //        }
        //    }
        //}

        //private void DataReceved(IAsyncResult ar)
        //{
        //    var receved = _stream.EndRead(ar);
        //    var model = (TransfarModel)ar.AsyncState;
        //    if (_client1.ReceiveBufferSize < 0)
        //        return;
        //    string name = Encoding.ASCII.GetString(model.Buffer, 0, receved);
        //    Invoke(new MethodInvoker(() =>
        //    {
        //        listBox1.Items.Add($"{model.Ip} {name}");
        //    }));
        //    _stream.Dispose();
        //    _client1.Close();
        //}

        //private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var lBox = (ListBox)sender;
        //    var ip = lBox.SelectedItem.ToString().Remove(13);
        //    ConnectServer(ip);
        //}

        //private void ConnectServer(string ipAdress)
        //{
        //    var socket = new Socket(addressFamily: AddressFamily.InterNetwork, socketType: SocketType.Stream, protocolType: ProtocolType.Tcp);
        //    socket.BeginConnect(IPAddress.Parse(ipAdress), 1401, Connceting, socket);
        //}

        //private void Connceting(IAsyncResult ar)
        //{
        //    var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        //    var file = @"C:\Users\Ayan\Desktop\Ayan.docx";
        //    socket.SendFile(file, null, null, TransmitFileOptions.UseSystemThread);
        //}


        //private class TransfarModel
        //{
        //    public byte[] Buffer { get; set; }
        //    public string Ip { get; set; }
        //}
        #endregion


        private TcpClient _tcpClient = new();
        private NetworkStream _networkStream;

        private void Connect(IPEndPoint endPoint)
        {
            _tcpClient = new TcpClient();
            _tcpClient.BeginConnect(endPoint.Address, endPoint.Port, callBack, _tcpClient);
        }

        private void callBack(IAsyncResult ar)
        {
            var tcpClient = (TcpClient)ar.AsyncState;
            _networkStream = tcpClient.GetStream();
        }

        private void SendFile(string file, FileTypes fileTypes, CancellationToken token)
        {
            if (_tcpClient.Connected)
            {
                var data = new TransfarFileModel()
                {
                    FileType = fileTypes
                };
                switch (fileTypes)
                {
                    case FileTypes.File when File.Exists(file):
                        data = new TransfarFileModel()
                        {
                            File = GetFileAsByte(file),
                            Name = Path.GetFileName(file)
                        };
                        break;
                    default:
                        data = new TransfarFileModel()
                        {
                            File = Encoding.ASCII.GetBytes(file)
                        };
                        break;
                }
                var objBytes = GetObjectAsByte(data);
                _networkStream.WriteAsync(objBytes, 0, objBytes.Length, token);
                _networkStream.FlushAsync(token);
            }
        }

        private byte[] GetFileAsByte(string file)
        {
            using (var input = new StreamReader(file).BaseStream)
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private byte[] GetObjectAsByte(TransfarFileModel obj)
        {
            var size = Marshal.SizeOf(obj);
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            return bytes;
        }

        ~SendForm()
        {
            _tcpClient.Close();
            _tcpClient.Dispose();
            _networkStream.Dispose();
        }
    }
}
