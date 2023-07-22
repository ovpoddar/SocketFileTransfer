
namespace SocketFileTransfer.Canvas
{
	partial class SendForm
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
			if (disposing && _clients.Count > 0)
			{
				foreach (var stream in _clients)
					stream.Value.Item1.Dispose();

				_clients.Clear();
			}
			if (disposing && !_cancellationTokenSource.IsCancellationRequested)
			{
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource.Dispose();
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
			BtnBack = new System.Windows.Forms.Button();
			listBox1 = new System.Windows.Forms.ListBox();
			TaskButton = new System.Windows.Forms.Button();
			SuspendLayout();
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
			BtnBack.Size = new System.Drawing.Size(386, 37);
			BtnBack.TabIndex = 0;
			BtnBack.Text = "Back";
			BtnBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			BtnBack.UseVisualStyleBackColor = false;
			BtnBack.Click += BtnBack_Click;
			// 
			// listBox1
			// 
			listBox1.BackColor = System.Drawing.Color.FromArgb(59, 72, 81);
			listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			listBox1.FormattingEnabled = true;
			listBox1.ItemHeight = 15;
			listBox1.Location = new System.Drawing.Point(0, 37);
			listBox1.Name = "listBox1";
			listBox1.Size = new System.Drawing.Size(386, 544);
			listBox1.TabIndex = 1;
			listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChangedAsync;
			// 
			// TaskButton
			// 
			TaskButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			TaskButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			TaskButton.Location = new System.Drawing.Point(0, 551);
			TaskButton.Name = "TaskButton";
			TaskButton.Size = new System.Drawing.Size(386, 30);
			TaskButton.TabIndex = 2;
			TaskButton.Text = "Cancel";
			TaskButton.UseVisualStyleBackColor = true;
			TaskButton.Click += TaskButton_Click;
			// 
			// SendForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.FromArgb(49, 57, 66);
			ClientSize = new System.Drawing.Size(386, 581);
			Controls.Add(TaskButton);
			Controls.Add(listBox1);
			Controls.Add(BtnBack);
			Name = "SendForm";
			Text = "SendForm";
			Load += StartScanForm_Load;
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button BtnBack;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button TaskButton;
	}
}