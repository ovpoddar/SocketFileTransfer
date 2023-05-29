using SocketFileTransfer.Configuration;
using SocketFileTransfer.Model;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SocketFileTransfer.Handler;

internal class PacketSender
{
	private readonly Socket _socket;

	public EventHandler<ProgressReport> EventHandler;

	public PacketSender(Socket socket)
	{
		_socket = socket ?? throw new ArgumentNullException(nameof(socket));
		if (!socket.Connected)
			throw new ArgumentNullException(nameof(socket));
	}

	public async Task SendContent(string content, ContentType contentType)
	{
		if (string.IsNullOrWhiteSpace(content))
			throw new ArgumentNullException(content);

		var header = GetPacketInfo(content, contentType);
		await _socket.SendAsync((byte[])header, SocketFlags.None);

		if (contentType == ContentType.File)
		{
			var fileInfo = new FileInfo(content);
			using var fileStream = fileInfo.OpenRead();
			long index = 0;
			var chunkSize = fileInfo.Length - index < 1024 * 1024 ? (int)(fileInfo.Length - index) : 1024 * 1024;
			while (index < fileInfo.Length)
			{
				chunkSize = fileInfo.Length - index < 1024 * 1024 ? (int)(fileInfo.Length - index) : 1024 * 1024;
				SendFile(fileStream, ref index, chunkSize);
				EventHandler.Invoke(this, new ProgressReport(fileInfo.Length, index));
			}
			var confirmation = new byte[2];
			await _socket.ReceiveAsync(confirmation);
		}
	}

	void SendFile(FileStream fileStream, ref long index, int chunkSize)
	{
		Span<byte> sendBytes = stackalloc byte[chunkSize];
		fileStream.Seek(index, SeekOrigin.Begin);
		fileStream.Read(sendBytes);
		index += _socket.Send(sendBytes, SocketFlags.None);
	}

	public async Task ReceivedContent(NetworkPacket networkPacket)
	{
		if (ContentType.File == networkPacket.PacketType)
		{
			var fileInfo = (FileDetails)networkPacket.Data;
			var filePath = Path.Combine(StaticConfiguration.StoredLocation, fileInfo.Name);
			var fileSize = fileInfo.Size;
			CreateFile(filePath);

			long index = 0;
			using var fs = new FileStream(filePath, FileMode.Append);
			int writingChunk = fileSize - index < 1024 * 1024 ? (int)(fileSize - index) : 1024 * 1024;
			while (index <= fileSize)
			{
				writingChunk = fileSize - index < 1024 * 1024 ? (int)(fileSize - index) : 1024 * 1024;
				ReceivedFile(fs, ref index, writingChunk);
				EventHandler.Invoke(this, new ProgressReport(fileSize, index));
				if (index == fileSize)
					break;
			}
			fs.Close();
			var confirmation = new byte[2];
			Unsafe.WriteUnaligned(ref confirmation[0], true);
			await _socket.SendAsync(confirmation);
		}
	}

	void ReceivedFile(FileStream fileStream, ref long index, int chunkSize)
	{
		Span<byte> writingChunk = stackalloc byte[chunkSize];
		fileStream.Seek(index, SeekOrigin.Begin);
		index += _socket.Receive(writingChunk, SocketFlags.None);
		fileStream.Write(writingChunk);
	}

	static void CreateFile(string path)
	{
		var sr = File.Create(path);
		sr.Dispose();
	}

	static NetworkPacket GetPacketInfo(string content, ContentType contentType)
	{
		var fileDetails = contentType switch
		{
			ContentType.File => (byte[])new FileDetails(content),
			ContentType.Message => (byte[])new MessageDetails(content),
			_ => null
		};
		return new NetworkPacket
		{
			PacketType = contentType,
			Data = fileDetails,
			ContentSize = fileDetails.Length + sizeof(short)
		};
	}
}