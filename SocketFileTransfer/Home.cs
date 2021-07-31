using SocketFileTransfer.Canvas;
using SocketFileTransfer.Model;
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
            var check = TestForWIFIOrLanConnection();
            if (check)
            {
                var indexpage = new Canvas.Index();
                indexpage.SelectItem += SelectConnectMethod;
                OpenChildForm(indexpage);
            }

        }

        private bool TestForWIFIOrLanConnection()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                foreach (var NetWorkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if ((
                        //NetWorkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                        NetWorkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                        NetWorkInterface.OperationalStatus == OperationalStatus.Up)
                        return true;
                }
            }
            return false;
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
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
