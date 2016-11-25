using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Net;
using Chat.Auth;
using Chat.Chat;
using System.Threading;
using System.Net.Sockets;

namespace Client
{
    public class Client : TCPClient
    {
        User user;
        ChatroomManager chatroomManager;
        Boolean logged;

        public User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public ChatroomManager ChatroomManager
        {
            get
            {
                return chatroomManager;
            }

            set
            {
                chatroomManager = value;
            }
        }

        public bool Logged
        {
            get
            {
                return logged;
            }

            set
            {
                logged = value;
            }
        }

        public Client()
        {
            User = new User();
            ChatroomManager = new ChatroomManager();
            Quit = false;
            Logged = false;
        }

        public void post(string message)
        {
            if(!Quit)
            {
                Message messagePost = new Message(Message.Header.POST);
                messagePost.addData(message);
                sendMessage(messagePost);
            }
        }

        public void run()
        {
            Thread checkConnection = new Thread(new ThreadStart(this.checkData));
            checkConnection.Start();

            Thread checkQuit = new Thread(new ThreadStart(this.checkQuit));
            checkQuit.Start();
        }

        public void checkData()
        {
            while(!Quit)
            {
                try
                {
                    if (tcpClient.GetStream().DataAvailable)
                    {
                        Thread.Sleep(10);
                        Message message = getMessage();

                        if (message != null)
                        {
                            Thread processData = new Thread(() => this.processData(message));
                            processData.Start();
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(5);
            }
        }

        private void checkQuit()
        {
            while (!Quit)
            {
                Socket socket = tcpClient.Client;

                if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
                {
                    Quit = true;
                    Console.WriteLine("- Client checkQuit: Server disconnected");
                }

                Thread.Sleep(5);
            }
        }

        public void processData(Message message)
        {
            switch(message.Head)
            {
                case Message.Header.REGISTER:
                    if(message.MessageList[0] == "success")
                    {
                        Console.WriteLine("- Registration success: " + User.Login);
                    }
                    else
                    {
                        Console.WriteLine("- Registration failed: " + User.Login);
                    }
                    break;

                case Message.Header.JOIN:
                    if (message.MessageList[0] == "success")
                    {
                        Logged = true;
                        Console.WriteLine("- Connection success: " + User.Login);
                    }
                    else
                    {
                        Logged = false;
                        Console.WriteLine("- Connection failed: " + User.Login);
                    }
                    break;

                case Message.Header.QUIT:
                    Quit = true;
                    Logged = false;
                    Console.WriteLine("Message.Header.QUIT : Server disconnected");
                    break;

                case Message.Header.JOIN_CR:
                    if (message.MessageList[0] == "success")
                    {
                        User.Chatroom = new Chatroom(message.MessageList[0]);
                        Console.WriteLine("- Joined chatroom: " + message.MessageList[1]);
                    }
                    else
                    {
                        Console.WriteLine("- Could not join chatroom: " + message.MessageList[1]);
                    }
                    break;

                case Message.Header.QUIT_CR:
                    if (message.MessageList[0] == "success")
                    {
                        User.Chatroom = null;
                        Console.WriteLine("- Chatroom left: " + message.MessageList[1]);
                    }
                    else
                    {
                        Console.WriteLine("- Could not leave chatroom : " + message.MessageList[1]);
                    }
                    break;

                case Message.Header.CREATE_CR:
                    if (message.MessageList[0] == "success")
                    {
                        sendMessage(new Message(Message.Header.LIST_CR));
                        Console.WriteLine("- Chatroom created: " + message.MessageList[1]);
                    }
                    else
                    {
                        Console.WriteLine("- Could not create chatroom: " + message.MessageList[1]);
                    }
                    break;

                case Message.Header.LIST_CR:
                    ChatroomManager.ChatroomList.Clear();

                    foreach(string chatroom in message.MessageList)
                    {
                        chatroomManager.addChatroom(new Chatroom(chatroom));
                        Console.WriteLine(chatroom);
                    }
                    break;

                case Message.Header.POST:
                    Console.WriteLine("- Message received: (" + message.MessageList[0] + ") " + message.MessageList[1]);
                    break;
            }
        }
    }
}
