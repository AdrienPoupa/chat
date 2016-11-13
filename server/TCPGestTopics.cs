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
    class TCPGestTopics : TextGestTopics
    {
        private static int nextPort = 16000;
        private bool started;
        private Dictionary<string, ServerChatRoom> chatrooms = new Dictionary<string, ServerChatRoom>();

        public void createTopic(string topic)
        {
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

        public List<string> listTopics()
        {
            List<string> topics = new List<string>();
            return topics;
        }
    }
}
