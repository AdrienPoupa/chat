using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class UserAlreadyExistsException : AuthentificationException
    {
        public UserAlreadyExistsException(string message) : base(message)
        {
        }

        public UserAlreadyExistsException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
