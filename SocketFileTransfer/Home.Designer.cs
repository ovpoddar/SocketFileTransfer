
namespace SocketFileTransfer
{
	partial class Home
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
			PanelHeader = new System.Windows.Forms.Panel();
			btnMinimize = new System.Windows.Forms.Button();
			BtnExit = new System.Windows.Forms.Button();
			panelBody = new System.Windows.Forms.Panel();
			LblMessage = new System.Windows.Forms.Label();
			PanelHeader.SuspendLayout();
			panelBody.SuspendLayout();
			SuspendLayout();
			// 
			// PanelHeader
			// 
			PanelHeader.BackColor = System.Drawing.Color.FromArgb(59, 72, 81);
			PanelHeader.Controls.Add(btnMinimize);
			PanelHeader.Controls.Add(BtnExit);
			PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
			PanelHeader.Location = new System.Drawing.Point(0, 0);
			PanelHeader.Name = "PanelHeader";
			PanelHeader.Size = new System.Drawing.Size(400, 38);
			PanelHeader.TabIndex = 0;
			PanelHeader.MouseDown += PanelHeader_MouseDown;
			// 
			// btnMinimize
			// 
			btnMinimize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnMinimize.FlatAppearance.BorderSize = 0;
			btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnMinimize.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			btnMinimize.ForeColor = System.Drawing.Color.FromArgb(238, 238, 238);
			btnMinimize.Location = new System.Drawing.Point(296, 0);
			btnMinimize.Name = "btnMinimize";
			btnMinimize.Size = new System.Drawing.Size(50, 38);
			btnMinimize.TabIndex = 0;
			btnMinimize.Text = "_";
			btnMinimize.UseVisualStyleBackColor = true;
			btnMinimize.Click += BtnMinimize_Click;
			// 
			// BtnExit
			// 
			BtnExit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			BtnExit.FlatAppearance.BorderSize = 0;
			BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnExit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			BtnExit.ForeColor = System.Drawing.Color.FromArgb(238, 238, 238);
			BtnExit.Location = new System.Drawing.Point(352, 0);
			BtnExit.Name = "BtnExit";
			BtnExit.Size = new System.Drawing.Size(48, 38);
			BtnExit.TabIndex = 0;
			BtnExit.Text = "X";
			BtnExit.UseVisualStyleBackColor = true;
			BtnExit.Click += BtnExit_Click;
			// 
			// panelBody
			// 
			panelBody.Controls.Add(LblMessage);
			panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
			panelBody.Location = new System.Drawing.Point(0, 38);
			panelBody.Name = "panelBody";
			panelBody.Size = new System.Drawing.Size(400, 612);
			panelBody.TabIndex = 1;
			// 
			// LblMessage
			// 
			LblMessage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			LblMessage.Location = new System.Drawing.Point(64, 222);
			LblMessage.Name = "LblMessage";
			LblMessage.Size = new System.Drawing.Size(269, 58);
			LblMessage.TabIndex = 0;
			LblMessage.Text = "Turn On your Wifi And Restart the application";
			LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Home
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(400, 650);
			ControlBox = false;
			Controls.Add(panelBody);
			Controls.Add(PanelHeader);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			MaximizeBox = false;
			MaximumSize = new System.Drawing.Size(400, 650);
			MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(400, 650);
			Name = "Home";
			SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			PanelHeader.ResumeLayout(false);
			panelBody.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Panel PanelHeader;
		private System.Windows.Forms.Panel panelBody;
		private System.Windows.Forms.Button BtnExit;
		private System.Windows.Forms.Button btnMinimize;
		private System.Windows.Forms.Label LblMessage;
	}
}