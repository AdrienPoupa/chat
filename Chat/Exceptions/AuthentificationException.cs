using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Exceptions
{
    public class AuthentificationException : System.Exception
    {
        public AuthentificationException(string message) : base(message)
        {
        }

        public AuthentificationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
