using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.chat
{
    interface TopicsManager
    {
        List<string> listTopics();

        Chatroom joinTopic(string topic);

        string createTopic(string topic);
    }
}
