using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.chat
{
    interface Chatroom
    {
        void post(string msg, Chatter c);

        void quit(Chatter c);

        void join(Chatter c);

        string getTopic();
    }
}
