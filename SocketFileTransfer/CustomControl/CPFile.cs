using SocketFileTransfer.Configuration;
using SocketFileTransfer.Model;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SocketFileTransfer.CustomControl
{
	public partial class CPFile : UserControl
	{
		private int _received = 0;

		public CPFile()
		{
			InitializeComponent();
		}
		public CPFile(string name, TypeOfConnect typeOfConnect)
		{
			InitializeComponent();

			LblName.Text = name;

			LblSize.Visible = false;
			LblType.Visible = false;

			LblName.Visible = true;

			SetBackground(typeOfConnect);
		}

		void SetBackground(TypeOfConnect typeOfConnect)
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

		public CPFile(FileDetails fileDetails, TypeOfConnect typeOfConnect)
		{
			InitializeComponent();

			LblName.Text = fileDetails.Name;
			LblSize.Text = Math.Round((decimal)fileDetails.Size / (1024 * 1024), 2, MidpointRounding.AwayFromZero).ToString();
			LblType.Text = fileDetails.Type;

			LblName.Visible = true;
			LblSize.Visible = true;
			LblType.Visible = true;

			LblName.Visible = false;

			SetBackground(typeOfConnect);
		}

		public void Write(NetworkPacket bytes)
		{
			var file = new FileStream(StaticConfiguration._storedLocation, FileMode.CreateNew);
			file.Seek(0, SeekOrigin.End);
			file.Write(bytes.Data, 0, bytes.Data.Length);
			_received++;
			//panel1.Width = (int)(decimal)(100 / _totalPercentage) * _received;
		}
	}
}
