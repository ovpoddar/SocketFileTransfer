using System.Net.Sockets;

namespace SocketFileTransfer.Model
{
	public class Connection
	{
		public Connection(TypeOfConnect typeOfConnect)
		{
			TypeOfConnect = typeOfConnect;
		}

		public Connection()
		{

		}

		public Socket Socket { get; set; }
		public TypeOfConnect TypeOfConnect { get; set; }
	}
}
