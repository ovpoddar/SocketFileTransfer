using SocketFileTransfer.Models;
using SocketFileTransfer.Services;
using SocketFileTransfer.ViewModels;

namespace SocketFileTransfer.Pages;

public partial class SettingPage : ContentPage
{

    public SettingPage(SettingModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }

}