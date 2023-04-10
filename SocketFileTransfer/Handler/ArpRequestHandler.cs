using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UtilitiTools;
using SocketFileTransfer.ExtendClass;

namespace SocketFileTransfer.Handler;
internal class ArpRequestHandler : ArpBase
{
	private readonly NetworkInterfaceType _interfaceType;
    
	public EventHandler<DeviceDetails> OnDeviceFound;
	public EventHandler<bool> OnScanComplete;

	public ArpRequestHandler(IPAddress address, NetworkInterfaceType interfaceType) : base(address) =>
		_interfaceType = interfaceType;

	public async Task GetNetWorkDevices()
	{
		var ipAddressess = base.GenerateIpList();
		await Parallel.ForEachAsync(ipAddressess, async (ipAddress, cts) =>
		{
			var responce = await base.CheckIpAdressWithARP(ipAddress);
			if (responce)
				OnDeviceFound.Raise(this, new DeviceDetails( ipAddress, _interfaceType));
		});
		OnScanComplete.Raise(this,EventArgs.Empty);
	}
}
