using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chat.net;
using chat.chat;

namespace chat.client
{
    class ClientGestTopics : TCPClient, TopicsManager
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

        public string createTopic(string topic)
        {
            Message message = new Message(Message.Header.CREATE_TOPIC, topic);
            try
            {
                sendMessage(message);
                return topic;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }


        public Chatroom joinTopic(String topic)
        {
            Message message = new Message(Message.Header.JOIN_TOPIC, topic);
            try
            {
                sendMessage(message);
                Message answer = getMessage();
                int port = Int32.Parse(answer.getData().First());
                ClientChatRoom chatroom = new ClientChatRoom();
                chatroom.setServer(getAddress(), port);
                chatroom.connect();
                new Thread(new ThreadStart(chatroom.run));
                return chatroom;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public List<string> listTopics()
        {
            Message message = new Message(Message.Header.LIST_TOPICS);
            List<string> topics = null;
            try
            {
                sendMessage(message);
                Message answer = getMessage();
                topics = answer.getData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return topics;
        }
    }
}
