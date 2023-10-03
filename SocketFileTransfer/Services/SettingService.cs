using SocketFileTransfer.Helpers;
using SocketFileTransfer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocketFileTransfer.Services;
public class SettingService : ISettingService
{
    private readonly ISettingHelper _settingHelper;

    public SettingViewModel Setting { get; set; } = new SettingViewModel();

    public SettingService(ISettingHelper settingHelper)
    {
        _settingHelper = settingHelper;
    }

    public bool Initialized()
    {
        var storageLocation = _settingHelper.GetSettingPath() ?? throw new Exception("Missing Setting Path Location.");
        var fileinfo = new FileInfo(storageLocation);
        if (!fileinfo.Exists)
        {
            using var writter = fileinfo.CreateText();
            Reset(writter);
            writter.Close();
        }

        try
        {
            using var context = fileinfo.OpenRead();
            Setting = JsonSerializer.Deserialize<SettingViewModel>(context);
            return true;
        }
        catch
        {
            return false;
        }
    }

    void Reset(StreamWriter stream)
    {
        var defaultSetting = new SettingViewModel
        {
            ApplicationRequiredPort = 1400,
            SavePath = _settingHelper.GetStoragePath(),
        };
        stream.Write(JsonSerializer.Serialize(defaultSetting));
        stream.Flush();
    }

    public void Reset()
    {
        var storageLocation = _settingHelper.GetSettingPath();
        using var sr = new StreamWriter(storageLocation);
        Reset(sr);
        sr.Close();
    }

    public void UpdateSetting(SettingViewModel model)
    {
        var storageLocation = _settingHelper.GetSettingPath() ?? throw new Exception("Missing Setting Path Location.");
        using var sr = new StreamWriter(storageLocation);
        var setting = JsonSerializer.Serialize(model);
        sr.Write(setting);
        sr.Flush();
    }
}
