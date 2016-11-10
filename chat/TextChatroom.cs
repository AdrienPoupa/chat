using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.chat
{
    class TextChatroom : Chatroom
    {
        private List<Chatter> _users = new List<Chatter>();
        private string _topic;

        public TextChatroom(string topic)
        {
            _topic = topic;
        }

        public void join(Chatter c)
        {
            if (!_users.Contains(c))
            {
                _users.Add(c);
                //System.out.println("(Message from Chatroom : " + _topic + ") " + c.getPseudo() + " has joined the room.");
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
                //System.out.println("ERROR : message \"" + msg + "\" could not be sent. Sender " + c.getPseudo() + " not in the chatroom");
            }
        }

        public void quit(Chatter c)
        {
            if (_users.Contains(c))
            {
                _users.Remove(c);
                //System.out.println("(Message from Chatroom : " + _topic + ") " + c.getPseudo() + " has quit the room.");
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
