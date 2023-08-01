using System.Net.Sockets;

namespace SocketFileTransfer.Model;

public class Connection
{
	public Connection(TypeOfConnect typeOfConnect)
	{
		TypeOfConnect = typeOfConnect;
	}
	public Connection(Socket[] serverSocket, TypeOfConnect typeOfConnect)
	{
		ServerSockets = serverSocket;
		TypeOfConnect = typeOfConnect;
	}
	public Connection()
	{

	}

	public Socket Socket { get; set; }
	public TypeOfConnect TypeOfConnect { get; set; }
	public Socket[] ServerSockets { get; set; }
}
