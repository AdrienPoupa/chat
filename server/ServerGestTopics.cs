using chat.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                                
                                // todo: tester validité du port
                                // http://stackoverflow.com/questions/1904160/getting-the-ip-address-of-a-remote-socket-endpoint
                                string port = ((IPEndPoint)(comm.RemoteEndPoint)).Port.ToString();
                            
                                Message outputMessage = new Message(Message.Header.JOIN_TOPIC, port);
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
