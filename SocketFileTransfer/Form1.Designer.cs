﻿
namespace SocketFileTransfer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cButton1 = new SocketFileTransfer.CustomControl.CButton();
            this.SuspendLayout();
            // 
            // cButton1
            // 
            this.cButton1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.cButton1.Location = new System.Drawing.Point(35, 187);
            this.cButton1.Name = "cButton1";
            this.cButton1.Size = new System.Drawing.Size(98, 98);
            this.cButton1.TabIndex = 0;
            this.cButton1.Text = "cButton1";
            this.cButton1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(57)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(339, 513);
            this.Controls.Add(this.cButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControl.CButton cButton1;
    }
}

