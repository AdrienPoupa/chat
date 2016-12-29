using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class UserUnknownException : AuthentificationException
    {
        public UserUnknownException(string message) : base(message)
        {
        }

        public UserUnknownException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
