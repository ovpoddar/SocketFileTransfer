﻿using SocketFileTransfer.Configuration;
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
		private readonly Dictionary<string, TcpClientModel> _clients = new();

		public event EventHandler<SocketInformation?> OnTransmissionIpFound;

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
				&& !_clients.ContainsKey(newDevice))
			{
				var buffer = new byte[client.ReceiveBufferSize];
				client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, DataReceivedNew, newDevice);
				_clients.Add(newDevice, new TcpClientModel(client, buffer));
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
				var received = _clients[currentAdded].Socket.EndReceive(ar);
				var shouldConnect = ProjectStandardUtilitiesHelper.ReceivedConnectedSignal(_clients[currentAdded].Socket, _clients[currentAdded].Bytes, received);
				if (shouldConnect)
					OnTransmissionIpFound.Raise(this, _clients[currentAdded].Socket.DuplicateAndClose(Environment.ProcessId));
			}
			finally
			{
				if (_clients.ContainsKey(currentAdded))
					_clients[currentAdded].Socket.Dispose();
			}
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			OnTransmissionIpFound.Raise(this, EventArgs.Empty);
		}

	}
}
