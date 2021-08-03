using System.Net.Sockets;

namespace SocketFileTransfer.Model
{
    public class TransfarModel
    {
        public byte[] Buffer { get; set; }
        public string Ip { get; set; }
        public int Id { get; set; }
        public TcpClient Client { get; set; }
    }
}
