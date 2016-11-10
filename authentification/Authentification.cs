using System;
using System.Collections.Generic;
using System.Linq;
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

            if(userToDelete != null)
            {
                userList.Remove(userToDelete);
            }
        }

        void authentify(string login, string password)
        {

        }

        AuthentificationManagement load(string path)
        {
            return new AuthentificationManagement();
        }

        void save(String path)
        {

        }
    }
}
