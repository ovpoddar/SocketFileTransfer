namespace SocketFileTransfer.CustomControl;

partial class TextBoxWithButton
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
        textBox1 = new System.Windows.Forms.TextBox();
        button1 = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        textBox1.Location = new System.Drawing.Point(0, 0);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.Size = new System.Drawing.Size(231, 21);
        textBox1.TabIndex = 0;
        // 
        // button1
        // 
        button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        button1.BackColor = System.Drawing.Color.White;
        button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        button1.Location = new System.Drawing.Point(231, 0);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(34, 21);
        button1.TabIndex = 1;
        button1.Text = "...";
        button1.UseVisualStyleBackColor = false;
        button1.Click += button1_Click;
        // 
        // TextBoxWithButton
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(button1);
        Controls.Add(textBox1);
        Name = "TextBoxWithButton";
        Size = new System.Drawing.Size(265, 21);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button button1;
}
