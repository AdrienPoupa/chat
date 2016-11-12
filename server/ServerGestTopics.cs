using chat.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace chat.server
{
    class ServerGestTopics : TCPServer
    {
        private TCPGestTopics tcpTopicsManager = new TCPGestTopics();

        public override void gereClient(Socket comm)
        {
            try
            {
                Message inputMessage;

                while ((inputMessage = getMessage()) != null)
                {
                    switch (inputMessage.Head)
                    {
                        case Message.Header.LIST_TOPICS:
                            {
                                List<string> topics = tcpTopicsManager.listTopics();
                                Message outputMessage = new Message(Message.Header.LIST_TOPICS, topics);
                                sendMessage(outputMessage);
                            }
                            break;

                        case Message.Header.CREATE_TOPIC:
                            tcpTopicsManager.createTopic(inputMessage.Data.First());
                            break;

                        case Message.Header.JOIN_TOPIC:
                            {
                                string topicToJoin = inputMessage.Data.First();

                                Message outputMessage = new Message(Message.Header.JOIN_TOPIC, "1000"); //todo, port
                                sendMessage(outputMessage);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
