using SocketFileTransfer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Services;
public interface ISettingService
{
    SettingViewModel Setting { get; set; }
    void Initialized();
    void Reset();
    void UpdateSetting(string proprityName, object value);
}
