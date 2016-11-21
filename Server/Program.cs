using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Chat;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.startServer(2300);

            if(server.Running)
            {
                server.UserManager.addUser("bob", "123");
                server.UserManager.addUser("bob2", "123");
                server.ChatroomManager.addChatroom(new Chatroom("Channel basique"));
                server.run();
            }
            
            Console.WriteLine("----- End of execution");
            Console.ReadLine();
        }
    }
}
