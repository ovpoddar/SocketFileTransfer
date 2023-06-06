using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Handler;
internal static class ProjectStandardUtilitiesHelper
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

	public static async Task<bool> SendConnectSignal(Socket socket)
	{
		var message = "@@Connected@@";
		SendDetails(socket, message.AsSpan());
		var receivedConfirmation = await ReadDetails(socket);
		return receivedConfirmation == "@Connected@";
	}

	public static bool ReceivedConnectedSignal(byte[] messageAsBytes, int messageLength)
	{
		if (messageLength == 0)
			return false;

		var receivedMessage = Encoding.ASCII.GetString(messageAsBytes, 0, messageLength);
		return receivedMessage == "@@Connected@@";
	}

	public static byte[] GetHashCode(string filePath)
	{
		using (var cryptoService = SHA256.Create())
		{
			using (var fileStream = new FileStream(filePath,
												   FileMode.Open,
												   FileAccess.Read,
												   FileShare.ReadWrite))
			{
				var hash = cryptoService.ComputeHash(fileStream);
				return hash;
			}
		}
	}

	public static async ValueTask<string> ExchangeInformation(Socket client, TypeOfConnect type)
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

	static async ValueTask<string> ReadDetails(Socket client)
	{
		var deviceNameAllocateByte = new byte[client.ReceiveBufferSize];
		var response = await client.ReceiveAsync(deviceNameAllocateByte);
		if (response == 0)
			return null;
		return Encoding.ASCII.GetString(deviceNameAllocateByte, 0, response)
			.AsSpan()
			.TrimEnd('\0')
			.ToString();
	}

	static void SendDetails(Socket client, ReadOnlySpan<char> message)
	{
		var deviceNameAsByte = Encoding.ASCII.GetBytes(message.ToString());
		client.Send(deviceNameAsByte, SocketFlags.None);
	}

}
