using Microsoft.Extensions.Logging;
using SocketFileTransfer.Extensions;
using SocketFileTransfer.Models;
using SocketFileTransfer.Pages;
using SocketFileTransfer.Services;
using SocketFileTransfer.ViewModels;

namespace SocketFileTransfer;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ISettingService, SettingService>();
        builder.Services.AddSingleton<SettingModel>();

        builder.AddPages(typeof(MauiProgram));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
