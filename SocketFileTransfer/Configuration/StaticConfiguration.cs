using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Configuration;
internal static class StaticConfiguration
{
	public static string _storedLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
}
