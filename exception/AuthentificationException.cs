using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class AuthentificationException: System.Exception
    {
        string _login;

        public AuthentificationException()
        {

        }

        public AuthentificationException(string message) : base(message)
        {

        }

        public AuthentificationException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public AuthentificationException(string login, string message) : base(message)
        {
            _login = login;
        }

        public AuthentificationException(string login, string message, Exception innerException) : base(message, innerException)
        {
            _login = login;
        }
    }
}
