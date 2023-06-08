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
		private bool _scan;
		private readonly Dictionary<string, TcpClientModel> _clients = new();

		public event EventHandler<Connection> OnTransmissionIpFound;

		public ReceivedForm()
		{
			InitializeComponent();
			_scan = true;
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
			var scanSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			scanSocket.Bind(new IPEndPoint(address, StaticConfiguration.ApplicationRequiredPort));
			scanSocket.Listen(100);
			scanSocket.BeginAccept(BroadcastSignal, scanSocket);
		}

		private async void BroadcastSignal(IAsyncResult ar)
		{
			var scanSocket = ar as Socket;
			var client = scanSocket.EndAccept(ar);
			var newDevice = await ProjectStandardUtilitiesHelper.ExchangeInformation(client, TypeOfConnect.Received);

			if (newDevice != null
				&& !_clients.ContainsKey(newDevice))
			{
				var buffer = new byte[client.ReceiveBufferSize];
				client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, DataReceivedNew, newDevice);
				_clients.Add(newDevice, new TcpClientModel(client, buffer));
			}
			else
			{
				client.Dispose();
			}

			if(_scan)
				scanSocket.BeginAccept(BroadcastSignal, scanSocket);
		}
		private void DataReceivedNew(IAsyncResult ar)
		{
			var currentAdded = ar.AsyncState as string;
			try
			{
				var received = _clients[currentAdded].Socket.EndReceive(ar);
				var shouldConnect = ProjectStandardUtilitiesHelper.ReceivedConnectedSignal(_clients[currentAdded].Socket, _clients[currentAdded].Bytes, received);
				if (shouldConnect)
				{
					var responce = new Connection
					{
						Socket = _clients[currentAdded].Socket,
						TypeOfConnect = TypeOfConnect.Transmission
					};
					_clients.Remove(currentAdded);
					_scan = false;
					OnTransmissionIpFound.Raise(this, responce);
				}
			}
			finally
			{
				if (_clients.ContainsKey(currentAdded))
					_clients[currentAdded].Socket.Dispose();
			}
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			OnTransmissionIpFound.Raise(this, new Connection(TypeOfConnect.None));
		}

	}
}
