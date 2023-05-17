using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Model;
[Serializable]
internal class MessageDetails
{
    public MessageDetails(string content)
    {
        EncodingCodePage = Encoding.Unicode.CodePage;
        Length = content.Length;
    }
    public int EncodingCodePage { get; set; }
    public int Length { get; set; }
}
