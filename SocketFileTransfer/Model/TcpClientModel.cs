using System;
using System.Net;
using System.Net.Sockets;

namespace SocketFileTransfer.Model;
internal struct TcpClientModel : IDisposable
{
	public byte[] Data { get; set; }
	public readonly NetworkStream Streams { get; init; }
	public readonly TcpClient Clients { get; init; }
	public string Name { get; }
	public IPEndPoint RemoteEndPoint { get; }
	public bool IsCreationSucceed { get; } = true;
	public TcpClientModel(string identity, TcpClient tcp)
	{
		try
		{
			if (identity == null || string.IsNullOrWhiteSpace(identity))
				throw new Exception();

			Name = identity;
			// test this parsing too
			RemoteEndPoint = IPEndPoint.Parse(tcp.Client.RemoteEndPoint.ToString());
			Clients = tcp;
			Streams = tcp.GetStream();
		}
		catch
		{
			IsCreationSucceed = false;
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
