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
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas
{
	public partial class SendForm : Form
	{
		private readonly Dictionary<string, (Socket, DeviceDetails)> _clients = new();

		public event EventHandler<Connection> OnTransmissionIpFound;

		public SendForm()
		{
			InitializeComponent();
		}

		private void StartScanForm_Load(object sender, EventArgs e)
		{
			var addresses = ProjectStandardUtilitiesHelper.DeviceNetworkInterfaceDiscovery();

			if (!addresses.Any())
				MessageBox.Show("No Device found");
			else
			{
				// combine into task then wait for completion
				foreach (var address in addresses)
					FindDevices(address);
			}
		}

		private void FindDevices((NetworkInterfaceType, UnicastIPAddressInformation) address)
		{
			var arp = new ArpRequestHandler(address.Item2.Address, address.Item1);
			arp.OnDeviceFound += DeviceFound;
			_ = arp.GetNetWorkDevices();
		}

		private void DeviceFound(object sender, DeviceDetails e)
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

		private async void StartConnecting(IAsyncResult ar)
		{
			var connectedDeviceDetails = ((DeviceDetails deviceDetails, Socket socket))ar.AsyncState;

			try
			{
				connectedDeviceDetails.socket.EndConnect(ar);
				var device = await ProjectStandardUtilitiesHelper.ExchangeInformation(connectedDeviceDetails.socket, TypeOfConnect.Send);

				if (device != null)
				{
					_clients.Add(device, (connectedDeviceDetails.socket, connectedDeviceDetails.deviceDetails));
					// need to look at _clients
					listBox1.InvokeFunctionInThreadSafeWay(a =>
					{
						a.Items.Add($"{device}");
					});
					// start reading because when reading has issue thats means user disconnected.
					// then on base of the user id remove the entry from list and from ui
				}
				else
				{
					connectedDeviceDetails.socket.Dispose();
				}
			}
			catch
			{
				connectedDeviceDetails.socket.Dispose();
			}
		}

		private async void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1)
				return;
			var item = listBox1.SelectedItem.ToString();
			if (!_clients.ContainsKey(item))
				listBox1.Items.Remove(item);

			var port = await ProjectStandardUtilitiesHelper.SendConnectSignal(_clients[item].Item1);
			if (port)
			{
				var responce = new Connection
				{
					Socket = _clients[item].Item1,
					TypeOfConnect = TypeOfConnect.Transmission
				};
				_clients.Remove(item);
				OnTransmissionIpFound.Raise(this, responce);
			}
			else
				MessageBox.Show("Failed to negotiate.");
			// dispose all the rest clients
		}

		private void BtnBack_Click(object sender, EventArgs e) =>
			OnTransmissionIpFound.Raise(this, new Connection(TypeOfConnect.None));

	}
}
