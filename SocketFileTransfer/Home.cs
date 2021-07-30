using SocketFileTransfer.Model;
using SocketFileTransfer.Pages;
using System;
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
     
            if (TestForWIFIOrLanConnection())
            {
                var indexpage = new Pages.Index();
                indexpage.SelectItem += SelectConnectMethod;
                OpenChildForm(indexpage);
            }

        }

        private bool TestForWIFIOrLanConnection()
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) return false;

            var interfaceTest = false;

            foreach (var netWorkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                interfaceTest =
                    //NetWorkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    netWorkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                    netWorkInterface.OperationalStatus == OperationalStatus.Up;
            }

            return interfaceTest;
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
            }
        }

        private void GotTransmissionIp(object sender, ConnectionDetails e)
        {
            OpenChildForm(new TransmissionPage(e));
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenChildForm(Form childForm)
        {
            //open only form
            if (_currentChildForm != null)
                _currentChildForm.Close();
            
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
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
