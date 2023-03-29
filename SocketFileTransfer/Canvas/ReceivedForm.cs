using Microsoft.VisualBasic.Devices;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using UtilitiTools;

namespace SocketFileTransfer.Canvas
{
	public partial class ReceivedForm : Form
	{
		private FireWall _fireWall;
		public EventHandler<ConnectionDetails> OnTransmissionIpFound;

		public ReceivedForm()
		{
			InitializeComponent();
			EstublishFireWall();
		}

		async void EstublishFireWall()
		{
			try
			{
				Hotspot.Start();
				_fireWall = FireWall.Instance;
				_fireWall.Begin();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Please disable your firewall.");
			}
		}

		private void ReceivedForm_Load(object sender, EventArgs e)
		{
			// all ipv4 of this pc
			var _addresses = (from address in NetworkInterface.GetAllNetworkInterfaces()
					.Where(a => a.OperationalStatus == OperationalStatus.Up
						&& a.NetworkInterfaceType != NetworkInterfaceType.Loopback)
							  let networkInterfaceType = address.NetworkInterfaceType
							  let b = address.GetIPProperties().UnicastAddresses
							  from ip in b.Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
							  select (networkInterfaceType, ip));

			var thread = new Thread(async () =>
			{				

				if (!_addresses.Any())
					LblMsg.Text = "Faild To start Your hotspot. Please do it maually or connect your self with a network cable which is connected with router.";
				else
				{
					BrodCastSignal();
					LblMsg.Text = "waiting for user to connect";
				}
			});
			thread.Start();
		}

		private void BrodCastSignal()
		{
			var tcpListner = new TcpListener(IPAddress.Any, 1400);
			tcpListner.Start();
			tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);
		}

		private void BrodcastSignal(IAsyncResult ar)
		{
			var tcpListner = (TcpListener)ar.AsyncState;
			var client = tcpListner.EndAcceptTcpClient(ar);
			// call the taking about scanning and store the channel and keep farther listening for farther listening
			tcpListner.BeginAcceptTcpClient(BrodcastSignal, tcpListner);
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			_fireWall.Close();
			OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			{
				TypeOfConnect = TypeOfConnect.None
			});
		}

		~ReceivedForm()
		{
			_fireWall.Close();
		}
	}
}
