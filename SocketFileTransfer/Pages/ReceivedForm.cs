using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketFileTransfer.Pages
{
    public partial class ReceivedForm : Form
    {
        #region Test Brodcast
        //private readonly List<byte[]> _clientsData = new List<byte[]>();
        //private readonly List<NetworkStream> _clientStreams = new List<NetworkStream>();
        //private int _currentAdded;
        //public ReceivedForm()
        //{
        //    InitializeComponent();
        //}

        //public void StartBrodcast()
        //{
        //    var tcpListner = new TcpListener(IPAddress.Any, 1400);
        //    tcpListner.Start();
        //    tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);
        //}

        //private void BrodcastSignal(IAsyncResult ar)
        //{
        //    var tcpListner = (TcpListener)ar.AsyncState;
        //    var client = tcpListner.EndAcceptTcpClient(ar);
        //    var stream = client.GetStream();
        //    SendSelfInformation(stream);

        //    tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);
        //}

        //private void SendSelfInformation(NetworkStream stream)
        //{
        //    var message = Encoding.ASCII.GetBytes(Dns.GetHostName());
        //    stream.Write(message);
        //    stream.Flush();

        //    var bytes = new byte[1024];
        //    stream.BeginRead(bytes, 0, bytes.Length, CheckForConnection, stream);
        //}

        //private void CheckForConnection(IAsyncResult ar)
        //{
        //    var tcpListener = (TcpListener)ar.AsyncState;
        //    var newMember = tcpListener.EndAcceptTcpClient(ar);
        //    var buffer = new byte[newMember.ReceiveBufferSize];

        //    var stream = newMember.GetStream();
        //    _clientsData.Add(buffer);
        //    _clientStreams.Add(stream);
        //    _currentAdded = _clientStreams.Count - 1;

        //    var message = Encoding.ASCII.GetBytes(Dns.GetHostName(), 0, 10);



        //    stream.BeginWrite(message, 0, message.Length, SendData, null);
        //    stream.BeginRead(buffer, 0, buffer.Length, DataReceived, null);
        //    Invoke((Action)delegate
        //    {
        //        dataGridView1.Rows.Add(newMember.Client.AddressFamily, newMember.Available);
        //    });
        //    tcpListener.BeginAcceptTcpClient(BrodcastSignal, tcpListener);
        //}

        //private void SendData(IAsyncResult ar)
        //{
        //    _clientStreams[_currentAdded].EndWrite(ar);
        //}

        //private void DataReceived(IAsyncResult ar)
        //{
        //    var receved = _clientStreams[_currentAdded].EndRead(ar);
        //    if (receved == 0)
        //    {
        //        return;
        //    }
        //    string message = Encoding.ASCII.GetString(_clientsData[_currentAdded], 0, receved);
        //    if(message == "Connected")
        //    {
        //        for (int i = 0; i < length; i++)
        //        {

        //        }
        //    }
        //} 
        #endregion


    }
}
