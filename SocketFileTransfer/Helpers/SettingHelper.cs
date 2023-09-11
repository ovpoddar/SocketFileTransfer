
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Helpers;
public class SettingHelper : ISettingHelper
{
    private static string FileName => "Setting.Json";
    public string GetSettingPath() => 
        Path.Combine(FileSystem.Current.AppDataDirectory, FileName);

    public string GetStoragePath()
    {
#if WINDOWS
        GetDownloadFolder(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0, IntPtr.Zero, out var path);
        return path;
#elif ANDROID
#elif IOS
#elif MACCATALYST
#endif
    }

#if WINDOWS
    [DllImport("shell32.dll", EntryPoint = "SHGetKnownFolderPath", CharSet = CharSet.Unicode)]
    static extern void GetDownloadFolder([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string ppszPath);
#endif
}
