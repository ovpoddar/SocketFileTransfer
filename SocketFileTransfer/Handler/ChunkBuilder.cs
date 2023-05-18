using SocketFileTransfer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Handler;

internal class ChunkBuilder
{
	private readonly ContentType _contentType;
	private readonly string _content;

	public ChunkBuilder(ContentType contentType, string content)
	{
		_contentType = contentType;
		_content = content;
		VerifyInputArgument();
	}

	private void VerifyInputArgument()
	{
		if (string.IsNullOrWhiteSpace(_content)) { throw new ArgumentException("Content Cant be Empty.", nameof(_content)); }
		if (_contentType == ContentType.File && !File.Exists(_content)) { throw new FileNotFoundException("File Could not Found.", _content); }
	}

	internal async IAsyncEnumerable<NetworkPacket> Get(int index = 0)
	{
		if (index == 0)
			yield return await GetPacketInfo();

		if (_contentType == ContentType.File)
		{
			var stream = File.OpenRead(_content);
			stream.Seek(index, SeekOrigin.Begin);

			while (index < stream.Length)
			{
				var chunk = new byte[(stream.Length - index) < ProjectStandardUtilitiesHelper.ChunkSize
					? stream.Length - index
					: ProjectStandardUtilitiesHelper.ChunkSize];
				stream.Read(chunk, 0, chunk.Length);
				index += chunk.Length;
				yield return new NetworkPacket
				{
					PacketType = ContentType.File,
					Data = chunk,
					ContentSize = chunk.Length
				};
			}
		}
		else
		{
			while (index < _content.Length)
			{
				var chunk = _content.Substring(index, (_content.Length - index) < ProjectStandardUtilitiesHelper.ChunkSize
					? _content.Length - index
					: ProjectStandardUtilitiesHelper.ChunkSize);

				index += chunk.Length;
				yield return new NetworkPacket
				{
					PacketType = ContentType.Message,
					Data = Encoding.Unicode.GetBytes(chunk, 0, chunk.Length),
					ContentSize = chunk.Length
				};
			}
		}
	}

	async Task<NetworkPacket> GetPacketInfo()
	{
		var fileDetails = await JsonWorker.GetContentDetails(_content, _contentType);
		return new NetworkPacket
		{
			PacketType = _contentType | ContentType.Information,
			Data = fileDetails,
			ContentSize = fileDetails.Length,
		};
	}
}