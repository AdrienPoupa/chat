using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Chat
{
    [Serializable]
    public class Chatroom : IComparable<Chatroom>, INotifyPropertyChanged
    {
        string name;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
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

        public override string ToString()
        {
            return name;
        }
    }
}
