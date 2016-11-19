using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using chat.authentification;
using chat.net;
using chat.chat;

namespace chat.server
{
    class ServerChatRoom : TCPServer, Chatter
    {
        private string pseudo;
        private string password;
        private TextChatRoom textChatroom;

        public ServerChatRoom(Chatroom chatroom)
        {
            this.textChatroom = (TextChatRoom)chatroom;
        }

        
        public override void gereClient(int port)
        {
            try
            {
                Message inputMessage;

                while ((inputMessage = getMessage()) != null)
                {
                    //Message inputMessage = getMessage();
                    switch (inputMessage.Head)
                    {
                        case Message.Header.JOIN_CR:
                            break;
                        case Message.Header.JOIN_REPLY:
                            break;
                        case Message.Header.JOIN_TOPIC:
                            pseudo = inputMessage.getData().First();
                            password = inputMessage.getData().ElementAt(1);
                            Authentification am = new Authentification();
                            try
                            {
                                am.authentify(pseudo, password);
                                textChatroom.join(this);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            //joinNotification(this);
                            break;
                        case Message.Header.POST:
                            string message = inputMessage.getData().ElementAt(1);
                            textChatroom.post(message, this);
                            break;
                        case Message.Header.QUIT_CR:
                            textChatroom.quit(this);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        
        public string getAlias()
        {
            return pseudo;
        }

        
        public string getPassword()
        {
            return password;
        }

        
        public void receiveAMessage(string msg, Chatter c)
        {
            List<string> data = new List<string>(2);
            data.Add(c.getAlias());
            data.Add(msg);

            try
            {
                sendMessage(new Message(Message.Header.GET, data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        
        public void joinNotification(Chatter c)
        {
            List<string> data = new List<string>(1);
            data.Add(c.getAlias());
            try
            {
                sendMessage(new Message(Message.Header.JOIN_TOPIC, data)); // todo: proper join
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        
        public void quitNotification(Chatter c)
        {
            List<string> data = new List<string>(1);
            data.Add(c.getAlias());

            try
            {
                sendMessage(new Message(Message.Header.QUIT_CR, data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override TCPServer cloneInstance()
        {
            return this;
        }
    }
}
