using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using chat.client;
using chat.net;
using chat.server;

namespace chat.test
{
    class Server
    {
        public static void Main()
        {
            ServerGestTopics server = new ServerGestTopics();
            server.startServer(2300);
        }
    }
}
