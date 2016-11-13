using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.chat
{
    class TextChatRoom : Chatroom
    {
        private List<Chatter> _users = new List<Chatter>();
        private string _topic;

        public TextChatRoom(string topic)
        {
            _topic = topic;
        }

        public void join(Chatter c)
        {
            if (!_users.Contains(c))
            {
                _users.Add(c);
                Console.WriteLine("(Message from Chatroom : " + _topic + ") " + c.getAlias() + " has joined the room.");
                for (int i = 0; i < _users.Count(); i++)
                {
                    _users.ElementAt(i).joinNotification(c);
                }
            }
        }

        public void post(String msg, Chatter c)
        {
            if (_users.Contains(c))
            {
                for (int i = 0; i < _users.Count(); i++)
                {
                    _users.ElementAt(i).receiveAMessage(msg, c);
                }
            }
            else
            {
                Console.WriteLine("ERROR : message \"" + msg + "\" could not be sent. Sender " + c.getAlias() + " is not in the chatroom");
            }
        }

        public void quit(Chatter c)
        {
            if (_users.Contains(c))
            {
                _users.Remove(c);
                Console.WriteLine("(Message from Chatroom : " + _topic + ") " + c.getAlias() + " has left the room.");
            }
            for (int i = 0; i < _users.Count(); i++)
            {
                _users.ElementAt(i).quitNotification(c);
            }
        }

        public string getTopic()
        {
            return _topic;
        }
    }
}
