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
		BtnBack = new System.Windows.Forms.Button();
		SuspendLayout();
		// 
		// flowLayoutPanel1
		// 
		flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		flowLayoutPanel1.AutoScroll = true;
		flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(59, 72, 81);
		flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		flowLayoutPanel1.Location = new System.Drawing.Point(0, 36);
		flowLayoutPanel1.Name = "flowLayoutPanel1";
		flowLayoutPanel1.Size = new System.Drawing.Size(386, 545);
		flowLayoutPanel1.TabIndex = 0;
		flowLayoutPanel1.WrapContents = false;
		// 
		// BtnBack
		// 
		BtnBack.BackColor = System.Drawing.Color.FromArgb(49, 57, 66);
		BtnBack.Dock = System.Windows.Forms.DockStyle.Top;
		BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		BtnBack.Location = new System.Drawing.Point(0, 0);
		BtnBack.Name = "BtnBack";
		BtnBack.Size = new System.Drawing.Size(386, 38);
		BtnBack.TabIndex = 1;
		BtnBack.Text = "Back";
		BtnBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		BtnBack.UseVisualStyleBackColor = false;
		BtnBack.Click += BtnBack_Click;
		// 
		// SettingPage
		// 
		AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		ClientSize = new System.Drawing.Size(386, 581);
		Controls.Add(BtnBack);
		Controls.Add(flowLayoutPanel1);
		Name = "SettingPage";
		Text = "SettingPage";
		ResumeLayout(false);
	}

	#endregion

	private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
	private System.Windows.Forms.Button BtnBack;
}