using chat.exception;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace chat.authentification
{
    class Authentification : AuthentificationManager
    {
        List<User> userList;

        internal List<User> UserList
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

        public Authentification()
        {
            UserList = new List<User>();
        }

        public void addUser(string login, string password)
        {

            foreach (User user in UserList)
            {
                if (user.Login == login)
                {
                    throw new UserExistsException(login);
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

        public AuthentificationManager load(string path)
        {
            Authentification authenfication = new Authentification();

            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    List<User> users = (List<User>)bin.Deserialize(stream);
                    authenfication.userList = users;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            return authenfication;
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
