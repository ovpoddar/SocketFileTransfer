﻿// Ignore Spelling: Arp

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
	public EventHandler<bool> OnScanComplete;

	public ArpRequestHandler(IPAddress address, NetworkInterfaceType interfaceType) : base(address) =>
		_interfaceType = interfaceType;
	//TODO: prepare cancellation token
	public async Task GetNetWorkDevices()
	{
		var ipAddresses = base.GenerateIpList();
		try
		{
			await Parallel.ForEachAsync(ipAddresses, async (ipAddress, cts) =>
			{
				var response = await base.CheckIpAddressWithARP(ipAddress);
				if (cts.IsCancellationRequested == true)
				{
					cts.ThrowIfCancellationRequested();
				}
				if (response)
					OnDeviceFound.Raise(this, new DeviceDetails(ipAddress, _interfaceType));
			});
		}
		catch { }
		OnScanComplete.Raise(this, EventArgs.Empty);
	}
}
