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
		private Dictionary<int, Socket> _scanSockets = new();
		private bool _isFinalized = false;

		public event EventHandler<Connection> OnTransmissionIPFound;

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
				var index = 0;
				foreach (var address in addresses)
				{
					var scanSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					scanSocket.Bind(new IPEndPoint(address.Item2.Address, ConfigurationSetting.ApplicationRequiredPort));
					scanSocket.Listen(100);
					scanSocket.BeginAccept(BroadcastSignal, index);
					_scanSockets.Add(index, scanSocket);
					index++;
				}

				LblMsg.Text = "waiting for user to connect";
			}
		}


		private async void BroadcastSignal(IAsyncResult ar)
		{
			try
			{
				var index = (int)ar.AsyncState;
				var client = _scanSockets[index].EndAccept(ar);
				var newDevice = await ProjectStandardUtilitiesHelper.ExchangeInformation(client, TypeOfConnect.Received);

				if (newDevice != null
					&& !_clients.ContainsKey(newDevice))
				{
					var buffer = new byte[client.ReceiveBufferSize];
					client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, DataReceivedNew, (newDevice, _scanSockets[index], index));
					_clients.Add(newDevice, new TcpClientModel(client, buffer));
				}
				else
				{
					client.Dispose();
				}
				if (_isFinalized)
					return;
				_scanSockets[index].BeginAccept(BroadcastSignal, index);
			}
			catch (ObjectDisposedException) { }
			catch (Exception) { }
		}

		private void DataReceivedNew(IAsyncResult ar)
		{
			var (newdevice, scanSocket, index) = ((string newdevice, Socket scanSocket, int index))ar.AsyncState;
			try
			{
				var received = _clients[newdevice].Socket.EndReceive(ar);
				var shouldConnect = ProjectStandardUtilitiesHelper.ReceivedConnectedSignal(_clients[newdevice].Socket, _clients[newdevice].Bytes, received);
				if (shouldConnect)
				{
					var responce = new Connection
					{
						Socket = _clients[newdevice].Socket,
						TypeOfConnect = TypeOfConnect.Transmission,
						ServerSockets = new Socket[]
						{
							scanSocket
						}
					};
					_isFinalized = true;
					_clients.Remove(newdevice);
					OnTransmissionIPFound.Raise(this, responce);

					foreach (var ss in _scanSockets)
					{
						if (ss.Key != index)
						{
							ss.Value.Close();
							ss.Value.Dispose();
							_scanSockets.Remove(ss.Key);
						}
					}
				}
			}
			catch (Exception)
			{
				if (!_clients.ContainsKey(newdevice))
					return;
				_clients[newdevice].Socket.Dispose();
				_clients.Remove(newdevice);
			}
			finally
			{
				if (_clients.ContainsKey(newdevice))
					_clients[newdevice].Socket.Dispose();
			}
		}

		private void BtnBack_Click(object sender, EventArgs e) =>
			OnTransmissionIPFound.Raise(this, new Connection(_scanSockets.Select(a => a.Value).ToArray(), TypeOfConnect.None));

	}
}
