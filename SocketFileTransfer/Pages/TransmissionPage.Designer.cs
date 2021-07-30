
namespace SocketFileTransfer.Pages
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtMessage = new System.Windows.Forms.TextBox();
            this.BtnOprate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelContainer
            // 
            this.PanelContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.PanelContainer.Location = new System.Drawing.Point(0, 0);
            this.PanelContainer.Name = "PanelContainer";
            this.PanelContainer.Size = new System.Drawing.Size(384, 542);
            this.PanelContainer.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnOprate);
            this.panel1.Controls.Add(this.TxtMessage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 542);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 31);
            this.panel1.TabIndex = 1;
            // 
            // TxtMessage
            // 
            this.TxtMessage.Dock = System.Windows.Forms.DockStyle.Left;
            this.TxtMessage.Location = new System.Drawing.Point(0, 0);
            this.TxtMessage.Multiline = true;
            this.TxtMessage.Name = "TxtMessage";
            this.TxtMessage.Size = new System.Drawing.Size(332, 31);
            this.TxtMessage.TabIndex = 0;
            this.TxtMessage.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // BtnOprate
            // 
            this.BtnOprate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnOprate.Font = new System.Drawing.Font("Yu Gothic", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BtnOprate.Location = new System.Drawing.Point(332, 0);
            this.BtnOprate.Margin = new System.Windows.Forms.Padding(0);
            this.BtnOprate.Name = "BtnOprate";
            this.BtnOprate.Size = new System.Drawing.Size(52, 31);
            this.BtnOprate.TabIndex = 1;
            this.BtnOprate.Text = "+";
            this.BtnOprate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnOprate.UseVisualStyleBackColor = true;
            this.BtnOprate.Click += new System.EventHandler(this.button1_Click);
            // 
            // TransmissionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 573);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PanelContainer);
            this.Name = "TransmissionPage";
            this.Text = "TransmissionPage";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel PanelContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TxtMessage;
        private System.Windows.Forms.Button BtnOprate;
    }
}