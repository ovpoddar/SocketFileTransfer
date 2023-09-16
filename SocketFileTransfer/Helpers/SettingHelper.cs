
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
        var path = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).Path;
        return path;
#elif IOS
        return FileSystem.AppDataDirectory;
#elif MACCATALYST
    var libraryPath = FileSystem.AppDataDirectory.AsSpan();
    libraryPath = libraryPath[..libraryPath.LastIndexOf("/")];
    return Path.Combine(libraryPath.ToString(), "Downloads");
#endif
        throw new NotSupportedException();
    }

#if WINDOWS
    [DllImport("shell32.dll", EntryPoint = "SHGetKnownFolderPath", CharSet = CharSet.Unicode)]
    static extern void GetDownloadFolder([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string ppszPath);
#endif
}
