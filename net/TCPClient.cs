using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace chat.net
{
    class TCPClient
    {
        private int _port; // todo: get port from socket?
        private Socket sock;
        private IPAddress _adr;

        public void setServer(IPAddress adr, int port)
        {
            // todo
            _port = port;
            _adr = adr;
        }

        public void connect()
        {
            // todo
        }
    }
}
