using SocketFileTransfer.CustomControl;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
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
		private readonly Dictionary<Guid, CPFile> _directory;

		public TransmissionPage(ConnectionDetails connectionDetails)
		{
			InitializeComponent();
			_directory = new();
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

			var buffer = new byte[8];
			
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
				
				var packetSize = Unsafe.ReadUnaligned<int>(ref buffer[0]);
				ProcessPacket(packetSize);
				// need to figure out the next packet size and allocate new arrey
				buffer = new byte[8];
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
			}
			catch(Exception ex) 
			{
				Debug.WriteLine(ex);
				Logging(ContentType.Message, "User is Disconnected", TypeOfConnect.None);
			}
		}

		private async void ProcessPacket(int packetSize)
		{
			var packet = new byte[packetSize];
			var received = await _clientSocket.ReceiveAsync(packet);
			if (received == packetSize)
				MessageBox.Show("bingo. same size");

			var networkPacket = (NetworkPacket)packet;
			MessageBox.Show(networkPacket.ContentSize.ToString());
		}

		async Task SendData(ContentType contentType, string content, Socket socket)
		{
			var chunkBuilder = new ChunkBuilder(contentType, content);
            foreach (var chunk in chunkBuilder.Get(0))
            {
				var isSuccessFul = await ProjectStandardUtilitiesHelper.SendData(socket, chunk);
				if (!isSuccessFul)
				{
					MessageBox.Show("Failed to send the file");
					break;
				}
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
					//case ContentType.File:
					//	var component = new string[3] { "Test.Txt", "5000000","Txt" };
					//	PanelContainer.Controls.Add(new CPFile(component[0], component[1], component[2], typeOfConnect));
					//	break;
					//case ContentType.Message:
					//	PanelContainer.Controls.Add(new CPFile(message, typeOfConnect));
					//	break;
					//case ContentType.Commend:
					//	break;
					//default:
					//	break;
				}
			});

		}
	}
}
