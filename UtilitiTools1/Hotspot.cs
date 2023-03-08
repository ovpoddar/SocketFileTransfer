using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

namespace UtilitiTools;
public sealed class Hotspot
{
	private bool _isRunning;

	public bool IsRunning
	{
		get { return _isRunning; }
		private set {
			_isRunning = NetworkInterface.GetAllNetworkInterfaces()
				.Any(a => a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
						  && a.OperationalStatus == OperationalStatus.Up
						  && a.Name.ToUpper().Contains("Local Area Connection".ToUpper()));
		}
	}

	public static void Start()
	{
		var process = CreateCmdProcess();
		process.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=" + Dns.GetHostName());
		process.StandardInput.WriteLine("netsh wlan start hosted network");
		process.StandardInput.Close();
	}
	public static void Stop()
	{
		var process = CreateCmdProcess();
		process.StandardInput.WriteLine("netsh wlan stop hostednetwork");
		process.StandardInput.Close();
	}


	static Process CreateCmdProcess()
	{
		var processStartInfo = new ProcessStartInfo("cmd.exe")
		{
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
			CreateNoWindow = true,
			UseShellExecute = false
		};
		return Process.Start(processStartInfo);
	}
}
