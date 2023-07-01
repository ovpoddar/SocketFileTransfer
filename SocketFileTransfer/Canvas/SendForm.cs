using SocketFileTransfer.Configuration;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas
{
	public partial class SendForm : Form
	{
		//TODO: need a rescan button
		private readonly Dictionary<string, (Socket, DeviceDetails)> _clients = new();
		private CancellationTokenSource _cancellationTokenSource;
		private readonly int _totalAddress;
		private int _currentSceanAddress;

		public event EventHandler<Connection> OnTransmissionIPFound;

		public SendForm()
		{
			InitializeComponent();

			var addresses = ProjectStandardUtilitiesHelper.DeviceNetworkInterfaceDiscovery();
			_totalAddress = addresses.Count();
		}

		private void StartScanForm_Load(object sender, EventArgs e)
		{
			Scan();
		}

		void Scan()
		{
			var addresses = ProjectStandardUtilitiesHelper.DeviceNetworkInterfaceDiscovery();
			_cancellationTokenSource = new();
			if (!addresses.Any())
				MessageBox.Show("No Device found");
			else
			{
				// combine into task then wait for completion
				foreach (var address in addresses)
					FindDevices(address);
			}
		}

		async void FindDevices((NetworkInterfaceType, UnicastIPAddressInformation) address)
		{
			var arp = new ArpRequestHandler(address.Item2.Address, address.Item1);
			arp.OnDeviceFound += DeviceFound;
			arp.OnScanComplete += ScanCompleted;
			await arp.GetNetWorkDevices(_cancellationTokenSource.Token);
		}

		void ScanCompleted(object sender, EventArgs e)
		{
			_currentSceanAddress++;
			if (_currentSceanAddress == _totalAddress)
			{
				TaskButton.Text = "Rescan";
			}
		}

		void DeviceFound(object sender, DeviceDetails e)
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				var connectingPort = new IPEndPoint(e.IP, StaticConfiguration.ApplicationRequiredPort);
				socket.BeginConnect(connectingPort, StartConnecting, (e, socket));
			}
			catch
			{
				socket.Dispose();
			}
		}

		async void StartConnecting(IAsyncResult ar)
		{
			var (deviceDetails, socket) = ((DeviceDetails deviceDetails, Socket socket))ar.AsyncState;

			try
			{
				socket.EndConnect(ar);
				var device = await ProjectStandardUtilitiesHelper.ExchangeInformation(socket, TypeOfConnect.Send);

				if (device != null)
				{
					if (_clients.ContainsKey(device))
						throw new Exception("Already have an connection.So throw it for disposal.");

					_clients.Add(device, (socket, deviceDetails));
					listBox1.InvokeFunctionInThreadSafeWay(a =>
					{
						a.Items.Add($"{device}");
					});
					var buffer = new byte[16];
					socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, BeginMoniter, (deviceDetails, socket, device));
				}
				else
				{
					socket.Dispose();
				}
			}
			catch
			{
				socket.Dispose();
			}
		}

		void BeginMoniter(IAsyncResult ar)
		{
			try
			{
				var (deviceDetails, socket, device) = ((DeviceDetails deviceDetails, Socket socket, string device))ar.AsyncState;
				listBox1.InvokeFunctionInThreadSafeWay(ar =>
				{
					ar.Items.Remove($"{device}");
				});
				_clients.Remove(device);
				socket.Disconnect(true);
				socket.Dispose();
			}
			catch
			{

			}
		}

		private async void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1)
				return;
			var item = listBox1.SelectedItem.ToString();
			if (!_clients.ContainsKey(item))
				listBox1.Items.Remove(item);

			var isConnected = await ProjectStandardUtilitiesHelper.SendConnectSignal(_clients[item].Item1);
			if (isConnected)
			{
				var responce = new Connection
				{
					Socket = _clients[item].Item1,
					TypeOfConnect = TypeOfConnect.Transmission
				};
				_clients.Remove(item);
				OnTransmissionIPFound.Raise(this, responce);
			}
			else
				MessageBox.Show("Failed to negotiate.");
			// dispose all the rest clients
		}

		private void BtnBack_Click(object sender, EventArgs e) =>
			OnTransmissionIPFound.Raise(this, new Connection(TypeOfConnect.None));

		private void TaskButton_Click(object sender, EventArgs e)
		{
			if (TaskButton.Text == "Rescan")
			{
				TaskButton.Text = "Cancel";
				foreach (var item in _clients)
				{
					item.Value.Item1.Dispose();
				}
				_clients.Clear();
				listBox1.Items.Clear();
				_currentSceanAddress = 0;
				Scan();
			}
			else
			{
				TaskButton.Text = "Rescan";
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource.Dispose();
			}

		}
	}
}
