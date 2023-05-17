using System.Net;
using System.Runtime.InteropServices;

namespace UtilityTools;

public abstract class ArpBase
{
	private readonly IPAddress _startAddress;
	private readonly IPAddress _address;

	public ArpBase(IPAddress address)
	{
		var addressAsSpan = address.ToString().AsSpan();
		var lastIndexOfDot = addressAsSpan.LastIndexOf('.') + 1;
		_startAddress = IPAddress.Parse(addressAsSpan.Slice(0, lastIndexOfDot).ToString() + "0");
		_address = address;
	}

	public IEnumerable<IPAddress> GenerateIpList()
	{
		var tempRoleIp = _startAddress;
		for (var i = 0; i < 255; i++)
		{
			tempRoleIp = IncrementIp(tempRoleIp);
			if (tempRoleIp.ToString().AsSpan().SequenceEqual(_address.ToString().AsSpan()))
				continue;
			yield return tempRoleIp;
		}
	}

	IPAddress IncrementIp(IPAddress ip)
	{
		var ipAsByte = ip.GetAddressBytes();

		if (++ipAsByte[3] == 0)
			if (++ipAsByte[2] == 0)
				if (++ipAsByte[1] == 0)
					++ipAsByte[0];

		return new IPAddress(ipAsByte);
	}

	public async Task<bool> CheckIpAddressWithARP(IPAddress ipAddress)
	{
		var macAddress = new byte[6];
		var len = (uint)macAddress.Length;
		var result = 0;
		await Task.Run(() =>
		{
			result = SendARP(BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0), 0, macAddress, ref len);

		});
		return result == 0;
	}

	#region Native Methods
	[DllImport("iphlpapi.dll", ExactSpelling = true)]
	private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
	#endregion
}
