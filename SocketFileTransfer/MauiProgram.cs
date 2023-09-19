// Ignore Spelling: App

using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SocketFileTransfer.Extensions;
using SocketFileTransfer.Helpers;
using SocketFileTransfer.Models;
using SocketFileTransfer.Pages;
using SocketFileTransfer.Services;
using SocketFileTransfer.ViewModels;
using System.Runtime.CompilerServices;

namespace SocketFileTransfer;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ISettingService, SettingService>();
        builder.Services.AddSingleton<ISettingHelper, SettingHelper>();
        builder.Services.AddSingleton<SettingModel>();

        builder.AddPages(typeof(MauiProgram));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        var setting = app.Services.GetRequiredService<ISettingService>();
        var result = setting.Initialized();
        if (result)
            return app;
        else
        {
            setting.Reset();
            result = setting.Initialized();
            if(result) return app;
            throw new Exception("Fail to initialize setting.");
        }
    }
}
