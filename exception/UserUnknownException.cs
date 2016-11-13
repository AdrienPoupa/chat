using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class UserUnknownException : AuthentificationException
    {
        public string login { get; set; }

        public UserUnknownException(string message) : base(message)
        {
            login = message;
        }

        public UserUnknownException(string message, Exception innerException) : base(message, innerException)
        {
            login = message;
        }
    }
}
