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
		private readonly Dictionary<string, (TcpClient, DeviceDetails)> _clients = new();

		public EventHandler<ConnectionDetails> OnTransmissionIpFound;

		public SendForm()
		{
			InitializeComponent();
		}

		private void StartScanForm_Load(object sender, EventArgs e)
		{
			var addresses = ProjectStandaredUtilitiesHelper.DeviceNetworkInterfaceDiscovery();

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
			var tcpClient = new TcpClient();
			try
			{
				tcpClient.BeginConnect(e.IP, 1400, ConnectToEndPoient, (e, tcpClient));
			}
			catch
			{
				tcpClient.Dispose();
			}
		}

		private async void ConnectToEndPoient(IAsyncResult ar)
		{
			var connectedDeviceDetails = ar.AsyncState as (DeviceDetails DeviceDetails, TcpClient TcpClient)?;
			if (!connectedDeviceDetails.HasValue)
				return;

			try
			{
				connectedDeviceDetails.Value.TcpClient.EndConnect(ar);
				var device = await ProjectStandaredUtilitiesHelper.ExchangeInformation(connectedDeviceDetails.Value.TcpClient, TypeOfConnect.Send);

				// device is not unique here.
				if (device != null)
				{
					// need to look at connectedDeviceDetails.Value.DeviceDetails.NetworkInterfaceType
					_clients.Add(device, (connectedDeviceDetails.Value.TcpClient, connectedDeviceDetails.Value.DeviceDetails));
					listBox1.InvokeFunctionInThradeSafeWay(() =>
					{
						listBox1.Items.Add($"{device}");
					});
					// start reading because when reading has issue thats means user disconnected.
				}
				else
				{
					connectedDeviceDetails.Value.TcpClient.Dispose();
				}
			}
			catch (Exception)
			{
				connectedDeviceDetails.Value.TcpClient.Dispose();
			}
		}

		private async void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = listBox1.SelectedItem.ToString();
			if (!_clients.ContainsKey(item))
				listBox1.Items.Remove(item);

			var port = await ProjectStandaredUtilitiesHelper.SendConnectSignalWithPort(_clients[item].Item1);
			if (port != null)
				OnTransmissionIpFound.Raise(this, new ConnectionDetails()
				{
					EndPoint = IPEndPoint.Parse(_clients[item].Item2.IP.ToString() + ":" + port),
					TypeOfConnect = TypeOfConnect.Send
				});
			else
				MessageBox.Show("Failed to negotiate.");
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			{
				EndPoint = null,
				TypeOfConnect = TypeOfConnect.None
			});
		}

		~SendForm()
		{
			foreach (var stream in _clients)
				stream.Value.Item1.Dispose();
		}
	}
}
