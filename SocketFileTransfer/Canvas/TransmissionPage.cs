using SocketFileTransfer.CustomControl;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
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
		private Socket _socket;
		private PacketSender _packetSender;

		public event EventHandler BackTransmissionRequest;

		public TransmissionPage(Socket socket)
		{
			InitializeComponent();
			_socket = socket;
		}

		private void TransmissionPage_Load(object sender, EventArgs e)
		{
			_packetSender = new(_socket);
			_packetSender.ProgressEventHandler += ProgressEvent;
			_packetSender.MessageEventHandler += MessageEvent;
			var buffer = new byte[8];
			_socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
		}

		private void MessageEvent(object sender, MessageReport e)
		{
			var control = PanelContainer.Controls.OfType<CPFile>().LastOrDefault();
			if (control != null)
			{
				control.ChangeMessage(e);
			}
		}

		private void ProgressEvent(object sender, ProgressReport e)
		{
			var control = PanelContainer.Controls.OfType<CPFile>().FirstOrDefault(a => a.Name == e.TargetedItemName);
			if (control != null)
			{
				control.ChangeProcess(e);
			}

		}

		private async void OnReceivedEnd(IAsyncResult ar)
		{
			try
			{
				var buffer = (byte[])ar.AsyncState;
				var received = _socket.EndReceive(ar);
				if (received == 0)
				{
					return;
				}
				var packetSize = Unsafe.ReadUnaligned<int>(ref buffer[0]);
				var restOfPacket = new byte[packetSize];
				await _socket.ReceiveAsync(restOfPacket);
				var fullPacket = new byte[packetSize + buffer.Length];
				Array.Copy(buffer, fullPacket, buffer.Length - 1);
				Array.Copy(restOfPacket, 0, fullPacket, buffer.Length, restOfPacket.Length);
				NetworkPacket networkPack = fullPacket;
				if (fullPacket is null)
				{
					buffer = new byte[8];
					_socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
					return;
				}
				else
				{
					ProcessNetWorkPack(networkPack);
					await _packetSender.ReceivedContent(networkPack);
					buffer = new byte[8];
					_socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceivedEnd, buffer);
				}
			}
			catch (Exception ex)
			{
				Logging(ContentType.Commend, "User is Disconnected", TypeOfConnect.None);
			}
		}
		void ProcessNetWorkPack(NetworkPacket fullPacket)
		{
			if (fullPacket.PacketType == ContentType.File)
			{
				var fileInfo = (FileDetails)fullPacket.Data;
				Logging(ContentType.File, fileInfo, TypeOfConnect.Received);
			}
			else if (fullPacket.PacketType == ContentType.Message)
			{
				var messageInfo = (MessageDetails)fullPacket.Data;
				Logging(ContentType.Message, messageInfo, TypeOfConnect.Received);
			}
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
						Logging(ContentType.File, fileinfo, TypeOfConnect.Send);
						await Task.Run(async () =>
						{
							await _packetSender.SendContent(ofd.FileName, ContentType.File);
						});
					}
				}
				else
				{

					var message = TxtMessage.Text;
					var messageInfo = new MessageDetails(message);
					Logging(ContentType.Message, messageInfo, TypeOfConnect.Send);
					TxtMessage.Text = "";
					await Task.Run(async () =>
					{
						await _packetSender.SendContent(message, ContentType.Message);
					});
				}
			}
			catch (Exception ex)
			{
				Logging(ContentType.Commend, ex.Message, TypeOfConnect.None);
			}
		}

		public void Logging(ContentType fileTypes, object info, TypeOfConnect typeOfConnect)
		{
			PanelContainer.InvokeFunctionInThreadSafeWay(() =>
			{
				switch (fileTypes)
				{
					case ContentType.File:
						PanelContainer.Controls.Add(new CPFile((FileDetails)info, typeOfConnect));
						break;
					case ContentType.Message:
						PanelContainer.Controls.Add(new CPFile((MessageDetails)info, typeOfConnect));
						break;
					case ContentType.Commend:
						PanelContainer.Controls.Add(new CPFile(typeOfConnect, (string)info));
						break;
					default:
						break;
				}
			});
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			if (!_socket.Connected || MessageBox.Show("Do you really want to left?", "Exit", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				BackTransmissionRequest.Raise(this, EventArgs.Empty);
			}
		}
	}
}