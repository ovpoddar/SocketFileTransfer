using Microsoft.VisualBasic.Devices;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilitiTools;

namespace SocketFileTransfer.Canvas
{
	public partial class ReceivedForm : Form
	{
		private int _currentAdded;
		private readonly Dictionary<int, TcpClientModel> _clients = new Dictionary<int, TcpClientModel>();

		public EventHandler<ConnectionDetails> OnTransmissionIpFound;

		public ReceivedForm()
		{
			InitializeComponent();
		}

		private void ReceivedForm_Load(object sender, EventArgs e)
		{
			var addresses = ProjectStandaredUtilitiesHelper.DeviceNetworkInterfaceDiscovery();

			if (!addresses.Any())
				LblMsg.Text = "Faild To start Your hotspot. Please do it maually or connect your self with a network cable which is connected with router.";
			else
			{
				foreach (var address in addresses)
					BrodCastSignal(address.Item2.Address);

				LblMsg.Text = "waiting for user to connect";
			}
		}

		private void BrodCastSignal(IPAddress address)
		{
			var tcpListner = new TcpListener(address, 1400);
			tcpListner.Start();
			tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);
		}

		private async void BrodcastSignal(IAsyncResult ar)
		{
			var tcpListner = (TcpListener)ar.AsyncState;
			var client = tcpListner.EndAcceptTcpClient(ar);
			var newDevice = await ProjectStandaredUtilitiesHelper.ExchangeInformation(client, TypeOfConnect.Received);
			var managedClient = new TcpClientModel(newDevice, client);

			if (newDevice != null
				&& managedClient.IsCreationSucced
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
				if (managedClient.IsCreationSucced)
					managedClient.Dispose();
			}
			tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);

		}

		private void DataReceived(IAsyncResult ar)
		{
			var currentAdded = (int)ar.AsyncState;

			try
			{
				var receved = _clients[currentAdded].Streams.EndRead(ar);
				var port = ProjectStandaredUtilitiesHelper.ReceivedTheConnectionPort(_clients[currentAdded].Data, receved);
				if (!string.IsNullOrWhiteSpace(port))
					OnTransmissionIpFound.Raise(this, new ConnectionDetails
					{
						EndPoint = IPEndPoint.Parse(_clients[currentAdded].Clients.Client.LocalEndPoint.ToString().Split(":")[0] + ":" + port[1]),
						TypeOfConnect = TypeOfConnect.Received
					});
			}
			finally
			{
				_clients[currentAdded].Dispose();
			}
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			OnTransmissionIpFound.Raise(this, new ConnectionDetails
			{
				TypeOfConnect = TypeOfConnect.None,
				EndPoint = null
			});
		}

		~ReceivedForm()
		{
			for (var i = 0; i < _currentAdded; i++)
				_clients[i].Dispose();
		}
	}
}
