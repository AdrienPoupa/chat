using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Chat
{
    [Serializable]
    public class Chatroom : IComparable<Chatroom>
    {
        string name;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Chatroom(string name)
        {
            this.Name = name;
        }

        public int CompareTo(Chatroom other)
        {
            if (this.Name == other.Name)
                return 0;

            return -1;
        }
    }
}
