using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace chat.net
{
    class Message
    {
        public enum Header { DEBUG, LIST_TOPICS, LISTE_TOPICS_REPLY, CREATE_TOPIC, JOIN_TOPIC, JOIN_REPLY, POST, GET, JOIN_CR, QUIT_CR }
        private Header head;
        private List<string> data;

        internal Header Head
        {
            get
            {
                return head;
            }

            set
            {
                head = value;
            }
        }

        public List<string> Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public Message(Header head, string message)
        {
            this.Head = head;
            this.Data.Add(message);
        }

        public Message(Header head, List<string> messages)
        {
            this.Head = head;
            this.Data = messages;
        }

        public void addData(string message)
        {
            this.Data.Add(message);
        }

        /*public Message(byte[] bytes, int bytesRec)
        {
            string received = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            data.Add(received);
        }*/

        public string toString()
        {
            return Head + " / " + Data.ToString();
        }
    }
}
