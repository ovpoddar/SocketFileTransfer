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
    private string SettingFile { get => "Setting.json"; }

    public void Initialized()
    {
        throw new NotImplementedException();
    }

    public async Task<SettingViewModel> Load()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(SettingFile);
        using var reader = new StreamReader(stream);

        var contents = reader.ReadToEnd();
        return JsonSerializer.Deserialize<SettingViewModel>(contents);
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void UpdateSetting(string proprityName, object value)
    {
        throw new NotImplementedException();
    }
}
