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
		private int _currentAdded;
		private readonly Dictionary<int, TcpClientModel> _clients = new Dictionary<int, TcpClientModel>();

		public event EventHandler<ConnectionDetails> OnTransmissionIpFound;

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
			var tcpListener = new TcpListener(address, 1400);
			tcpListener.Start();
			tcpListener.BeginAcceptTcpClient(BroadcastSignal, tcpListener);
		}

		private async void BroadcastSignal(IAsyncResult ar)
		{
			var tcpListener = (TcpListener)ar.AsyncState;
			var client = tcpListener.EndAcceptTcpClient(ar);
			var newDevice = await ProjectStandardUtilitiesHelper.ExchangeInformation(client, TypeOfConnect.Received);
			var managedClient = new TcpClientModel(newDevice, client);

			if (newDevice != null
				&& managedClient.IsCreationSucceed
				&& !_clients.Any(a => a.Value.Name == managedClient.Name))
			{
				_currentAdded = _clients.Count;

				var bytes = new byte[client.ReceiveBufferSize];
				client.GetStream().BeginRead(bytes, 0, bytes.Length, DataReceived, _currentAdded);
				managedClient.Data = bytes;
				_clients.Add(_currentAdded, managedClient);
			}
			else
			{
				if (managedClient.IsCreationSucceed)
					managedClient.Dispose();
			}
			tcpListener.BeginAcceptTcpClient(BroadcastSignal, tcpListener);

		}

		private void DataReceived(IAsyncResult ar)
		{
			var currentAdded = (int)ar.AsyncState;

			try
			{
				var received = _clients[currentAdded].Streams.EndRead(ar);
				var port = ProjectStandardUtilitiesHelper.ReceivedTheConnectionPort(_clients[currentAdded].Clients, _clients[currentAdded].Data, received);
				if (!string.IsNullOrWhiteSpace(port))
					OnTransmissionIpFound.Raise(this, new ConnectionDetails
					{
						EndPoint = IPEndPoint.Parse(_clients[currentAdded].Clients.Client.LocalEndPoint.ToString().Split(":")[0] + ":" + port),
						TypeOfConnect = TypeOfConnect.Received
					});
			}
			finally
			{
				if (_clients.ContainsKey(currentAdded))
					_clients[currentAdded].Dispose();
			}
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			{
				TypeOfConnect = TypeOfConnect.None
			});
		}

	}
}
