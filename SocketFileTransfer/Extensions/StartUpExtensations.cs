using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Extensions;

internal static class StartUpExtensations
{
    public static void AddPages(this MauiAppBuilder builder, Type type)
    {
        var pages = type
            .Assembly.GetTypes()
            .Where(a => a.GetInterface("IView") != null && a.IsClass == true);

        foreach (var page in pages)
            builder.Services.AddTransient(page);
    }
}