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
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilitiTools;
using Windows.Networking.NetworkOperators;
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
				StartHotspotAsync();
				_fireWall = FireWall.Instance;
				_fireWall.Begin();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Please disable your firewall.");
			}
		}

		private async Task StartHotspotAsync()
		{
			var installLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

			// Access the provisioning file
			var provisioningFile = await installLocation.GetFileAsync("ProvisioningData.xml");

			// Load with XML parser
			var xmlDocument = await XmlDocument.LoadFromFileAsync(provisioningFile);

			// Get raw XML
			var provisioningXml = xmlDocument.GetXml();

			// Create ProvisiongAgent Object
			var provisioningAgent = new ProvisioningAgent();

			try
			{
				// Create ProvisionFromXmlDocumentResults Object
				var result = await provisioningAgent.ProvisionFromXmlDocumentAsync(provisioningXml);

				if (result.AllElementsProvisioned)
				{
					rootPage.NotifyUser("Provisioning was successful", NotifyType.StatusMessage);
				}
				else
				{
					rootPage.NotifyUser("Provisioning result: " + result.ProvisionResultsXml, NotifyType.ErrorMessage);
				}
			}
			catch (System.Exception ex)
			{
				// See https://learn.microsoft.com/en-us/uwp/api/windows.networking.networkoperators.provisioningagent.provisionfromxmldocumentasync
				// for list of possible exceptions.
				rootPage.NotifyUser($"Unable to provision: {ex}", NotifyType.ErrorMessage);
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
