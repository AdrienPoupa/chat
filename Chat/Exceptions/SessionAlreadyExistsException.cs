using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class SessionAlreadyExistsException : AuthentificationException
    {
        public SessionAlreadyExistsException(string message) : base(message)
        {
        }

        public SessionAlreadyExistsException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
