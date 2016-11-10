using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class ChatroomAlreadyExistsException : System.Exception
    {
        public string _chatroom;

        public ChatroomAlreadyExistsException()
        {

        }

        public ChatroomAlreadyExistsException(string message) : base(message)
        {
            _chatroom = message;
        }

        public ChatroomAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
            _chatroom = message;
        }
    }
}
