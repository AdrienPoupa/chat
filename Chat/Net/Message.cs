using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Net
{
    /// <summary>
    /// Store a message
    /// Each message is composed of a header, which describe its nature (register an user, join a chatroom, quit a chatroom, post a message, etc)
    /// Then, its content is stored into a list of strings
    /// </summary>
    [Serializable]
    public class Message
    {
        public enum Header { REGISTER, JOIN, QUIT, JOIN_CR, QUIT_CR, CREATE_CR, LIST_CR, POST, LIST_USERS }
        private Header head;
        private List<string> messageList;

        public Header Head
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

        public List<string> MessageList
        {
            get
            {
                return messageList;
            }

            set
            {
                messageList = value;
            }
        }

        public Message(Header head, string message)
        {
            this.Head = head;
            this.MessageList = new List<string>();
            this.MessageList.Add(message);
        }

        public Message(Header head)
        {
            this.Head = head;
            this.MessageList = new List<string>();
        }

        public Message(Header head, List<string> messages)
        {
            this.Head = head;
            this.MessageList = new List<string>();
            this.MessageList = messages;
        }

        /// <summary>
        /// Add data to the message list
        /// </summary>
        /// <param name="message"></param>
        public void addData(string message)
        {
            this.MessageList.Add(message);
        }
    }
}
