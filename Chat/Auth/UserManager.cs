using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Chat.Exceptions;

namespace Chat.Auth
{
    public class UserManager
    {
        List<User> userList;

        public List<User> UserList
        {
            get
            {
                return userList;
            }

            set
            {
                userList = value;
            }
        }

        public UserManager()
        {
            UserList = new List<User>();
        }

        public void addUser(string login, string password)
        {
            foreach (User user in UserList)
            {
                if (user.Login == login)
                {
                    throw new UserAlreadyExistsException(login);
                }
            }

            UserList.Add(new User(login, password));
        }

        public void removeUser(string login)
        {
            User userToDelete = null;

            foreach(User user in UserList)
            {
                if(user.Login == login)
                {
                    userToDelete = user;
                }
            }

            if(userToDelete == null)
            {
                throw new UserUnknownException(login);
            }

            UserList.Remove(userToDelete);
        }

        public User getUser(User other)
        {
            User getUser = null;

            foreach (User user in UserList)
            {
                if (user.Login == other.Login && user.Password == other.Password)
                {
                    getUser = user;
                }
            }

            if (getUser == null)
            {
                throw new UserUnknownException(other.Login);
            }

            return getUser;
        }

        public void authentify(string login, string password)
        {
            User userToAuthentify = null;

            foreach (User user in UserList)
            {
                if(user.Login == login && user.Password == password)
                {
                    userToAuthentify = user;
                }
            }

            if(userToAuthentify == null)
            {
                throw new WrongPasswordException(login);
            }
        }

        public void load(string path)
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    List<User> users = (List<User>)bin.Deserialize(stream);
                    userList = users;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void save(string path)
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, UserList);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
