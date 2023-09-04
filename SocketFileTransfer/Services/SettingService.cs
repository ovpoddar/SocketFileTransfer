using SocketFileTransfer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocketFileTransfer.Services;
public class SettingService : ISettingService
{
    private static string SettingFile { get => "Setting.json"; }

    public SettingViewModel Setting { get; set; } = new SettingViewModel();

    public SettingService()
    {
        Load();
    }

    public void Initialized()
    {
        throw new NotImplementedException();
    }

    private void Load()
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(SettingService.SettingFile);
        using var reader = new StreamReader(stream.Result);
        var contents = reader.ReadToEnd();

        Setting = JsonSerializer.Deserialize<SettingViewModel>(contents);
    }

    public void Reset()
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(SettingService.SettingFile);
    }

    public void UpdateSetting(string propertyName, object value)
    {
        throw new NotImplementedException();
    }
}
