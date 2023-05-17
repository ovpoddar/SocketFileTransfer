using SocketFileTransfer.Model;
using System.Drawing;
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
				_ => BackColor
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
	}
}
