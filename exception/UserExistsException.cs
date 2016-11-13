using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class UserExistsException : AuthentificationException
    {
        public string login { get; set; }

        public UserExistsException(string message) : base(message)
        {
            login = message;
        }

        public UserExistsException(string message, Exception innerException) : base(message, innerException)
        {
            login = message;
        }
    }
}
