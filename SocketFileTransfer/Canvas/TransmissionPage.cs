using SocketFileTransfer.CustomControl;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas
{
	public partial class TransmissionPage : Form
	{
		private Socket _clientSocket;
		private Socket _serverSocket;
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

					case TypeOfConnect.Received:
						OpenPortToConnect(connectionDetails.EndPoint);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(connectionDetails), connectionDetails, null);
				}
			});
			worker.Start();
		}

		private void OpenPortToConnect(IPEndPoint endPoint)
		{
			try
			{
				_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				_serverSocket.Bind(endPoint);
				_serverSocket.Listen(10);
				_serverSocket.BeginAccept(AcceptClient, null);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void AcceptClient(IAsyncResult ar)
		{
			_clientSocket = _serverSocket.EndAccept(ar);
			var buffer = new byte[_clientSocket.ReceiveBufferSize];
			_clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
		}

		private void ConnectPort(IPEndPoint endPoint)
		{
			_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_clientSocket.BeginConnect(endPoint, OnConnect, null);
		}

		private void OnConnect(IAsyncResult ar)
		{
			_clientSocket.EndConnect(ar);

			var buffer = new byte[_clientSocket.ReceiveBufferSize];
			
			_clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
		}

		private void OnReceivedEnd(IAsyncResult ar)
		{
			try
			{
				var buffer = (byte[])ar.AsyncState;
				var received = _clientSocket.EndReceive(ar);
				if (received == 0)
				{
					return;
				}
				//var message = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
				//if (message.Contains(':'))
				//{
				//	// prepare for file;
				//	Logging(ContentType.File, message, TypeOfConnect.Received);
				//}
				//else if (message.Contains("@@"))
				//{
				//	// It's a commend
				//}
				//else
				//{
				//	// Simple Sting
				//	Logging(ContentType.Message, message, TypeOfConnect.Received);
				//}
				MessageBox.Show("showing the code");
				var c = (NetworkPacket)buffer;
				MessageBox.Show(c.Data.Length.ToString());
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
			}
			catch
			{
				Logging(ContentType.Message, "User is Disconnected", TypeOfConnect.None);
			}
		}

		//private void SendData(string file, ContentType fileTypes, Socket socket)
		//{
		//	if (fileTypes == ContentType.File && File.Exists(file))
		//	{
		//		var fileInfo = new FileInfo(file);
		//		var message = Encoding.ASCII.GetBytes($"{fileInfo.Name}:{fileInfo.Length}:{fileInfo.Extension}");
		//		socket.Send(message, 0, message.Length, SocketFlags.None);
		//		socket.SendFile(file);

		//		Logging(fileTypes, Encoding.ASCII.GetString(message), TypeOfConnect.Send);
		//		return;
		//	}
		//	else if (fileTypes == ContentType.File && !File.Exists(file) || fileTypes == ContentType.Message)
		//	{
		//		var message = Encoding.ASCII.GetBytes(file);
		//		socket.Send(message, 0, message.Length, SocketFlags.None);
		//		Logging(fileTypes, Encoding.ASCII.GetString(message), TypeOfConnect.Send);
		//	}
		//	else
		//	{
		//		var message = Encoding.ASCII.GetBytes($"@@ {file.ToUpper()}");
		//		socket.Send(message, 0, message.Length, SocketFlags.None);
		//	}
		//}

		async Task SendData(ContentType contentType, string content, Socket socket)
		{
			var chunkBuilder = new ChunkBuilder(contentType, content);
            await foreach (var chunk in chunkBuilder.Get(0))
            {
				await socket.SendAsync((byte[])chunk);
            }
			Logging(contentType, content, TypeOfConnect.Send);
        }

		private void TextBox1_TextChanged(object sender, EventArgs e)
		{
			BtnOperate.Text = TxtMessage.Text.Length <= 0 ? "->" : "+";
		}

		private async void Button1_Click(object sender, EventArgs e)
		{
			if (TxtMessage.Text.Length <= 0)
			{
				var ofd = new OpenFileDialog
				{
					Multiselect = false
				};

				if (ofd.ShowDialog() == DialogResult.OK)
					await SendData(ContentType.File, ofd.FileName, _clientSocket);
			}
			else
			{
				await SendData(ContentType.Message, TxtMessage.Text, _clientSocket);
				TxtMessage.Text = "";
			}
		}

		public void Logging(ContentType fileTypes, string message, TypeOfConnect typeOfConnect)
		{
			Invoke(() =>
			{
				switch (fileTypes)
				{
					case ContentType.File:
						var component = message.Split(":");
						PanelContainer.Controls.Add(new CPFile(component[0], component[1], component[2], typeOfConnect));
						break;
					case ContentType.Message:
						PanelContainer.Controls.Add(new CPFile(message, typeOfConnect));
						break;
					case ContentType.Commend:
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(fileTypes), fileTypes, null);
				}
			});

		}
	}
}
