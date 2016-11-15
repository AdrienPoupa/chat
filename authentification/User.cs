using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.authentification
{
    [Serializable()]
    class User : IComparable<User>
    {
        string login;
        string password;

        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public int CompareTo(User other)
        {
            if (this.Login == other.Login && this.Password == other.Password)
                return 0;

            return -1;
        }
    }
}
