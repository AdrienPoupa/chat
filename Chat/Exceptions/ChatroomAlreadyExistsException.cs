using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class ChatroomAlreadyExistsException : ChatroomException
    {
        public ChatroomAlreadyExistsException(string message) : base(message)
        {
        }

        public ChatroomAlreadyExistsException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
