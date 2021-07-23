using SocketFileTransfer.Model;
using SocketFileTransfer.Pages;
using System;
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
            var indexpage = new Pages.Index();
            indexpage.SelectItem += SelectConnectMethod;
            OpenChildForm(indexpage);
        }

        private void SelectConnectMethod(object sender, TypeOfConnect e)
        {
            switch (e)
            {
                case TypeOfConnect.Send:
                    OpenChildForm(new SendForm());
                    break;
                case TypeOfConnect.Received:
                    OpenChildForm(new ReceivedForm());
                    break;
            }
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
