using System.Net;

namespace SocketFileTransfer.Models
{
    public class ConnectionDetails
    {
        public IPEndPoint EndPoint { get; set; }
        public TypeOfConnect TypeOfConnect { get; set; }
    }
}
