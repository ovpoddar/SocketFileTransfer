using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilitiTools;
using UtilitiTools.Models;

namespace SocketFileTransfer.Canvas
{
	public partial class SendForm : Form
	{
		private IEnumerable<(NetworkInterfaceType, UnicastIPAddressInformation)> _addresses;
		private Dictionary<string, NetworkStream> _streams = new();
		private Dictionary<string, DeviceDetails> _address = new();

		public EventHandler<ConnectionDetails> OnTransmissionIpFound;

		public SendForm()
		{
			InitializeComponent();
		}

		private void StartScanForm_Load(object sender, EventArgs e)
		{
			_addresses = (from address in NetworkInterface.GetAllNetworkInterfaces()
					.Where(a => a.OperationalStatus == OperationalStatus.Up
						&& a.NetworkInterfaceType != NetworkInterfaceType.Loopback)
						  let networkInterfaceType = address.NetworkInterfaceType
						  let b = address.GetIPProperties().UnicastAddresses
						  from ip in b.Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
						  select (networkInterfaceType, ip));
			if (!_addresses.Any())
				MessageBox.Show("No Device found");
			else
			{
				foreach (var address in _addresses)
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
				var deviceDetails = new ConnectDevice()
				{
					DeviceDetails = e,
					TcpClient = tcpClient
				};
				tcpClient.BeginConnect(e.IP, 1400, ConnectToEndPoient, deviceDetails);
			}
			catch
			{
				tcpClient.Dispose();
			}
		}

		private async void ConnectToEndPoient(IAsyncResult ar)
		{
			var connectedDeviceDetails = ar.AsyncState as ConnectDevice;
			try
			{
				connectedDeviceDetails.TcpClient.EndConnect(ar);
				var stream = connectedDeviceDetails.TcpClient.GetStream();
				var device = await ExchangeInformation(stream);
				// device is not unique here.
				if (device != null)
				{
					_streams.Add(device, stream);
					_address.Add(device, connectedDeviceDetails.DeviceDetails);
					if (listBox1.InvokeRequired)
					{
						listBox1.Invoke(() =>
						{
							listBox1.Items.Add($"{device} {connectedDeviceDetails.DeviceDetails.InterfaceType.ToString()}");
						});
					}
					else
					{
						listBox1.Items.Add($"{device} {connectedDeviceDetails.DeviceDetails.InterfaceType.ToString()}");
					}
					// start reading because when reading has issue thats means user disconnected.
				}
				else
				{
					stream.Close();
					connectedDeviceDetails.TcpClient.Dispose();
				}
			}
			catch (Exception)
			{
				connectedDeviceDetails.TcpClient.Dispose();
			}
		}

		private async Task<string> ExchangeInformation(NetworkStream stream)
		{
			var connectedDeviceName = new byte[1024 * 4];
			var responce = await stream.ReadAsync(connectedDeviceName);
			if (responce == 0)
				return null;
			var currentDeviceName = Encoding.ASCII.GetBytes(Dns.GetHostName());
			stream.Write(currentDeviceName);
			stream.Flush();
			return Encoding.ASCII.GetString(connectedDeviceName);
		}

		private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = listBox1.SelectedItem.ToString();
			if (!_streams.ContainsKey(item))
				listBox1.Items.Remove(item);

			var connectingPort = GeneratePort();
			var message = Encoding.ASCII.GetBytes($"@@Connected::{connectingPort}").AsSpan();
			_streams[item].Write(message);
			_streams[item].Flush();
			OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			{
				EndPoint = IPEndPoint.Parse(_address[item].IP.ToString() + ":" + connectingPort),
				TypeOfConnect = TypeOfConnect.Send
			});
		}

		string GeneratePort()
		{
			Random random = new Random(DateTime.UtcNow.Month);
			var port = random.Next(1000, 1200);
			return port.ToString();
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{

			OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			{
				EndPoint = null,
				TypeOfConnect = TypeOfConnect.None
			});
		}
	}
	file class ConnectDevice
	{
		public TcpClient TcpClient { get; set; }
		public DeviceDetails DeviceDetails { get; set; }
	}
}
