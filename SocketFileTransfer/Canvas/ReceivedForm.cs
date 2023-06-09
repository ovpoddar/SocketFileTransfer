using SocketFileTransfer.Configuration;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas
{
	public partial class ReceivedForm : Form
	{
		private readonly Dictionary<string, TcpClientModel> _clients = new();
		private Socket _scanSocket;

		public event EventHandler<Connection> OnTransmissionIpFound;

		public ReceivedForm()
		{
			InitializeComponent();
		}

		private void ReceivedForm_Load(object sender, EventArgs e)
		{
			var addresses = ProjectStandardUtilitiesHelper.DeviceNetworkInterfaceDiscovery();

			if (!addresses.Any())
				LblMsg.Text = "Failed To start Your hotspot. Please do it manually or connect your self with a network cable which is connected with router.";
			else
			{
				foreach (var address in addresses)
					BrodCastSignal(address.Item2.Address);

				LblMsg.Text = "waiting for user to connect";
			}
		}

		private void BrodCastSignal(IPAddress address)
		{
			_scanSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_scanSocket.Bind(new IPEndPoint(address, StaticConfiguration.ApplicationRequiredPort));
			_scanSocket.Listen(100);
			_scanSocket.BeginAccept(BroadcastSignal, _scanSocket);
		}

		private async void BroadcastSignal(IAsyncResult ar)
		{
			try
			{
				var scanSocket = ar.AsyncState as Socket;
				var client = scanSocket.EndAccept(ar);
				var newDevice = await ProjectStandardUtilitiesHelper.ExchangeInformation(client, TypeOfConnect.Received);

				if (newDevice != null
					&& !_clients.ContainsKey(newDevice))
				{
					var buffer = new byte[client.ReceiveBufferSize];
					client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, DataReceivedNew, (newDevice, scanSocket));
					_clients.Add(newDevice, new TcpClientModel(client, buffer));
				}
				else
				{
					client.Dispose();
				}

				scanSocket.BeginAccept(BroadcastSignal, scanSocket);
			}
			catch (ObjectDisposedException ex) { }
			catch (Exception ex) { }
		}
		private void DataReceivedNew(IAsyncResult ar)
		{
			var models = ((string newdevice, Socket scanSocket))ar.AsyncState;
			try
			{
				var received = _clients[models.newdevice].Socket.EndReceive(ar);
				var shouldConnect = ProjectStandardUtilitiesHelper.ReceivedConnectedSignal(_clients[models.newdevice].Socket, _clients[models.newdevice].Bytes, received);
				if (shouldConnect)
				{
					var responce = new Connection
					{
						Socket = _clients[models.newdevice].Socket,
						TypeOfConnect = TypeOfConnect.Transmission,
						ServerSocket = models.scanSocket
					};
					_clients.Remove(models.newdevice);
					OnTransmissionIpFound.Raise(this, responce);
				}
			}
			catch (Exception ex) { }
			finally
			{
				if (_clients.ContainsKey(models.newdevice))
					_clients[models.newdevice].Socket.Dispose();
			}
		}

		private void BtnBack_Click(object sender, EventArgs e) =>
			OnTransmissionIpFound.Raise(this, new Connection(_scanSocket, TypeOfConnect.None));

	}
}
