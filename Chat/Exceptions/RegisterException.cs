using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class RegisterException : System.Exception
    {
        public RegisterException(string message) : base(message)
        {
        }

        public RegisterException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}