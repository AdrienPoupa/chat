using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Chat.Exceptions;

namespace Chat.Auth
{
    /// <summary>
    /// Handle users in a manager
    /// </summary>
    public class UserManager
    {
        List<User> userList;

        /// <summary>
        /// Store users in a list
        /// </summary>
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

        /// <summary>
        /// Add an user to the list. Make sure it does not already exist, using his login.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void addUser(string login, string password)
        {
            foreach (User user in UserList.ToList())
            {
                if (user.Login == login)
                {
                    throw new UserAlreadyExistsException(login);
                }
            }

            UserList.Add(new User(login, password));
        }

        /// <summary>
        /// Remove the user from the list based on his login
        /// </summary>
        /// <param name="login"></param>
        public void removeUser(string login)
        {
            User userToDelete = null;

            foreach(User user in UserList.ToList())
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

        /// <summary>
        /// Find an user in the list
        /// </summary>
        /// <param name="other">User to look for</param>
        /// <returns>User found</returns>
        public User getUser(User other)
        {
            User getUser = null;

            foreach (User user in UserList.ToList())
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

        /// <summary>
        /// Auth the user based on his password.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void authentify(string login, string password)
        {
            User userToAuthentify = null;

            foreach (User user in UserList.ToList())
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

        /// <summary>
        /// Load the users from a static file
        /// </summary>
        /// <param name="path">Path to the file</param>
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

        /// <summary>
        /// Save the current list to a static file.
        /// </summary>
        /// <param name="path">Path to the file</param>
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
