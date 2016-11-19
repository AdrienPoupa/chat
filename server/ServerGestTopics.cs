using chat.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace chat.server
{
    [Serializable]
    class ServerGestTopics : TCPServer
    {
        private TCPGestTopics tcpTopicsManager;

        public override void gereClient(int port)
        {
            tcpTopicsManager = new TCPGestTopics();

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
                            
                                Message outputMessage = new Message(Message.Header.JOIN_TOPIC, port.ToString());
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

        public override TCPServer cloneInstance()
        {
            ServerGestTopics newInstance = new ServerGestTopics();

            newInstance.mode = Mode.treatClient;
            newInstance._port = _port;
            newInstance.waitSocket = waitSocket;
            newInstance.commSocket = newInstance.waitSocket.Accept();

            return newInstance;
        }
    }
}
