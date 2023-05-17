using SocketFileTransfer.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Model;
[Serializable]
internal class FileDetails
{
    public FileDetails(string context)
    {
        var fileInfo = new FileInfo(context);
        Name = fileInfo.Name;
        Size = fileInfo.Length;
        Type = fileInfo.Extension;
        ChunkSize = (int)Math.Ceiling((double)fileInfo.Length / ProjectStandardUtilitiesHelper.ChunkSize);
    }
    public string Name { get; set; }
    public long Size { get; set; }
    public int ChunkSize { get; set; }
    public string Type { get; set; }
}
