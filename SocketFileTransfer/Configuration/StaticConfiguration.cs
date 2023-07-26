using System;

namespace SocketFileTransfer.Configuration;
internal static class StaticConfiguration
{
	public static string StoredLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	public static int ApplicationRequiredPort = 1400;
}
