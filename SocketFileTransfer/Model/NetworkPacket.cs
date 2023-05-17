using System.Net.Sockets;

namespace SocketFileTransfer.Model;
internal struct NetworkPacket
{
	public long ContentSize { get; set; }
	public ContentType PacketType { get; set; }
	public byte[] Data { get; set; }
}
