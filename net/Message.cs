using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace chat.net
{
    class Message
    {
        private Header head;
        private List<string> data;

        public Message(byte[] bytes, int bytesRec)
        {
            string received = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            // todo: crade. Dans l'absolu, on n'a même pas besoin d'avoir List<string> pour data, vu qu'on a déjà une string en réception...
            data.Add(received);
        }

        public string toString()
        {
            return head + data.ToString();
        }
    }
}
