using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.chat
{
    class TextChatter : Chatter
    {
        private string _alias;
        private string _password;

        public TextChatter(string alias)
        {
            _alias = alias;
        }

        public TextChatter(string alias, string password)
        {
            _alias = alias;
            _password = password;
        }
        public void receiveAMessage(string msg, Chatter c)
        {
            Console.WriteLine("(At " + _alias + ") : " + c.getAlias() + " $> " + msg);
        }

        public string getAlias()
        {
            return _alias;
        }

        public string getPassword()
        {
            return _password;
        }

        public void joinNotification(Chatter c)
        {
            // todo: fonction inutile?
            //Console.WriteLine(c.getAlias() + " joined");
        }
        
        public void quitNotification(Chatter c)
        {
            // todo: fonction inutile?
            //Console.WriteLine(c.getAlias() + " left");
        }
    }
}
