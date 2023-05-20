using SocketFileTransfer.Model;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SocketFileTransfer.CustomControl
{
	public partial class CPFile : UserControl
	{

		public CPFile()
		{
			InitializeComponent();
		}
		public CPFile(string name, TypeOfConnect typeOfConnect)
		{
			InitializeComponent();

			LblName.Text = name;

			LblName.Visible = false;
			LblSize.Visible = false;
			LblType.Visible = false;

			LblName.Visible = true;

			SetBackground(typeOfConnect);
		}

		private void SetBackground(TypeOfConnect typeOfConnect)
		{
			BackColor = typeOfConnect switch
			{
				TypeOfConnect.Send => Color.Blue,
				TypeOfConnect.Received => Color.Green,
				TypeOfConnect.None => Color.Gray,
				_ => throw new NotImplementedException()
			};
			panel1.BackColor = typeOfConnect switch
			{
				TypeOfConnect.Send => SystemColors.ActiveCaption,
				TypeOfConnect.Received => SystemColors.ActiveCaption,
				TypeOfConnect.None => Color.Transparent,
				_ => throw new NotImplementedException()
			};
		}

		public CPFile(string filename, string fileSize, string filetype, TypeOfConnect typeOfConnect)
		{
			InitializeComponent();

			LblName.Text = filename;
			LblSize.Text = fileSize;
			LblType.Text = filetype;

			LblName.Visible = true;
			LblSize.Visible = true;
			LblType.Visible = true;

			LblName.Visible = false;

			SetBackground(typeOfConnect);
		}

		private void CPFile_Load(object sender, EventArgs e)
		{

		}
	}
}
