// Ignore Spelling: Arp

using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools;

namespace SocketFileTransfer.Handler;
internal class ArpRequestHandler : ArpBase
{
	private readonly NetworkInterfaceType _interfaceType;

	public EventHandler<DeviceDetails> OnDeviceFound;
	public EventHandler OnScanComplete;

	public ArpRequestHandler(IPAddress address, NetworkInterfaceType interfaceType) : base(address) =>
		_interfaceType = interfaceType;

	public async Task GetNetWorkDevices(CancellationToken cancellationToken)
	{
		var ipAddresses = base.GenerateIPList();
		var po = new ParallelOptions
		{
			CancellationToken = cancellationToken,
			MaxDegreeOfParallelism = Environment.ProcessorCount
		};

		try
		{
			await Parallel.ForEachAsync(ipAddresses,po, async (ipAddress, _) =>
			{
				var response = await CheckIPAddressWithARP(ipAddress);
				if (response)
					OnDeviceFound.Raise(this, new DeviceDetails(ipAddress, _interfaceType));
			});
		}
		catch { }
		OnScanComplete.Raise(this, EventArgs.Empty);
	}
}
