
namespace SocketFileTransfer.Canvas
{
	partial class ReceivedForm
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
			if(disposing && _clients.Count>0)
			{
				foreach (var stream in _clients)
					stream.Value.Socket.Dispose();

				_clients.Clear();
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
			LblMsg = new System.Windows.Forms.Label();
			BtnBack = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// LblMsg
			// 
			LblMsg.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			LblMsg.ForeColor = System.Drawing.Color.Snow;
			LblMsg.Location = new System.Drawing.Point(12, 222);
			LblMsg.Name = "LblMsg";
			LblMsg.Size = new System.Drawing.Size(360, 32);
			LblMsg.TabIndex = 0;
			LblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BtnBack
			// 
			BtnBack.BackColor = System.Drawing.Color.FromArgb(49, 57, 66);
			BtnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			BtnBack.Dock = System.Windows.Forms.DockStyle.Top;
			BtnBack.FlatAppearance.BorderSize = 0;
			BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnBack.ForeColor = System.Drawing.SystemColors.Control;
			BtnBack.Location = new System.Drawing.Point(0, 0);
			BtnBack.Name = "BtnBack";
			BtnBack.Size = new System.Drawing.Size(384, 37);
			BtnBack.TabIndex = 1;
			BtnBack.Text = "Back";
			BtnBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			BtnBack.UseVisualStyleBackColor = false;
			BtnBack.Click += BtnBack_Click;
			// 
			// ReceivedForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.FromArgb(49, 57, 66);
			ClientSize = new System.Drawing.Size(384, 573);
			Controls.Add(BtnBack);
			Controls.Add(LblMsg);
			Name = "ReceivedForm";
			Text = "ReceivedForm";
			Load += ReceivedForm_Load;
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Label LblMsg;
		private System.Windows.Forms.Button BtnBack;
	}
}