using SocketFileTransfer.Handler;
using System;
using System.Windows.Forms;

namespace SocketFileTransfer
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!ConfigurationSetting.IsInitialized)
                ConfigurationSetting.Initialized();
            ConfigurationSetting.Load();
            Application.Run(new Home());
        }
    }
}
