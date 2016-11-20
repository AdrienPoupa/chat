using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class WrongPasswordException : System.Exception
    {
        public WrongPasswordException(string message) : base(message)
        {
        }

        public WrongPasswordException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}