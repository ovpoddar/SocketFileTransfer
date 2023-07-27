namespace SocketFileTransfer.Canvas;

partial class SettingPage
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		SuspendLayout();
		// 
		// flowLayoutPanel1
		// 
		flowLayoutPanel1.AutoScroll = true;
		flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		flowLayoutPanel1.Name = "flowLayoutPanel1";
		flowLayoutPanel1.Size = new System.Drawing.Size(386, 581);
		flowLayoutPanel1.TabIndex = 0;
		flowLayoutPanel1.WrapContents = false;
		// 
		// SettingPage
		// 
		AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		ClientSize = new System.Drawing.Size(386, 581);
		Controls.Add(flowLayoutPanel1);
		Name = "SettingPage";
		Text = "SettingPage";
		ResumeLayout(false);
	}

	#endregion

	private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
}