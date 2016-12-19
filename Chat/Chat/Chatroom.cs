using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Chat
{
    /// <summary>
    /// Handle a chatroom
    /// </summary>
    [Serializable]
    public class Chatroom : IComparable<Chatroom>, INotifyPropertyChanged
    {
        string name;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Getter & setter for chatroom's name.
        /// Implement the PropertyChanged to make the binding list work.
        /// </summary>
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

        public bool Equals(Chatroom other)
        {
            if (this.Name == other.Name)
                return true;

            return false;
        }

        /// <summary>
        /// Display a chatroom, useful for the binding list
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }
    }
}
