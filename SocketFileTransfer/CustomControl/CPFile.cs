using SocketFileTransfer.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
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

            SetBackground(typeOfConnect);
        }

        public CPFile(MessageDetails messageDetails, TypeOfConnect typeOfConnect)
        {
            InitializeComponent();

            LblType.Text = "";
            Height = ((messageDetails.Length / 30) == 0
                ? 1
                : (int)(messageDetails.Length / 30D * .5)) * 25;
            // hide all other component and made lalname to dock fill
            LblName.Visible = false;
            LblSize.Visible = false;
            LblType.Visible = true;
            LblName.Size = new Size(Width - 20, 20);
            LblType.Dock = DockStyle.Fill;
            BtnCopy.Visible = true;
            LblType.TextAlign = ContentAlignment.TopLeft;
            SetBackground(typeOfConnect);
        }

        public CPFile(TypeOfConnect typeOfConnect, string message)
        {
            InitializeComponent();

            LblType.Text = message;
            Height = 30;
            // hide all other component and made lalname to dock fill
            LblName.Visible = false;
            LblSize.Visible = false;
            LblType.Visible = true;

            LblType.Dock = DockStyle.Fill;
            SetBackground(typeOfConnect);
        }

        public void ChangeMessage(MessageReport messageReport)
        {
            var encoding = Encoding.GetEncoding(messageReport.EncodingPage);
            var message = encoding.GetString(messageReport.Message);

            if(message.Contains(Environment.NewLine))
            {
                var nelinesCount = message.Split(Environment.NewLine).Length;
                this.Height = this.Height + (int)((30 * nelinesCount) * .5);
            }

            LblType.Text += message;
        }

        public void ChangeProcess(ProgressReport progress)
        {
            this.LblSize.Text = $"{progress.Complete} / {progress.Total}";
            this.ProgresPanel.Width = (int)(progress.Percentage / 100 * Width);
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.LblType.Text);
        }
    }
}
