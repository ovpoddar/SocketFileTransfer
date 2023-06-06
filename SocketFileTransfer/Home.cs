using SocketFileTransfer.Canvas;
using SocketFileTransfer.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SocketFileTransfer
{
	public partial class Home : Form
	{
		private Form _currentChildForm;
		private Socket _communicationSocket;

		public Home()
		{
			InitializeComponent();

			if (!TestForWIFIOrLanConnection()) return;

			var indexPage = new Canvas.Index();
			indexPage.SelectItem += SelectConnectMethod;
			OpenChildForm(indexPage);
		}

		private bool TestForWIFIOrLanConnection()
		{
			if (!NetworkInterface.GetIsNetworkAvailable()) return false;

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
					var connectSend = new SendForm(ref _communicationSocket);
					connectSend.OnTransmissionIpFound += GotTransmissionIp;
					OpenChildForm(connectSend);
					break;
				case TypeOfConnect.Received:
					var connectReceived = new ReceivedForm(ref _communicationSocket);
					connectReceived.OnTransmissionIpFound += GotTransmissionIp;
					OpenChildForm(connectReceived);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(e), e, null);
			}
			var control = sender as Control;
			if (control != null)
				control.Dispose();
		}

		private void GotTransmissionIp(object sender, TypeOfConnect e)
		{
			if (e == TypeOfConnect.None)
			{
				var indexPage = new Canvas.Index();
				indexPage.SelectItem += SelectConnectMethod;
				OpenChildForm(indexPage);
				//back signal
			}
			else
			{
				Debug.Assert(e == TypeOfConnect.Transmission);
				var transmissionPage = new TransmissionPage(_communicationSocket);
				transmissionPage.BackTransmissionRequest += GotTransmissionIp;
				OpenChildForm(transmissionPage);
			}
		}

		private void BtnExit_Click(object sender, EventArgs e) =>
			Application.Exit();

		private void OpenChildForm(Form childForm)
		{
			//open only form
			if (_currentChildForm != null)
			{
				_currentChildForm.Close();
			}
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
