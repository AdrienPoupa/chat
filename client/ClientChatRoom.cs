using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat.chat;
using chat.net;

namespace chat.client
{
    class ClientChatRoom : TCPClient, Chatroom
    {
        private Chatter chatter;
        //private string topic;
        private List<Message> messages = new List<Message>();
        private List<string> aliases = new List<string>();

        public String getTopic()
        {
            Message message;
            String topic = "";
            try
            {
                message = getMessage();
                topic = message.getData().First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return topic;
        }
        
        public void join(Chatter c)
        {
            try
            {
                List<string> temp = new List<string>();
                temp.Add(c.getAlias());
                //temp.Add(c.getPassword());
                
                sendMessage(new Message(Message.Header.JOIN_CR, temp));
                chatter = c;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public void post(String msg, Chatter c)
        {
            Message message = new Message(Message.Header.POST);
            message.addData(c.getAlias());
            message.addData(msg);
            try
            {
                sendMessage(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public void quit(Chatter c)
        {
            try
            {
                sendMessage(new Message(Message.Header.QUIT_CR, c.getAlias()));
                this.close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public void run()
        {
            try
            {
                Message message;
                while ((message = getMessage()) != null)
                {
                    switch (message.Head)
                    {
                        case Message.Header.JOIN_CR:
                        case Message.Header.JOIN_REPLY:
                        case Message.Header.JOIN_TOPIC:
                            aliases.Add(message.getData().First());
                            if (chatter != null)
                            {
                                chatter.joinNotification(new TextChatter(message.getData().First()));
                            }
                            break;
                        case Message.Header.GET:
                            messages.Add(message);
                            if (chatter != null)
                            {
                                chatter.receiveAMessage(message.getData().ElementAt(1), new TextChatter(message.getData().First()));
                            }
                            break;
                        case Message.Header.QUIT_CR:
                            aliases.Remove(message.getData().First());
                            if (chatter != null)
                            {
                                chatter.quitNotification(new TextChatter(message.getData().First()));
                            }
                            break;
                        // todo:post notification?
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
