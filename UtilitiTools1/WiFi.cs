using ManagedNativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiTools;
public class WiFi
{
	public void Start()
	{
		var ablavleRadioInterfaces = NativeWifi.EnumerateInterfaces();
        foreach (var radioInterface in ablavleRadioInterfaces)
        {
			NativeWifi.TurnOnInterfaceRadio(radioInterface.Id);
		}
    }


	public void Stop()
	{
		var ablavleRadioInterfaces = NativeWifi.EnumerateInterfaces();
		foreach (var radioInterface in ablavleRadioInterfaces)
		{
			NativeWifi.TurnOffInterfaceRadio(radioInterface.Id);
		}
	}

	public List<AvailableNetworkPack> Scan()
	{
		
		return NativeWifi.EnumerateAvailableNetworks().ToList();
	}

	public async Task<bool> Connect(AvailableNetworkPack? networkPack)
	{
		NativeWifi.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true)

		return await NativeWifi.ConnectNetworkAsync(
			networkPack.Interface.Id,
			networkPack.ProfileName,
			networkPack.BssType,
			TimeSpan.FromSeconds(10));
	}
}
