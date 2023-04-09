using System.Net;
using System.Net.NetworkInformation;

namespace UtilitiTools.Models;
public readonly struct DeviceDetails
{
	public DeviceDetails(IPAddress ip, NetworkInterfaceType networkInterface)
	{
		IP = ip;
		InterfaceType = networkInterface;
	}
	public IPAddress IP { get; init; }
	public NetworkInterfaceType InterfaceType { get; init; }

}
