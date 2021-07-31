
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
            this.Send = new SocketFileTransfer.CustomControl.CButton();
            this.BtnReceived = new SocketFileTransfer.CustomControl.CButton();
            this.SuspendLayout();
            // 
            // Send
            // 
            this.Send.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.Send.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.Send.Location = new System.Drawing.Point(36, 209);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(144, 144);
            this.Send.TabIndex = 0;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = false;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            this.Send.MouseEnter += new System.EventHandler(this.OnMouseEnterButton1);
            this.Send.MouseLeave += new System.EventHandler(this.OnMouseLeaveButton1);
            // 
            // BtnReceived
            // 
            this.BtnReceived.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.BtnReceived.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.BtnReceived.Location = new System.Drawing.Point(211, 212);
            this.BtnReceived.Name = "BtnReceived";
            this.BtnReceived.Size = new System.Drawing.Size(142, 142);
            this.BtnReceived.TabIndex = 0;
            this.BtnReceived.Text = "Received";
            this.BtnReceived.UseVisualStyleBackColor = false;
            this.BtnReceived.Click += new System.EventHandler(this.BtnReceived_Click);
            this.BtnReceived.MouseEnter += new System.EventHandler(this.OnMouseEnterButton1);
            this.BtnReceived.MouseLeave += new System.EventHandler(this.OnMouseLeaveButton1);
            // 
            // Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(57)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(384, 573);
            this.Controls.Add(this.BtnReceived);
            this.Controls.Add(this.Send);
            this.Name = "Index";
            this.Text = "Home";
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControl.CButton Send;
        private CustomControl.CButton BtnReceived;
    }
}