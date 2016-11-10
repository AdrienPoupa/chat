using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.exception
{
    class UserExitsException : AuthentificationException
    {
        public UserExitsException(string message) : base(message)
        {

        }

        public UserExitsException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
