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
			var identitySplit = identity.Split(":-");
			if (identitySplit.Length != 2 || string.IsNullOrEmpty(identitySplit[0]) || string.IsNullOrEmpty(identitySplit[0]))
				throw new Exception();

			Name = identitySplit[0];
			// test this parsing too
			RemoteEndPoient = IPEndPoint.Parse(identitySplit[1]);
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
		if(Streams.CanWrite)
		{
			Streams.Close();
			Streams.Dispose();
		}
		Clients.Close();
		Clients.Dispose();
		GC.SuppressFinalize(this);
	}
}
