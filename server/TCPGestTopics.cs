using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat.chat;
using chat.exception;

namespace chat.server
{
    [Serializable]
    class TCPGestTopics : TextGestTopics
    {
        private static int nextPort = 16000;
        private bool started;
        private Dictionary<string, ServerChatRoom> chatrooms;

        public new void createTopic(string topic)
        {
            chatrooms = new Dictionary<string, ServerChatRoom>();

            try
            {
                base.createTopic(topic);
                Chatroom chatroom = joinTopic(topic);
                ServerChatRoom serverChatroom = new ServerChatRoom(chatroom);
                chatrooms.Add(topic, serverChatroom);
                started = true;
                do
                {
                    try
                    {
                        serverChatroom.startServer(nextPort);
                        started = true;
                    }
                    catch (IOException e)
                    {
                        started = false;
                        Console.WriteLine(e);
                    }
                    nextPort++;
                } while (!started);
            }
            catch (ChatroomAlreadyExistsException e)
            {
                Console.WriteLine(e);
            }
        }

        public new List<string> listTopics()
        {
            List<string> topics = new List<string>();
            return topics;
        }
    }
}
