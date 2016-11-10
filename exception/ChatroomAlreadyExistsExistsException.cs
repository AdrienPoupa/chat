using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class ChatroomAlreadyExistsExistsException : System.Exception
    {
        public string _chatroom;

        public ChatroomAlreadyExistsExistsException()
        {

        }

        public ChatroomAlreadyExistsExistsException(string message) : base(message)
        {
            _chatroom = message;
        }

        public ChatroomAlreadyExistsExistsException(string message, Exception innerException) : base(message, innerException)
        {
            _chatroom = message;
        }
    }
}
