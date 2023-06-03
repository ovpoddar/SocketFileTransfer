
namespace SocketFileTransfer.Canvas
{
	partial class TransmissionPage
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
			if(disposing && _clientSocket.Connected)
			{
				_clientSocket.Close();
				_clientSocket.Dispose();
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
			System.Windows.Forms.Button button1;
			PanelContainer = new System.Windows.Forms.FlowLayoutPanel();
			panel1 = new System.Windows.Forms.Panel();
			BtnOperate = new System.Windows.Forms.Button();
			TxtMessage = new System.Windows.Forms.TextBox();
			button1 = new System.Windows.Forms.Button();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// PanelContainer
			// 
			PanelContainer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			PanelContainer.AutoScroll = true;
			PanelContainer.BackColor = System.Drawing.Color.FromArgb(59, 72, 81);
			PanelContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			PanelContainer.Location = new System.Drawing.Point(0, 37);
			PanelContainer.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			PanelContainer.Name = "PanelContainer";
			PanelContainer.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			PanelContainer.Size = new System.Drawing.Size(384, 502);
			PanelContainer.TabIndex = 0;
			PanelContainer.WrapContents = false;
			// 
			// panel1
			// 
			panel1.Controls.Add(BtnOperate);
			panel1.Controls.Add(TxtMessage);
			panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			panel1.Location = new System.Drawing.Point(0, 542);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(384, 31);
			panel1.TabIndex = 1;
			// 
			// BtnOperate
			// 
			BtnOperate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			BtnOperate.Font = new System.Drawing.Font("Yu Gothic", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			BtnOperate.Location = new System.Drawing.Point(326, 0);
			BtnOperate.Margin = new System.Windows.Forms.Padding(0);
			BtnOperate.Name = "BtnOperate";
			BtnOperate.Size = new System.Drawing.Size(58, 31);
			BtnOperate.TabIndex = 1;
			BtnOperate.Text = "+";
			BtnOperate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			BtnOperate.UseVisualStyleBackColor = true;
			BtnOperate.Click += Button1_Click;
			// 
			// TxtMessage
			// 
			TxtMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			TxtMessage.Location = new System.Drawing.Point(0, 0);
			TxtMessage.Multiline = true;
			TxtMessage.Name = "TxtMessage";
			TxtMessage.Size = new System.Drawing.Size(329, 31);
			TxtMessage.TabIndex = 0;
			TxtMessage.TextChanged += TextBox1_TextChanged;
			// 
			// button1
			// 
			button1.BackColor = System.Drawing.Color.FromArgb(49, 57, 66);
			button1.Dock = System.Windows.Forms.DockStyle.Top;
			button1.FlatAppearance.BorderSize = 0;
			button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			button1.Location = new System.Drawing.Point(0, 0);
			button1.Margin = new System.Windows.Forms.Padding(0);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(384, 37);
			button1.TabIndex = 2;
			button1.Text = "Back";
			button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			button1.UseVisualStyleBackColor = false;
			button1.Click += button1_Click_1;
			// 
			// TransmissionPage
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(384, 573);
			Controls.Add(button1);
			Controls.Add(panel1);
			Controls.Add(PanelContainer);
			Name = "TransmissionPage";
			Text = "TransmissionPage";
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel PanelContainer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox TxtMessage;
		private System.Windows.Forms.Button BtnOperate;
		private System.Windows.Forms.Button button1;
	}
}