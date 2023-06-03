using System.Net;

namespace SocketFileTransfer.Model
{
	public class ConnectionDetails
	{
		public IPEndPoint EndPoint { get; set; }
		public TypeOfConnect TypeOfConnect { get; set; }
	}
}
