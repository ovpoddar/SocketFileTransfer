
namespace SocketFileTransfer.CustomControl
{
	partial class CPFile
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			LblType = new System.Windows.Forms.Label();
			LblName = new System.Windows.Forms.Label();
			LblSize = new System.Windows.Forms.Label();
			LblText = new System.Windows.Forms.Label();
			ProgresPanel = new System.Windows.Forms.Panel();
			SuspendLayout();
			// 
			// LblType
			// 
			LblType.Dock = System.Windows.Forms.DockStyle.Left;
			LblType.Font = new System.Drawing.Font("Nirmala UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			LblType.Location = new System.Drawing.Point(0, 0);
			LblType.Name = "LblType";
			LblType.Size = new System.Drawing.Size(90, 103);
			LblType.TabIndex = 0;
			LblType.Text = "label1";
			LblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LblName
			// 
			LblName.Dock = System.Windows.Forms.DockStyle.Top;
			LblName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			LblName.Location = new System.Drawing.Point(90, 0);
			LblName.Name = "LblName";
			LblName.Size = new System.Drawing.Size(294, 48);
			LblName.TabIndex = 1;
			LblName.Text = "label1";
			LblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblSize
			// 
			LblSize.AutoSize = true;
			LblSize.Location = new System.Drawing.Point(90, 48);
			LblSize.Name = "LblSize";
			LblSize.Size = new System.Drawing.Size(38, 15);
			LblSize.TabIndex = 2;
			LblSize.Text = "label1";
			// 
			// LblText
			// 
			LblText.Location = new System.Drawing.Point(0, 0);
			LblText.Name = "LblText";
			LblText.Size = new System.Drawing.Size(381, 103);
			LblText.TabIndex = 3;
			LblText.Text = "label1";
			// 
			// ProgresPanel
			// 
			ProgresPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
			ProgresPanel.Location = new System.Drawing.Point(0, 0);
			ProgresPanel.Name = "ProgresPanel";
			ProgresPanel.Size = new System.Drawing.Size(0, 103);
			ProgresPanel.TabIndex = 4;
			// 
			// CPFile
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(ProgresPanel);
			Controls.Add(LblSize);
			Controls.Add(LblName);
			Controls.Add(LblType);
			Controls.Add(LblText);
			Name = "CPFile";
			Size = new System.Drawing.Size(384, 103);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label LblType;
		private System.Windows.Forms.Label LblName;
		private System.Windows.Forms.Label LblSize;
		private System.Windows.Forms.Label LblText;
		private System.Windows.Forms.Panel ProgresPanel;
	}
}
