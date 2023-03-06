using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UtilitiTools;
using WindowsFirewallHelper;

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

		void EstublishFireWall()
		{
			try
			{
				StartHotspot();
				_fireWall = new FireWall();
				_fireWall.Begin();
			}
			catch(Exception ex) 
			{
				MessageBox.Show("Please disable your firewall.");
			}
		}

		private void StartHotspot()
		{
			var id = WindowsIdentity.GetCurrent();
			var p = new WindowsPrincipal(id);
			if (p.IsInRole(WindowsBuiltInRole.Administrator))
			{
				var startInfo = new ProcessStartInfo();
				startInfo.UseShellExecute = true;
				startInfo.CreateNoWindow = true;
				startInfo.WorkingDirectory = Environment.CurrentDirectory;
				startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
				startInfo.Verb = "runas";
				Process.Start(startInfo);

				Application.Exit();
			}
			else
			{
				var processStartInfo = new ProcessStartInfo("cmd.exe")
				{
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					UseShellExecute = false
				};
				var process = Process.Start(processStartInfo);
				process.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=" + Dns.GetHostName());
				process.StandardInput.WriteLine("netsh wlan start hosted network");
				process.StandardInput.Close();
			}
		}

		private void ReceivedForm_Load(object sender, EventArgs e)
		{
			var thread = new Thread(() =>
			{
				var _addresses = NetworkInterface.GetAllNetworkInterfaces()
					.Where(a => a.OperationalStatus == OperationalStatus.Up)
					.SelectMany(a => a.GetIPProperties().UnicastAddresses)
					.Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
					.Select(a => a.Address.ToString())
					.ToList();


			});
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			_fireWall.Close();
			OnTransmissionIpFound.Raise(this, new ConnectionDetails()
			{
				TypeOfConnect = TypeOfConnect.None
			});
		}

	}
}
