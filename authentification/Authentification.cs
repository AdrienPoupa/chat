using chat.exception;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace chat.authentification
{
    class Authentification : AuthentificationManagement
    {
        List<User> userList;

        Authentification()
        {
            userList = new List<User>();
        }

        void addUser(string login, string password)
        {

            foreach (User user in userList)
            {
                if (user.Login == login)
                {
                    throw new UserExitsException(login);
                }
            }

            userList.Add(new User(login, password));
        }

        void removeUser(string login)
        {
            User userToDelete = null;

            foreach(User user in userList)
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

            userList.Remove(userToDelete);
        }

        void authentify(string login, string password)
        {
            User userToAuthentify = null;

            foreach (User user in userList)
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

        static AuthentificationManagement load(string path)
        {
            return new AuthentificationManagement();

            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        void save(string path)
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, userList);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
