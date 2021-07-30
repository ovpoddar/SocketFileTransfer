
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
            this.LblType = new System.Windows.Forms.Label();
            this.LblName = new System.Windows.Forms.Label();
            this.LblSize = new System.Windows.Forms.Label();
            this.LblText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblType
            // 
            this.LblType.Dock = System.Windows.Forms.DockStyle.Left;
            this.LblType.Font = new System.Drawing.Font("Nirmala UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblType.Location = new System.Drawing.Point(0, 0);
            this.LblType.Name = "LblType";
            this.LblType.Size = new System.Drawing.Size(90, 103);
            this.LblType.TabIndex = 0;
            this.LblType.Text = "label1";
            this.LblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblName
            // 
            this.LblName.Dock = System.Windows.Forms.DockStyle.Top;
            this.LblName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblName.Location = new System.Drawing.Point(90, 0);
            this.LblName.Name = "LblName";
            this.LblName.Size = new System.Drawing.Size(294, 48);
            this.LblName.TabIndex = 1;
            this.LblName.Text = "label1";
            this.LblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblSize
            // 
            this.LblSize.AutoSize = true;
            this.LblSize.Location = new System.Drawing.Point(96, 59);
            this.LblSize.Name = "LblSize";
            this.LblSize.Size = new System.Drawing.Size(38, 15);
            this.LblSize.TabIndex = 2;
            this.LblSize.Text = "label1";
            // 
            // LblText
            // 
            this.LblText.Location = new System.Drawing.Point(0, 0);
            this.LblText.Name = "LblText";
            this.LblText.Size = new System.Drawing.Size(381, 103);
            this.LblText.TabIndex = 3;
            this.LblText.Text = "label1";
            // 
            // CPFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LblSize);
            this.Controls.Add(this.LblName);
            this.Controls.Add(this.LblType);
            this.Controls.Add(this.LblText);
            this.Name = "CPFile";
            this.Size = new System.Drawing.Size(384, 103);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblType;
        private System.Windows.Forms.Label LblName;
        private System.Windows.Forms.Label LblSize;
        private System.Windows.Forms.Label LblText;
    }
}
