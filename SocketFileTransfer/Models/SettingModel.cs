using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using SocketFileTransfer.Services;
using SocketFileTransfer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace SocketFileTransfer.Models;
public class SettingModel : SettingViewModel
{
    private readonly ISettingService _settingService;

    public SettingModel(ISettingService settingService) : base(settingService.Setting)
    {
        _settingService = settingService;
    }

    public override async void OpenDialogAsync()
    {
#if ANDROID
        var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Current activity is null");

        if (AndroidX.Core.Content.ContextCompat.CheckSelfPermission(activity, Android.Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
        {
            if (AndroidX.Core.App.ActivityCompat.ShouldShowRequestPermissionRationale(activity, Android.Manifest.Permission.ReadExternalStorage))
                Toast.Make("you did not provide the permission to access your files.", ToastDuration.Short);

            AndroidX.Core.App.ActivityCompat.RequestPermissions(activity, new[] { Android.Manifest.Permission.ReadExternalStorage }, 1);
        }
#endif
        var result = await FolderPicker.PickAsync(base.SavePath, default);
        result.EnsureSuccess();
        base.SavePath = result.Folder.Path;
    }

    
}
