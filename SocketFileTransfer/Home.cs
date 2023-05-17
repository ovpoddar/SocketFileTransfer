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
					var connectSend = new SendForm();
					connectSend.OnTransmissionIpFound += GotTransmissionIp;
					OpenChildForm(connectSend);
					break;
				case TypeOfConnect.Received:
					var connectReceived = new ReceivedForm();
					connectReceived.OnTransmissionIpFound += GotTransmissionIp;
					OpenChildForm(connectReceived);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(e), e, null);
			}
		}

		private void GotTransmissionIp(object sender, ConnectionDetails e)
		{
			switch (e.TypeOfConnect)
			{
				case TypeOfConnect.Send:
				case TypeOfConnect.Received:
					OpenChildForm(new TransmissionPage(e));
					break;
				case TypeOfConnect.None:
					var indexPage = new Canvas.Index();
					indexPage.SelectItem += SelectConnectMethod;
					OpenChildForm(indexPage);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(e), e, null);


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
