using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Handler;

internal class ChunkBuilder
{
	private readonly ContentType _contentType;
	private readonly string _content;
	private readonly Guid _chunkBuilderId;

	public ChunkBuilder(ContentType contentType, string content)
	{
		_contentType = contentType;
		_content = content;
		VerifyInputArgument();
		_chunkBuilderId = Guid.NewGuid();
	}

	private void VerifyInputArgument()
	{
		if (string.IsNullOrWhiteSpace(_content)) { throw new ArgumentException("Content Cant be Empty.", nameof(_content)); }
		if (_contentType == ContentType.File && !File.Exists(_content)) { throw new FileNotFoundException("File Could not Found.", _content); }
	}

	internal IEnumerable<NetworkPacket> Get(int index = 0, int chunkSize = 1024 * 1024 * 10)
	{
		if (index == 0)
			yield return GetPacketInfo();

		if (_contentType == ContentType.File)
		{
			var stream = File.OpenRead(_content);
			stream.Seek(index, SeekOrigin.Begin);

			while (index < stream.Length)
			{
				var chunk = new byte[(stream.Length - index) < chunkSize
					? stream.Length - index
					: chunkSize];
				stream.Read(chunk, 0, chunk.Length);
				index += chunk.Length;
				yield return new NetworkPacket
				{
					PacketType = ContentType.File,
					Data = chunk,
					ContentSize = chunk.Length,
					PacketId = _chunkBuilderId
				};
			}
		}
		else
		{
			while (index < _content.Length)
			{
				var chunk = _content.Substring(index, (_content.Length - index) < chunkSize
					? _content.Length - index
					: chunkSize);

				index += chunk.Length;
				yield return new NetworkPacket
				{
					PacketType = ContentType.Message,
					Data = Encoding.Unicode.GetBytes(chunk, 0, chunk.Length),
					ContentSize = chunk.Length,
					PacketId = _chunkBuilderId
				};
			}
		}
	}

	NetworkPacket GetPacketInfo()
	{
		var fileDetails = _contentType switch
		{
			ContentType.File => (byte[])new FileDetails(_content),
			ContentType.Message => (byte[])new MessageDetails(_content),
			_ => null
		};
		return new NetworkPacket
		{
			PacketType = _contentType | ContentType.Information,
			Data = fileDetails,
			ContentSize = fileDetails.Length,
			PacketId = _chunkBuilderId
		};
	}
}