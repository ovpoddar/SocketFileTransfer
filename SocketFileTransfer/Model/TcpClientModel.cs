using System;
using System.Net;
using System.Net.Sockets;

namespace SocketFileTransfer.Model;
internal struct TcpClientModel : IDisposable
{
	public readonly byte[] Data { get; init; }
	public readonly NetworkStream Streams { get; init; }
	public readonly TcpClient Clients { get; init; }
	public string Name { get; }
	public IPEndPoint RemoteEndPoient { get; }
	public bool IsCreationSucced { get; } = true;
	public TcpClientModel(string identity, TcpClient tcp)
	{
		try
		{
			if (identity == null || string.IsNullOrWhiteSpace(identity))
				throw new Exception();

			Name = identity;
			// test this parsing too
			RemoteEndPoient = IPEndPoint.Parse(tcp.Client.RemoteEndPoint.ToString());
			Clients = tcp;
			Streams = tcp.GetStream();
			Data = new byte[tcp.ReceiveBufferSize];
		}
		catch
		{
			IsCreationSucced = false;
		}
	}

	public void Dispose()
	{
		Streams.Close();
		Streams.Dispose();
		Clients.Close();
		Clients.Dispose();
	}
}
