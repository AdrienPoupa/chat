using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat.net;
using chat.chat;

namespace chat.client
{
    class ClientGestTopics : TCPClient
    {
        private Chatter chatter;

        private string topic;

        public string Topic
        {
            get
            {
                return topic;
            }

            set
            {
                topic = value;
            }
        }

        internal Chatter Chatter
        {
            get
            {
                return chatter;
            }

            set
            {
                chatter = value;
            }
        }
    }
}
