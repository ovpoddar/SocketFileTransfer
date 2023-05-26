using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Handler;
internal static class ProjectStandardUtilitiesHelper
{
	public static async Task<bool> SendData(Socket socket, NetworkPacket packet)
	{
		var packetByte = (byte[])packet;
		var packetSize = packetByte.LongLength;
		var data = new byte[packetSize];
		Unsafe.WriteUnaligned(ref data[0], packetSize);
		await socket.SendAsync(data);
		var aData = new byte[packetSize];
		await socket.ReceiveAsync(aData);
		if (Enumerable.SequenceEqual(data, aData))
		{
			await socket.SendAsync(packetByte);
			return true;
		}
		else
		{
			return false;
		}
	}

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
		string response;
		switch (type)
		{
			case TypeOfConnect.Send:
				response = await ReadDetails(client);
				SendDetails(client, Dns.GetHostName());
				break;
			case TypeOfConnect.Received:
				SendDetails(client, Dns.GetHostName());
				response = await ReadDetails(client);
				break;
			default:
				throw new InvalidOperationException();
		}
		return response;
	}

	public static async Task<string> SendConnectSignalWithPort(TcpClient client)
	{
		var connectingPort = GeneratePort();
		var message = $"@@Connected::{connectingPort}";
		SendDetails(client, message.AsSpan());
		var receivedConfirmation = await ReadDetails(client);
		if (receivedConfirmation == "@Connected@")
			return connectingPort;
		return null;
	}

	public static string ReceivedTheConnectionPort(TcpClient client, byte[] messageAsBytes, int messageLength)
	{
		if (messageLength == 0)
			return null;

		var receivedMessage = Encoding.ASCII.GetString(messageAsBytes, 0, messageLength);
		if (!receivedMessage.StartsWith("@@Connected::"))
			return null;

		var slice = receivedMessage.Split("::");
		if (slice.Length != 2)
			return null;
		else
		{
			SendDetails(client, "@Connected@");
			return slice[1];
		}
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
		var response = await client.GetStream().ReadAsync(deviceNameAllocateByte);
		if (response == 0)
			return null;
		return Encoding.ASCII.GetString(deviceNameAllocateByte, 0 ,response)
			.AsSpan()
			.TrimEnd('\0')
			.ToString();
	}
}
