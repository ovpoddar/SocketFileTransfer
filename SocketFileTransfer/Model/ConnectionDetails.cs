using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Model
{
    public class ConnectionDetails
    {
        public IPEndPoint EndPoint { get; set; }
        public TypeOfConnect TypeOfConnect { get; set; }
    }
}
