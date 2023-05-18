using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SocketFileTransfer.Model;
internal struct NetworkPacket
{
    public long ContentSize { get; set; }
    public ContentType PacketType { get; set; }
    public byte[] Data { get; set; }
    public static explicit operator byte[](NetworkPacket networkPacket)
    {
        var size = sizeof(long) 
            + sizeof(ContentType) 
            + (Marshal.SizeOf(networkPacket.Data[0]) 
                * networkPacket.Data.Length);
        var result = new byte[size];
        Unsafe.WriteUnaligned(ref result[0], networkPacket.ContentSize);
        Unsafe.WriteUnaligned(ref result[3], networkPacket.ContentSize);
        Array.Copy(networkPacket.Data, 0, result, 5, networkPacket.Data.Length);
        return result;
    }

    public static explicit operator NetworkPacket(byte[] data)
    {
        var result = new NetworkPacket();
        result.ContentSize = Unsafe.ReadUnaligned<long>(ref data[0]);
        result.PacketType = Unsafe.ReadUnaligned<ContentType>(ref data[3]);
        Array.Copy(data, 5, result.Data, 0,data.Length - 5);
        return result;
    }
}
