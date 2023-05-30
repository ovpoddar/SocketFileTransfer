using SocketFileTransfer.Configuration;
using SocketFileTransfer.Model;
using System;
using System.Drawing;
using System.IO;
using System.Text;
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
			ProgresPanel.BackColor = typeOfConnect switch
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
			var hashName = Encoding.ASCII.GetString(fileDetails.FileHash);
			this.Name = hashName;

			LblName.Text = fileDetails.Name;
			LblSize.Text = Math.Round((decimal)fileDetails.Size / (1024 * 1024), 2, MidpointRounding.AwayFromZero).ToString();
			LblType.Text = fileDetails.Type;

			LblName.Visible = true;
			LblSize.Visible = true;
			LblType.Visible = true;

			LblName.Visible = false;
			SetBackground(typeOfConnect);
		}

		public void ChangeProcess(ProgressReport progress)
		{
			this.LblSize.Text = $"{progress.Complete} / {progress.Total}";
			this.ProgresPanel.Width = (int)progress.Percentage / 100 * Width;
		}
	}
}
