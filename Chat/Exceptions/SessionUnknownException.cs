using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class SessionUnknownException : AuthentificationException
    {
        public SessionUnknownException(string message) : base(message)
        {
        }

        public SessionUnknownException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
