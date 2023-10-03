using SocketFileTransfer.Models;
using SocketFileTransfer.Services;
using SocketFileTransfer.ViewModels;

namespace SocketFileTransfer.Pages;

public partial class SettingPage : ContentPage
{
    private readonly SettingModel _model;
    private readonly ISettingService _settingService;
    public SettingPage(SettingModel model, ISettingService settingService)
    {
        InitializeComponent();
        BindingContext = model;
        _model = model;
        _settingService = settingService;
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        _settingService.UpdateSetting(_model);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}