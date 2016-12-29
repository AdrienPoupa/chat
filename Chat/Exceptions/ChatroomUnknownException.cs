using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class ChatroomUnknownException : ChatroomException
    {
        public ChatroomUnknownException(string message) : base(message)
        {
        }

        public ChatroomUnknownException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
