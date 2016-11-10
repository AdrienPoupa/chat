using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chat.net
{
    abstract class TCPServer
    {
        private Socket commSocket;
        private Socket waitSocket; // todo: serversocket in the subject
        private int _port; // todo: get port from socket?

        public void startServer(int port)
        {
            IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[0];

            waitSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            waitSocket.Bind(new IPEndPoint(ipAddress, 8000));
            waitSocket.Listen(2000);

            Thread t = new Thread(new ThreadStart(this.run));
            t.Start();
        }

        public void stopServer()
        {
            waitSocket.Close();
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
