using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Helpers;
public interface ISettingHelper
{
    string GetSettingPath();
    string GetStoragePath();
}
