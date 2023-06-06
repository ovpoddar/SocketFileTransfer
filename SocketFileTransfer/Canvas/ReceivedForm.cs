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
		private readonly Dictionary<string, TcpClientModel> _newClients = new();

		public event EventHandler<Socket> OnNewTransmissionIpFound;

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
			var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			serverSocket.Bind(new IPEndPoint(address, StaticConfiguration.ApplicationRequiredPort));
			serverSocket.Listen(100);
			serverSocket.BeginAccept(BroadcastSignal, serverSocket);
		}

		private async void BroadcastSignal(IAsyncResult ar)
		{
			var serverSocket = ar.AsyncState as Socket;
			var client = serverSocket.EndAccept(ar);
			var newDevice = await ProjectStandardUtilitiesHelper.ExchangeInformation(client, TypeOfConnect.Received);

			if (newDevice != null
				&& !_newClients.ContainsKey(newDevice))
			{
				var buffer = new byte[client.ReceiveBufferSize];
				client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, DataReceivedNew, newDevice);
				_newClients.Add(newDevice, new TcpClientModel(client, buffer));
			}
			else
			{
				client.Dispose();
			}

			serverSocket.BeginAccept(BroadcastSignal, serverSocket);
		}
		private void DataReceivedNew(IAsyncResult ar)
		{
			var currentAdded = ar.AsyncState as string;
			try
			{
				var received = _newClients[currentAdded].Socket.EndReceive(ar);
				var shouldConnect = ProjectStandardUtilitiesHelper.ReceivedConnectedSignal(_newClients[currentAdded].Bytes, received);
				if (shouldConnect)
					OnNewTransmissionIpFound.Raise(this, _newClients[currentAdded]);
			}
			finally
			{
				if (_newClients.ContainsKey(currentAdded))
					_newClients[currentAdded].Socket.Dispose();
			}
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			//OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			//{
			//	TypeOfConnect = TypeOfConnect.None
			//});
		}

	}
}
