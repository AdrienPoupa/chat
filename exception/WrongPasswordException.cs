using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class WrongPasswordException : AuthentificationException
    {
        public WrongPasswordException(string message) : base(message)
        {

        }

        public WrongPasswordException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}