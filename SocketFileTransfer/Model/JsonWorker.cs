using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocketFileTransfer.Model;
internal sealed class JsonWorker
{
    public static async Task<byte[]> GetContentDetails(string content, ContentType contentType)
    {
        using MemoryStream stream = new();
        object model = ContentType.File == contentType
            ? new FileDetails(content)
            : new MessageDetails(content);
        await JsonSerializer.SerializeAsync(stream, model);
        return stream.ToArray();
    }
}
