using SocketFileTransfer.Extensions;

namespace SocketFileTransfer.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private void NavigateSeting(object sender, EventArgs e)
    {
		var page = StaticDiProvider.Services.GetRequiredService<SettingPage>();
        Application.Current.MainPage = page;
    }
}