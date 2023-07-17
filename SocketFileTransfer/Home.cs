using SocketFileTransfer.Canvas;
using SocketFileTransfer.Model;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SocketFileTransfer
{
	public partial class Home : Form
	{
		private Form _currentChildForm;

		public Home()
		{
			InitializeComponent();

			if (!TestForWIFIOrLanConnection()) 
				return;

			var indexPage = new Canvas.Index();
			indexPage.SelectItem += SelectConnectMethod;
			OpenChildForm(indexPage);
		}

		private static bool TestForWIFIOrLanConnection()
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
				return false;

			var netWorkInterfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();

			return netWorkInterfaces.Any(e =>
				(e.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
				e.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
				e.OperationalStatus == OperationalStatus.Up);
		}

		private void SelectConnectMethod(object sender, TypeOfConnect e)
		{
			switch (e)
			{
				case TypeOfConnect.Send:
					var connectSend = new SendForm();
					connectSend.OnTransmissionIPFound += GotTransmissionIp;
					OpenChildForm(connectSend);
					break;
				case TypeOfConnect.Received:
					var connectReceived = new ReceivedForm();
					connectReceived.OnTransmissionIPFound += GotTransmissionIp;
					OpenChildForm(connectReceived);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(e), e, null);
			}
			if (sender is Control control)
				control.Dispose();
		}

		private void GotTransmissionIp(object sender, Connection e)
		{
			if (e.TypeOfConnect == TypeOfConnect.None)
			{
				// work on back signal.
				if (e.ServerSockets != null)
				{
					foreach (var item in e.ServerSockets)
					{
						item.Close();
						item.Dispose();
					}
				}
				var indexPage = new Canvas.Index();
				indexPage.SelectItem += SelectConnectMethod;
				OpenChildForm(indexPage);
			}
			else
			{
				var transmissionPage = new TransmissionPage(e.Socket);
				transmissionPage.BackTransmissionRequest += GotTransmissionIp;
				OpenChildForm(transmissionPage);
			}
			if (sender is Control control)
				control.Dispose();
			GC.Collect();
		}

		private void BtnExit_Click(object sender, EventArgs e) =>
			Application.Exit();

		private void OpenChildForm(Form childForm)
		{
			//open only form
			_currentChildForm?.Close();
			_currentChildForm = childForm;
			//End
			childForm.TopLevel = false;
			childForm.FormBorderStyle = FormBorderStyle.None;
			childForm.Dock = DockStyle.Fill;
			panelBody.Controls.Add(childForm);
			panelBody.Tag = childForm;
			childForm.BringToFront();
			childForm.Show();
		}

		[DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
		private extern static void ReleaseCapture();
		[DllImport("user32.DLL", EntryPoint = "SendMessage")]
		private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

		private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			SendMessage(Handle, 0x112, 0xf012, 0);
		}

		private void BtnMinimize_Click(object sender, EventArgs e) =>
			WindowState = FormWindowState.Minimized;
	}
}
