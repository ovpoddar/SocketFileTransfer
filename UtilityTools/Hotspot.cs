using System.Net;
using System.Net.NetworkInformation;

namespace UtilityTools;
public sealed class Hotspot
{
	// todo: need instance
	private bool _isRunning;

	public bool IsRunning
	{
		get { return _isRunning; }
		private set
		{
			_isRunning = NetworkInterface.GetAllNetworkInterfaces()
				.Any(a => a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
						  && a.OperationalStatus == OperationalStatus.Up
						  && a.Name.ToUpper().Contains("Local Area Connection".ToUpper()));
		}
	}

	public static void Start() =>
		CommendPrompt.ExecuteCommand(new string[]
		{
			"netsh wlan set hostednetwork mode=allow ssid=" + Dns.GetHostName(),
			"netsh wlan start hosted network"
		});

	public static void Stop() =>
		CommendPrompt.ExecuteCommand(new string[]
		{
			"netsh wlan stop hostednetwork"
		});
}