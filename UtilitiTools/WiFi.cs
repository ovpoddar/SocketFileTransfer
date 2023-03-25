using ManagedNativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UtilitiTools.Helpers;

namespace UtilitiTools;
public sealed class WiFi
{
	private static WiFi _instance = null;
	private readonly InterfaceInfo? _interface;

	private const string _hotspotTemplate = "Models/HotspotConfiguration.xml";

	private static readonly object _lockObj = new object();
	public static WiFi Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lockObj)
				{
					if (_instance == null)
					{
						_instance = new WiFi();
					}
				}
			}
			return _instance;
		}
	}

	WiFi()
	{
		_interface = NativeWifi.EnumerateInterfaces()
			.First(x =>
			{
				var radioSet = NativeWifi.GetInterfaceRadio(x.Id)?.RadioSets.FirstOrDefault();
				if (radioSet is null)
					return false;

				if (!radioSet.HardwareOn.GetValueOrDefault()) // Hardware radio state is off.
					return false;

				return radioSet.SoftwareOn.HasValue ? radioSet.SoftwareOn.Value : false; // Software radio state is off.
			});

	}

	public void Start()
	{
		NativeWifi.TurnOnInterfaceRadio(_interface.Id);
	}


	public void Stop()
	{
		NativeWifi.TurnOffInterfaceRadio(_interface.Id);
	}

	public List<AvailableNetworkPack> Scan() =>
		NativeWifi.EnumerateAvailableNetworks()
			.Where(a => a.Interface.Id == _interface.Id)
			.GroupBy(a => new { a = a.Ssid.ToString(), b = a.Interface.Id })
			.Select(a => a.First())
			.ToList();

	public async Task<bool> Connect(AvailableNetworkPack networkPack, string password = null)
	{
		NativeWifiCustome.SetProfile(_interface.Id,
			ProfileType.AllUser,
			CreateProfile(networkPack.Ssid.ToString(), password),
			"WPA2PSK AES",
			true);

		return await NativeWifi.ConnectNetworkAsync(
			_interface.Id,
			networkPack.Ssid.ToString(),
			BssType.None,
			TimeSpan.FromSeconds(10));
	}

	string CreateProfile(string profileName, string password)
	{
		var template = File.ReadAllText(_hotspotTemplate);
		var profileNameInByte = Encoding.Default.GetBytes(profileName);
		var profileNameInHEX = BitConverter.ToString(profileNameInByte).Replace("-", "");

		password ??= GeneratePassword();

		return string.Format(template, profileName, profileNameInHEX, password);
	}

	string GeneratePassword()
	{
		Random access = new Random(DateTime.UtcNow.Day);
		var c = access.Next(00000000, 99999999);
		return c.ToString();
	}
}
