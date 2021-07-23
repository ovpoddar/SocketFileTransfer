using SocketFileTransfer.CustomControl;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SocketFileTransfer.Pages
{
    public partial class Index : Form
    {
        public EventHandler<TypeOfConnect> SelectItem;
        public Index()
        {
            InitializeComponent();
        }

        private void Send_Click(object sender, EventArgs e)
        {
            SelectItem.Raise(this, TypeOfConnect.Send);
            Dispose();
        }

        private void BtnReceived_Click(object sender, EventArgs e)
        {
            SelectItem.Raise(this, TypeOfConnect.Received);
            this.Dispose();
        }

        private void OnMouseEnterButton1(object sender, EventArgs e)
        {
            var senderButton = sender as CButton;
            senderButton.BackColor = Color.FromArgb(63, 64, 64);
        }
        private void OnMouseLeaveButton1(object sender, EventArgs e)
        {
            var senderButton = sender as CButton;
            senderButton.BackColor = Color.FromArgb(96, 97, 97);
        }
    }
}
