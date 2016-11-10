using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.authentification
{
    interface AuthentificationManager
    {
        void addUser(String login, String password);
        void removeUser(String login);
        void authentify(String login, String password);
        AuthentificationManagement load(String path);
        void save(String path);
    }
}
