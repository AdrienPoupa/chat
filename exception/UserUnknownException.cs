using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class UserUnknownException : AuthentificationException
    {
        public UserUnknownException(string message) : base(message)
        {

        }

        public UserUnknownException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
