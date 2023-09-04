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
}
