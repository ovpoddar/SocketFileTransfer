﻿using Microsoft.VisualBasic.Devices;
using SocketFileTransfer.ExtendClass;
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
		private IEnumerable<(NetworkInterfaceType, UnicastIPAddressInformation)> _addresses;
		private readonly Dictionary<int, TcpClientModel> _clients = new Dictionary<int, TcpClientModel>();

		public EventHandler<ConnectionDetails> OnTransmissionIpFound;

		public ReceivedForm()
		{
			InitializeComponent();
		}

		private void ReceivedForm_Load(object sender, EventArgs e)
		{
			// all ipv4 of this pc
			_addresses = (from address in NetworkInterface.GetAllNetworkInterfaces()
					.Where(a => a.OperationalStatus == OperationalStatus.Up
						&& a.NetworkInterfaceType != NetworkInterfaceType.Loopback)
						  let networkInterfaceType = address.NetworkInterfaceType
						  let b = address.GetIPProperties().UnicastAddresses
						  from ip in b.Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
						  select (networkInterfaceType, ip));

			if (!_addresses.Any())
				LblMsg.Text = "Faild To start Your hotspot. Please do it maually or connect your self with a network cable which is connected with router.";
			else
			{
				foreach (var address in _addresses)
					BrodCastSignal(_addresses.First().Item2.Address);

				LblMsg.Text = "waiting for user to connect";
			}
		}

		private void BrodCastSignal(IPAddress address)
		{
			// test the parsing.
			var tcpListner = new TcpListener(address, 1400);
			tcpListner.Start();
			tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);
		}

		private async void BrodcastSignal(IAsyncResult ar)
		{
			var tcpListner = (TcpListener)ar.AsyncState;
			var client = tcpListner.EndAcceptTcpClient(ar);
			var stream = client.GetStream();
			var newDevice = await ExchangeInformation(stream);
			var managedClient = new TcpClientModel(newDevice, client);

			if (newDevice != null
				&& managedClient.IsCreationSucced
				&& !_clients.Any(a => a.Value.Name == managedClient.Name))
			{
				_currentAdded = _clients.Count;

				var bytes = new byte[client.ReceiveBufferSize];
				stream.BeginRead(bytes, 0, bytes.Length, DataReceived, _currentAdded);
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

		private async Task<string> ExchangeInformation(NetworkStream stream)
		{
			var currentDeviceName = Encoding.ASCII.GetBytes(Dns.GetHostName());
			stream.Write(currentDeviceName);
			stream.Flush();
			var connectedDeviceName = new byte[1024 * 4];
			var responce = await stream.ReadAsync(connectedDeviceName);
			if (responce == 0)
				return null;
			return Encoding.ASCII.GetString(connectedDeviceName);
		}

		private void DataReceived(IAsyncResult ar)
		{
			var currentAdded = (int)ar.AsyncState;
			var receved = _clients[currentAdded].Streams.EndRead(ar);

			try
			{
				if (receved == 0)
				{
					return;
				}
				var message = Encoding.ASCII.GetString(_clients[currentAdded].Data, 0, receved);
				if (message.StartsWith("@@Connected"))
				{
					var port = message.Split("::");
					OnTransmissionIpFound.Raise(this, new ConnectionDetails
					{
						EndPoint = IPEndPoint.Parse(_clients[currentAdded].RemoteEndPoient.Address.ToString() + ":" + port),
						TypeOfConnect = TypeOfConnect.Received
					});

					for (var i = 0; i < _currentAdded; i++)
					{
						_clients[i].Dispose();
					}
					Dispose();
				}
			}
			catch
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
			Dispose();
		}

		~ReceivedForm()
		{
			for (var i = 0; i < _currentAdded; i++)
				_clients[i].Dispose();
		}
	}
}
