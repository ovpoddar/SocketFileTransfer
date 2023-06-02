using System;

namespace SocketFileTransfer.Configuration;
internal static class StaticConfiguration
{
	public static string StoredLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	public static byte[] NegotiationMessage = new byte[2] { 1, 1 };
}
