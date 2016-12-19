using System;
using System.Net.Sockets;
using Chat.Chat;

namespace Chat.Auth
{
    /// <summary>
    /// Handle an user
    /// </summary>
    [Serializable()]
    public class User : IComparable<User>
    {
        string login;
        string password;
        Chatroom chatroom;

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

        public Chatroom Chatroom
        {
            get
            {
                return chatroom;
            }

            set
            {
                chatroom = value;
            }
        }

        public User()
        {
            this.Login = "";
            this.Password = "";
        }

        public User(string login)
        {
            this.Login = login;
            this.Password = "";
        }

        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }

        public int CompareTo(User other)
        {
            if (this.Login == other.Login && this.Password == other.Password)
                return 0;

            return -1;
        }

        public override string ToString()
        {
            return login;
        }
    }
}
