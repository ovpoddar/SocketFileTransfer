using SocketFileTransfer.CustomControl;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas
{
	public partial class Index : Form
	{
		public EventHandler<TypeOfConnect> SelectItem;
		public event NotifyUser ProcessDone;

		public delegate void NotifyUser();

		public virtual void OnProcessDone()
		{
			ProcessDone.Invoke();
		}

		public Index()
		{
			InitializeComponent();
		}

		private void Send_Click(object sender, EventArgs e)
		{
			SelectItem.Raise(this, TypeOfConnect.Send);
			this.Dispose();
		}

		private void BtnReceived_Click(object sender, EventArgs e)
		{
			SelectItem.Raise(this, TypeOfConnect.Received);
			this.Dispose();
		}

		private void OnMouseEnterButton(object sender, EventArgs e)
		{
			var senderButton = sender as CButton;
			senderButton.BackColor = Color.FromArgb(63, 64, 64);
		}
		private void OnMouseLeaveButton(object sender, EventArgs e)
		{
			var senderButton = sender as CButton;
			senderButton.BackColor = Color.FromArgb(96, 97, 97);
		}
	}
}
