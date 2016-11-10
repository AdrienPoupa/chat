using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class AuthentificationException: System.Exception
    {
        public string _login;

        public AuthentificationException()
        {

        }

        public AuthentificationException(string message) : base(message)
        {
            _login = message;
        }

        public AuthentificationException(string message, Exception innerException) : base(message, innerException)
        {
            _login = message;
        }
    }
}
