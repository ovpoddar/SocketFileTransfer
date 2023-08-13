
namespace SocketFileTransfer.Canvas
{
	partial class Index
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
			Send = new CustomControl.CButton();
			BtnReceived = new CustomControl.CButton();
			Settings = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// Send
			// 
			Send.BackColor = System.Drawing.Color.FromArgb(96, 97, 97);
			Send.ForeColor = System.Drawing.Color.FromArgb(236, 236, 236);
			Send.Location = new System.Drawing.Point(36, 209);
			Send.Name = "Send";
			Send.Size = new System.Drawing.Size(144, 144);
			Send.TabIndex = 0;
			Send.Text = "Send";
			Send.UseVisualStyleBackColor = false;
			Send.Click += Send_Click;
			Send.MouseEnter += OnMouseEnterButton;
			Send.MouseLeave += OnMouseLeaveButton;
			// 
			// BtnReceived
			// 
			BtnReceived.BackColor = System.Drawing.Color.FromArgb(96, 97, 97);
			BtnReceived.ForeColor = System.Drawing.Color.FromArgb(236, 236, 236);
			BtnReceived.Location = new System.Drawing.Point(211, 212);
			BtnReceived.Name = "BtnReceived";
			BtnReceived.Size = new System.Drawing.Size(142, 142);
			BtnReceived.TabIndex = 0;
			BtnReceived.Text = "Received";
			BtnReceived.UseVisualStyleBackColor = false;
			BtnReceived.Click += BtnReceived_Click;
			BtnReceived.MouseEnter += OnMouseEnterButton;
			BtnReceived.MouseLeave += OnMouseLeaveButton;
			// 
			// Settings
			// 
			Settings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			Settings.AutoEllipsis = true;
			Settings.BackColor = System.Drawing.Color.FromArgb(49, 57, 66);
			Settings.BackgroundImage = Properties.Resources.settings;
			Settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			Settings.FlatAppearance.BorderSize = 0;
			Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			Settings.Location = new System.Drawing.Point(324, 12);
			Settings.Name = "Settings";
			Settings.Size = new System.Drawing.Size(48, 48);
			Settings.TabIndex = 1;
			Settings.UseMnemonic = false;
			Settings.UseVisualStyleBackColor = false;
			Settings.Click += Settings_Click;
			// 
			// Index
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.FromArgb(49, 57, 66);
			ClientSize = new System.Drawing.Size(384, 573);
			Controls.Add(Settings);
			Controls.Add(BtnReceived);
			Controls.Add(Send);
			Name = "Index";
			Text = "Home";
			ResumeLayout(false);
		}

		#endregion

		private CustomControl.CButton Send;
		private CustomControl.CButton BtnReceived;
		private System.Windows.Forms.Button Settings;
	}
}