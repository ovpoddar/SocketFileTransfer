using SocketFileTransfer.Canvas;
using SocketFileTransfer.Configuration;
using SocketFileTransfer.ExtendClass;
using SocketFileTransfer.Model;
using System;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Handler;

internal class PacketSender
{
	private readonly Socket _socket;

	public EventHandler<ProgressReport> ProgressEventHandler;
	public EventHandler<MessageReport> MessageEventHandler;

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
			var fileDetails = (FileDetails)header.Data;
			var fileInfo = new FileInfo(content);
			using var fileStream = fileInfo.OpenRead();
			long index = 0;
			var chunkSize = fileInfo.Length - index < 1024 * 1024 ? (int)(fileInfo.Length - index) : 1024 * 1024;
			while (index < fileInfo.Length)
			{
				chunkSize = fileInfo.Length - index < 1024 * 1024 ? (int)(fileInfo.Length - index) : 1024 * 1024;
				SendFile(fileStream, ref index, chunkSize);
				ProgressEventHandler.Raise(this, new ProgressReport(fileInfo.Length, index, fileDetails.FileHash));
			}
		}
		else if (contentType == ContentType.Message)
		{
			// might need a chunk version.
			var messageInfo = (MessageDetails)header.Data;
			var encoding = Encoding.GetEncoding(messageInfo.EncodingCodePage);
			var message = encoding.GetBytes(content);
			await _socket.SendAsync(message, SocketFlags.None);
			var report = new MessageReport()
			{
				EncodingPage = messageInfo.EncodingCodePage,
				Message = message
			};
			MessageEventHandler.Raise(this, report);
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
				ProgressEventHandler.Raise(this, new ProgressReport(fileSize, index, fileInfo.FileHash));
				if (index == fileSize)
					break;
			}
			fs.Close();
		}
		else if (ContentType.Message == networkPacket.PacketType)
		{
			// might need a chunk version
			var messageInfo = (MessageDetails)networkPacket.Data;
			var message = new byte[messageInfo.Length];
			await _socket.ReceiveAsync(message, SocketFlags.None);
			var report = new MessageReport()
			{
				EncodingPage = messageInfo.EncodingCodePage,
				Message = message,
			};
			MessageEventHandler.Raise(this, report);
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