using System.Drawing;
using System.Windows.Forms;
using SocketFileTransfer.Models;

namespace SocketFileTransfer.CustomControl
{
    public partial class CpFile : UserControl
    {
        public CpFile()
        {
            InitializeComponent();
        }
        public CpFile(string name, TypeOfConnect typeOfConnect)
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

        public CpFile(string filename, string fileSize, string fileType, TypeOfConnect typeOfConnect)
        {
            InitializeComponent();

            LblName.Text = filename;
            LblSize.Text = fileSize;
            LblType.Text = fileType;

            LblName.Visible = true;
            LblSize.Visible = true;
            LblType.Visible = true;

            LblName.Visible = false;

            SetBackground(typeOfConnect);
        }
    }
}
