using System.Net.Sockets;

namespace SocketFileTransfer.Model;
internal readonly struct TcpClientModel
{
	public readonly Socket Socket { get; init; }
	public byte[] Bytes { get; }

	public TcpClientModel(Socket socket, byte[] bytes)
	{
		Socket = socket;
		Bytes = bytes;
	}

}
