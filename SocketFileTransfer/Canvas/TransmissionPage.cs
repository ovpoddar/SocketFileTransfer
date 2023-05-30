using SocketFileTransfer.CustomControl;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas
{
	public partial class TransmissionPage : Form
	{
		private Socket _clientSocket;
		private Socket _serverSocket;
		private PacketSender _packetSender;

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
			_packetSender = new(_clientSocket);
			_packetSender.EventHandler += ProgressEvent;
			var buffer = new byte[_clientSocket.ReceiveBufferSize];
			_clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
		}

		private void ProgressEvent(object sender, ProgressReport e)
		{
			// use the guid for fining the element.
			var d = PanelContainer.Controls.OfType<CPFile>();
			var control = PanelContainer.Controls.OfType<CPFile>().FirstOrDefault(a => a.Name == e.TargetedItemName);
			if (control != null)
			{
				control.ChangeProcess(e);
			}

		}

		private void ConnectPort(IPEndPoint endPoint)
		{
			_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_clientSocket.BeginConnect(endPoint, OnConnect, null);
		}

		private void OnConnect(IAsyncResult ar)
		{
			_clientSocket.EndConnect(ar);
			_packetSender = new(_clientSocket);
			_packetSender.EventHandler += ProgressEvent;
			var buffer = new byte[8];

			_clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
		}

		private async void OnReceivedEnd(IAsyncResult ar)
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
				var restOfPacket = new byte[packetSize];
				await _clientSocket.ReceiveAsync(restOfPacket);
				var fullPacket = new byte[packetSize + buffer.Length];
				Array.Copy(buffer, fullPacket, buffer.Length - 1);
				Array.Copy(restOfPacket, 0, fullPacket, buffer.Length, restOfPacket.Length);
				NetworkPacket networkPack = fullPacket;
				if (fullPacket is null)
				{
					buffer = new byte[8];
					_clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
					return;
				}
				else
				{
					ProcessNetWorkPack(networkPack);
					await _packetSender.ReceivedContent(networkPack);
					buffer = new byte[8];
					_clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				Logging(ContentType.Message, "User is Disconnected", TypeOfConnect.None);
			}
		}

		void ProcessNetWorkPack(NetworkPacket fullPacket)
		{
			if (fullPacket.PacketType == ContentType.File)
			{
				var fileInfo = (FileDetails)fullPacket.Data;
				LogFile(fileInfo, TypeOfConnect.Received);
			}
		}

		private void LogFile(FileDetails fileInfo, TypeOfConnect send)
		{
			Invoke(() =>
			{
				var fileItem = new CPFile(fileInfo, send);
				PanelContainer.Controls.Add(fileItem);
			});
		}

		private void TextBox1_TextChanged(object sender, EventArgs e)
		{
			BtnOperate.Text = TxtMessage.Text.Length <= 0 ? "->" : "+";
		}

		private async void Button1_Click(object sender, EventArgs e)
		{
			try
			{
				if (TxtMessage.Text.Length <= 0)
				{
					var ofd = new OpenFileDialog
					{
						Multiselect = false
					};

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						var fileinfo = new FileDetails(ofd.FileName);
						LogFile(fileinfo, TypeOfConnect.Send);
						await Task.Run(async () =>
						{
							await _packetSender.SendContent(ofd.FileName, ContentType.File);
						});
					}
				}
				else
				{
					var messageInfo = new MessageDetails(TxtMessage.Text);
					//LogMessage(fileinfo, TypeOfConnect.Send);
					TxtMessage.Text = "";
				}
			}
			catch (Exception ex)
			{
				Logging(ContentType.Information, ex.Message, TypeOfConnect.None);
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
