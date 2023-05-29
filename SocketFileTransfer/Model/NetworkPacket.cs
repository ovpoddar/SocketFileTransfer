using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SocketFileTransfer.Model;
public struct NetworkPacket
{
	public long ContentSize { get; set; }
	public ContentType PacketType { get; set; }
	public byte[] Data { get; set; }

	public static explicit operator byte[](NetworkPacket networkPacket)
	{
		var size = Marshal.SizeOf(networkPacket.ContentSize)
			+ sizeof(short)
			+ (Marshal.SizeOf(networkPacket.Data[0]) * networkPacket.Data.Length);
		var result = new byte[size];
		var index = 0;
		Unsafe.WriteUnaligned(ref result[index], networkPacket.ContentSize);
		index += Marshal.SizeOf(networkPacket.ContentSize);
		Unsafe.WriteUnaligned(ref result[index], networkPacket.PacketType);
		index += sizeof(short);
		Array.Copy(networkPacket.Data, 0, result, index, networkPacket.Data.Length);
		return result;
	}

	public static implicit operator NetworkPacket(byte[] data)
	{
		try
		{
			var result = new NetworkPacket();
			var index = 0;
			result.ContentSize = Unsafe.ReadUnaligned<long>(ref data[index]);
			index += Marshal.SizeOf(result.ContentSize);
			result.PacketType = Unsafe.ReadUnaligned<ContentType>(ref data[index]);
			index += sizeof(short);
			result.Data = new byte[result.ContentSize];
			Array.Copy(data, index, result.Data, 0, result.ContentSize);
			return result;
		}
		catch
		{
			return null;
		}
	}
}
