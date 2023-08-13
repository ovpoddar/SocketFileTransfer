﻿using SocketFileTransfer.Attributes;
using SocketFileTransfer.CustomControl;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Handler;
using SocketFileTransfer.Model;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas;
public partial class SettingPage : Form
{
    private bool _isValidSetting;
    public event EventHandler<Connection> OnTransmissionIPFound;

    public SettingPage()
    {
        _isValidSetting = true;
        InitializeComponent();

        var fieldToInitilized = typeof(ConfigurationSetting).GetProperties()
            .Where(a => a.CustomAttributes.Any(a => a.AttributeType == typeof(SettingOptionAttribute)));
        foreach (var field in fieldToInitilized)
        {
            var displayLabel = field.GetCustomAttributes(true).OfType<SettingOptionAttribute>().FirstOrDefault().DisplayText;
            var proprityName = string.IsNullOrEmpty(displayLabel) ? field.Name : displayLabel;
            var value = field.GetValue(null);
            var label = new Label { Text = proprityName, AutoSize = true, Margin = new Padding(0, 20, 0, 0) };
            flowLayoutPanel1.Controls.Add(label);
            
            if (field.Name == nameof(ConfigurationSetting.SavePath))
            {
                var textbox = new TextBoxWithButton { Text = value.ToString(), Width = flowLayoutPanel1.Width };
                textbox.textBox1.Name = field.Name;
                textbox.textBox1.LostFocus += Textbox_LostFocus;
                textbox.textBox1.TextChanged += TextBox_TextChange;
                flowLayoutPanel1.Controls.Add(textbox);
            }
            else
            {
                var textbox = new TextBox { Text = value.ToString(), Width = flowLayoutPanel1.Width, Name = field.Name };
                textbox.TextChanged += TextBox_TextChange;
                textbox.LostFocus += Textbox_LostFocus;
                flowLayoutPanel1.Controls.Add(textbox);
            }
        }
    }

    private void TextBox_TextChange(object sender, EventArgs e)
    {
        try
        {
            var textbox = sender as TextBox;
            var field = typeof(ConfigurationSetting).GetProperties()
                .FirstOrDefault(a => a.Name == textbox.Name);
            var value = Convert.ChangeType(textbox.Text, field.GetCustomAttributes(true).OfType<SettingOptionAttribute>().FirstOrDefault().Type);
            field.SetValue(null, value);
        }
        catch
        { }
    }

    private void Textbox_LostFocus(object sender, EventArgs e)
    {
        var textbox = sender as TextBox;
        if (textbox.Name == nameof(ConfigurationSetting.ApplicationRequiredPort))
        {
            _isValidSetting = int.TryParse(textbox.Text, out _);
        }

        if (textbox.Name == nameof(ConfigurationSetting.SavePath))
        {
            _isValidSetting = Directory.Exists(textbox.Text);
        }
        if (!_isValidSetting)
        {
            textbox.ForeColor = Color.Red;
            textbox.Focus();
        }
        else
        {
            textbox.ForeColor = default(Color);
        }
    }


    private void BtnBack_Click(object sender, EventArgs e)
    {
        if (!_isValidSetting)
            MessageBox.Show("Invalidate configuration found");
        else
            OnTransmissionIPFound.Raise(this, new Connection(TypeOfConnect.None));
    }

    ~SettingPage()
    {
        if (_isValidSetting)
            ConfigurationSetting.Reset();
    }
}
