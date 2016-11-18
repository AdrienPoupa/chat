using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using chat.client;

namespace chat.test
{
    class Client
    {
        public static void Main()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            ClientGestTopics cgt = new ClientGestTopics();
            cgt.setServer(ipAddress, 2300);
            cgt.connect();
            cgt.createTopic("Test topic");
        }
    }
}
