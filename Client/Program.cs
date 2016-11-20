using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Chat.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            client.User.Login = "bob";
            client.User.Password = "123";

            client.setServer(IPAddress.Parse("127.0.0.1"), 2300);
            client.connect();

            if(!client.Quit)
            {
                client.run();

                Message messageJoin = new Message(Message.Header.JOIN);
                messageJoin.addData(client.User.Login);
                messageJoin.addData(client.User.Password);
                client.sendMessage(messageJoin);

                Message messageCreateCr = new Message(Message.Header.CREATE_CR);
                messageCreateCr.addData("Channel basique 2");
                client.sendMessage(messageCreateCr);

                Message messageJoinCr = new Message(Message.Header.JOIN_CR);
                messageJoinCr.addData("Channel basique");
                client.sendMessage(messageJoinCr);

                /*Message messageQuitCr = new Message(Message.Header.QUIT_CR);
                messageQuitCr.addData("Channel basique");
                client.sendMessage(messageQuitCr);

                Message messageQuit = new Message(Message.Header.QUIT);
                client.sendMessage(messageQuit);*/

                while(!client.Quit)
                {
                    Console.Write("Message a envoyer : ");
                    string message = Console.ReadLine();
                    client.post(message);
                }
            }

            Console.WriteLine("----- End of execution");
            Console.ReadLine();
        }
    }
}
