using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class ChatroomException : System.Exception
    {
        public ChatroomException(string message) : base(message)
        {
        }

        public ChatroomException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
