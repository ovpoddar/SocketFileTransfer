using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Handler;
internal static class ProjectStandaredUtilitiesHelper
{

	// exclude the virtual machines
	public static IEnumerable<(NetworkInterfaceType networkInterfaceType, UnicastIPAddressInformation ip)> DeviceNetworkInterfaceDiscovery() =>
		from address in NetworkInterface.GetAllNetworkInterfaces()
		where address.OperationalStatus == OperationalStatus.Up
			&& address.NetworkInterfaceType != NetworkInterfaceType.Loopback // removing localhost or 127.0.0.0 or 0.0.0.0
		let networkInterfaceType = address.NetworkInterfaceType
		let b = address.GetIPProperties().UnicastAddresses
		from ip in b
		where ip.Address.AddressFamily == AddressFamily.InterNetwork // removing ipv6 addresses
		select (networkInterfaceType, ip);

	public static async ValueTask<string> ExchangeInformation(TcpClient client, TypeOfConnect type)
	{
		string responce;
		switch (type)
		{
			case TypeOfConnect.Send:
				responce = await ReadDetails(client);
				SendDetails(client, Dns.GetHostName());
				break;
			case TypeOfConnect.Received:
				SendDetails(client, Dns.GetHostName());
				responce = await ReadDetails(client);
				break;
			default:
				throw new InvalidOperationException();
		}
		return responce;
	}

	public static string SendConnectSignalWithPort(TcpClient client)
	{
		var connectingPort = GeneratePort();
		var message = $"@@Connected::{connectingPort}";
		SendDetails(client, message.AsSpan());
		return connectingPort;
	}

	public static async Task<string> ReceivedTheConnectionPort(TcpClient client)
	{
		var receivedMessage = await ReadDetails(client);
		if (receivedMessage.StartsWith("@@Connected::"))
		{
			var slice = receivedMessage.Split("::");
			return slice.Length == 2 ? slice[1] : null;
		}
		return null;
	}

	static string GeneratePort()
	{
		Random random = new Random(DateTime.UtcNow.Month);
		var port = random.Next(1000, 1200);
		return port.ToString();
	}

	static void SendDetails(TcpClient client, ReadOnlySpan<char> message)
	{
		var deviceNameAsByte = Encoding.ASCII.GetBytes(message.ToString());
		client.GetStream().Write(deviceNameAsByte);
		client.GetStream().Flush();
	}

	static async ValueTask<string> ReadDetails(TcpClient client)
	{
		var deviceNameAllocateByte = new byte[client.ReceiveBufferSize];
		var responce = await client.GetStream().ReadAsync(deviceNameAllocateByte);
		if (responce  == 0)
			return null;
		return Encoding.ASCII.GetString(deviceNameAllocateByte);
	}


}
