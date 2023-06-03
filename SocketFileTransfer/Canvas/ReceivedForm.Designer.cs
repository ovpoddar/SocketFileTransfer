
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
			if(disposing && _clients.Count > 0)
			{
				for (var i = 0; i < _currentAdded; i++)
					_clients[i].Dispose();
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
			this.LblMsg = new System.Windows.Forms.Label();
			this.BtnBack = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// LblMsg
			// 
			this.LblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.LblMsg.ForeColor = System.Drawing.Color.Snow;
			this.LblMsg.Location = new System.Drawing.Point(12, 222);
			this.LblMsg.Name = "LblMsg";
			this.LblMsg.Size = new System.Drawing.Size(360, 32);
			this.LblMsg.TabIndex = 0;
			this.LblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BtnBack
			// 
			this.BtnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(57)))), ((int)(((byte)(66)))));
			this.BtnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.BtnBack.Dock = System.Windows.Forms.DockStyle.Top;
			this.BtnBack.FlatAppearance.BorderSize = 0;
			this.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnBack.ForeColor = System.Drawing.SystemColors.Control;
			this.BtnBack.Location = new System.Drawing.Point(0, 0);
			this.BtnBack.Name = "BtnBack";
			this.BtnBack.Size = new System.Drawing.Size(384, 37);
			this.BtnBack.TabIndex = 1;
			this.BtnBack.Text = "Back";
			this.BtnBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.BtnBack.UseVisualStyleBackColor = false;
			this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
			// 
			// ReceivedForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(57)))), ((int)(((byte)(66)))));
			this.ClientSize = new System.Drawing.Size(384, 573);
			this.Controls.Add(this.BtnBack);
			this.Controls.Add(this.LblMsg);
			this.Name = "ReceivedForm";
			this.Text = "ReceivedForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label LblMsg;
		private System.Windows.Forms.Button BtnBack;
	}
}