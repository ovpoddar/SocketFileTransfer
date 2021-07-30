using SocketFileTransfer.CustomControl;
using SocketFileTransfer.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SocketFileTransfer.Pages
{
    public partial class TransmissionPage : Form
    {
        private Socket _socket;
        private IPEndPoint _iPEndPoint;
        public TransmissionPage(ConnectionDetails connectionDetails)
        {
            InitializeComponent();
            var worker = new Thread(() =>
            {
                switch (connectionDetails.TypeOfConnect)
                {
                    case TypeOfConnect.Send:
                        ConnectPort(connectionDetails.EndPoint);
                        break;
                    default:
                        OpenPortToConnect(connectionDetails.EndPoint);
                        break;
                }
            });
            worker.Start();
        }

        private void OpenPortToConnect(IPEndPoint endPoint)
        {
            _iPEndPoint = endPoint;
            try
            {
                var sarverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sarverSocket.Bind(new IPEndPoint(IPAddress.Any, endPoint.Port));
                sarverSocket.Listen(10);
                sarverSocket.BeginAccept(AcceptClient, sarverSocket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AcceptClient(IAsyncResult ar)
        {
            var sarverSocket = (Socket)ar.AsyncState;
            _socket = sarverSocket.EndAccept(ar);

            if (_socket.RemoteEndPoint == _iPEndPoint)
            {
                var buffer = new byte[_socket.ReceiveBufferSize];
                _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
            }
            else
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
        }

        private void ConnectPort(IPEndPoint endPoint)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.BeginConnect(endPoint, OnConnect, null);
        }

        private void OnConnect(IAsyncResult ar)
        {
            _socket.EndConnect(ar);

            var buffer = new byte[_socket.ReceiveBufferSize];

            _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
        }

        private void OnReceivedEnd(IAsyncResult ar)
        {
            var buffer = (byte[])ar.AsyncState;
            var received = _socket.EndReceive(ar);
            if (received == 0)
            {
                return;
            }
            var message = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

            if (message.Contains(':'))
            {
                // prepare for file;
                Logging(FileTypes.File, message, TypeOfConnect.Received);
            }
            else if (message.Contains("@@"))
            {
                // It's a commend
            }
            else
            {
                // Simple Sting
                Logging(FileTypes.Text, message, TypeOfConnect.Received);
            }
            _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
        }

        private void SendData(string file, FileTypes fileTypes, Socket socket)
        {
            if (fileTypes == FileTypes.File && File.Exists(file))
            {
                var fileinfo = new FileInfo(file);
                var message = Encoding.ASCII.GetBytes($"{fileinfo.Name}:{fileinfo.Length}:{fileinfo.Extension}");
                socket.Send(message, 0, message.Length, SocketFlags.None);
                socket.SendFile(file);

                Logging(fileTypes, Encoding.ASCII.GetString(message), TypeOfConnect.Send);
                return;
            }
            else if (fileTypes == FileTypes.File && !File.Exists(file) || fileTypes == FileTypes.Text)
            {
                var message = Encoding.ASCII.GetBytes(file);
                socket.Send(message, 0, message.Length, SocketFlags.None);
                Logging(fileTypes, Encoding.ASCII.GetString(message), TypeOfConnect.Send);
            }
            else
            {
                var message = Encoding.ASCII.GetBytes($"@@ {file.ToUpper()}");
                socket.Send(message, 0, message.Length, SocketFlags.None);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (TxtMessage.Text.Length <= 0)
            {
                BtnOprate.Text = "->";
            }
            else
            {
                BtnOprate.Text = "+";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TxtMessage.Text.Length <= 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SendData(ofd.FileName, FileTypes.File, _socket);
                }
            }
            else
            {
                SendData(TxtMessage.Text, FileTypes.Text, _socket);
                TxtMessage.Text = "";
            }
        }

        public void Logging(FileTypes fileTypes, string message, TypeOfConnect typeOfConnect)
        {
            switch (fileTypes)
            {
                case FileTypes.File:
                    var component = message.Split(":");
                    PanelContainer.Controls.Add(new CPFile(component[0], component[1], component[2], typeOfConnect));
                    break;
                case FileTypes.Text:
                    PanelContainer.Controls.Add(new CPFile(message, typeOfConnect));
                    break;
                case FileTypes.Commend:
                    break;
                default:
                    break;
            }

        }
    }
}
