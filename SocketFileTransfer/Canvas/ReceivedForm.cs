using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using UtilitiTools;
using ManagedNativeWifi;

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
			//var wifiAddress = new WlanClient();
			var thread = new Thread(async () =>
			{
				var _addresses = NetworkInterface.GetAllNetworkInterfaces()
					.Where(a => a.OperationalStatus == OperationalStatus.Up)
					.SelectMany(a => a.GetIPProperties().UnicastAddresses)
					.Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
					.Select(a => a.Address.ToString())
					.ToList();

				//var addr = wifiAddress.Interfaces
				//	.SelectMany(wlaninterfaces =>
				//		wlaninterfaces.GetAvailableNetworkList(Wlan.WlanGetAvailableNetworkFlags.IncludeAllManualHiddenProfiles)
				//		.Where(a => a.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.RSNA))
				//	;

				var c = WiFi.Instance.Scan();
			});
			thread.Start();
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
