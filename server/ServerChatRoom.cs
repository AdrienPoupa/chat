using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using chat.net;
using chat.chat;

namespace chat.server
{
    class ServerChatRoom : TCPServer
    {
        private Dictionary<string,TextChatRoom> textChatRoom = new Dictionary<string, TextChatRoom>();

        public override void gereClient(Socket comm)
        {
            throw new NotImplementedException();
        }

        public void recevoirMessage()
        {

        }
    }
}
