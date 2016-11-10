using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace chat.net
{
    abstract class TCPServer
    {
        private Socket commSocket;
        private Socket waitSock; // todo: serversocket in the subject
        private int _port; // todo: get port from socket?

        public void startServer(int port)
        {
            // todo
            _port = port;
        }

        public void stopServer()
        {
            // todo
        }

        public void run()
        {
            // todo
        }

        public int getPort()
        {
            return _port;
        }

        public abstract void gereClient(Socket comm);
    }
}
