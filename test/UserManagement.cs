using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using chat.authentification;
using chat.exception;

namespace chat.test
{
    class UserManagement
    {
        public static void Main()
        {
            AuthentificationManager am = new Authentification();
            // users management
            try
            {
                am.addUser("bob", "123");
                Console.WriteLine("Bob has been added !");
                am.removeUser("bob");
                Console.WriteLine("Bob has been removed !");
                am.removeUser("bob");
                Console.WriteLine("Bob has been removed twice !");
            }
            catch (UserUnknownException e)
            {
                Console.WriteLine(e.login + " : user unknown (unable to remove) !");
            }
            catch (UserExistsException e)
            {
                Console.WriteLine(e.login + " has already been added !");
            }

            // authentication
            try
            {
                am.addUser("bob", "123");
                Console.WriteLine("Bob has been added !");
                am.authentify("bob", "123");
                Console.WriteLine("Authentification OK !");
                am.authentify("bob", "456");
                Console.WriteLine("Invalid password !");
            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine(e.login + " has provided an invalid password !");
            }
            catch (UserExistsException e)
            {
                Console.WriteLine(e.login + " has already been added !");
            }
            catch (UserUnknownException e)
            {
                Console.WriteLine(e.login + " : user unknown (enable to remove) !");
            }

            // persistance

            try
            {
                am.save("users.txt");
                AuthentificationManager am1 = new Authentification();
                am1.load("users.txt");
                am1.authentify("bob", "123");
                Console.WriteLine("Loading complete !");
            }
            catch (UserUnknownException e)
            {
                Console.WriteLine(e.login + " is unknown ! error during the saving/loading.");
            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine(e.login + " has provided an invalid password ! error during the saving/loading .");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown error");
                Console.WriteLine(e);
            }
        }
    }
}
