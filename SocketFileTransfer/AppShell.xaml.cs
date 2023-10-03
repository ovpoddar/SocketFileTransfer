using SocketFileTransfer.Pages;

namespace SocketFileTransfer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
    }
}
