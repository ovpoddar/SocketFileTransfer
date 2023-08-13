using System;
using System.Windows.Forms;

namespace SocketFileTransfer.CustomControl;
public partial class TextBoxWithButton : UserControl
{
    public override string Text { get => this.textBox1.Text; set => this.textBox1.Text = value; }

    public TextBoxWithButton()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var folderDlg = new FolderBrowserDialog
        {
            ShowNewFolderButton = true
        };
        var result = folderDlg.ShowDialog();
        if (result == DialogResult.OK)
            this.textBox1.Text = folderDlg.SelectedPath;
    }
}
