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

		public event EventHandler<Socket> OnTransmissionIpFound;

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
			var connectedDeviceDetails = ar.AsyncState as (DeviceDetails deviceDetails, Socket socket)?;
			if (!connectedDeviceDetails.HasValue)
				return;

			try
			{
				connectedDeviceDetails.Value.socket.EndConnect(ar);
				var device = await ProjectStandardUtilitiesHelper.ExchangeInformation(connectedDeviceDetails.Value.socket, TypeOfConnect.Send);

				if (device != null)
				{
					_clients.Add(device, (connectedDeviceDetails.Value.socket, connectedDeviceDetails.Value.deviceDetails));
					// need to look at _clients
					listBox1.InvokeFunctionInThreadSafeWay(() =>
					{
						listBox1.Items.Add($"{device}");
					});
					// start reading because when reading has issue thats means user disconnected.
				}
				else
				{
					connectedDeviceDetails.Value.socket.Dispose();
				}
			}
			catch
			{
				connectedDeviceDetails.Value.socket.Dispose();
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
				OnTransmissionIpFound.Raise(this, _clients[item].Item1);
			else
				MessageBox.Show("Failed to negotiate.");
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			//OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			//{
			//	EndPoint = null,
			//	TypeOfConnect = TypeOfConnect.None
			//});
		}
	}
}
