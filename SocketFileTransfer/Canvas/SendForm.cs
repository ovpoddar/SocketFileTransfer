﻿using SocketFileTransfer.ExtendClass;
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
using System.Reflection.Metadata.Ecma335;
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
		private readonly Dictionary<string, (NetworkStream, DeviceDetails)> _streams = new();

		public EventHandler<ConnectionDetails> OnTransmissionIpFound;

		public SendForm()
		{
			InitializeComponent();
		}

		private void StartScanForm_Load(object sender, EventArgs e)
		{
			// exclude the virtual machines
			var addresses = (from address in NetworkInterface.GetAllNetworkInterfaces()
					.Where(a => a.OperationalStatus == OperationalStatus.Up
						&& a.NetworkInterfaceType != NetworkInterfaceType.Loopback)
							 let networkInterfaceType = address.NetworkInterfaceType
							 let b = address.GetIPProperties().UnicastAddresses
							 from ip in b.Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
							 select (networkInterfaceType, ip));
			if (!addresses.Any())
				MessageBox.Show("No Device found");
			else
			{
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
				var stream = connectedDeviceDetails.Value.TcpClient.GetStream();
				var device = await ExchangeInformation(stream);
				// device is not unique here.
				if (device != null)
				{
					_streams.Add(device, (stream, connectedDeviceDetails.Value.DeviceDetails));
					if (listBox1.InvokeRequired)
					{
						listBox1.Invoke(() =>
						{
							listBox1.Items.Add($"{device}");
						});
					}
					else
					{
						listBox1.Items.Add($"{device}");
					}
					// start reading because when reading has issue thats means user disconnected.
				}
				else
				{
					stream.Close();
					connectedDeviceDetails.Value.TcpClient.Dispose();
				}
			}
			catch (Exception)
			{
				connectedDeviceDetails.Value.TcpClient.Dispose();
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
			_streams[item].Item1.Write(message);
			_streams[item].Item1.Flush();

			OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			{
				EndPoint = IPEndPoint.Parse(_streams[item].Item2.IP.ToString() + ":" + connectingPort),
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

		~SendForm()
		{
			foreach (var stream in _streams)
				stream.Value.Item1.Dispose();
		}
	}
}
