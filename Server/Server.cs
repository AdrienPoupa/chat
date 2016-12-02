using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Net;
using Chat.Auth;
using Chat.Exceptions;
using Chat.Chat;
using System.Threading;
using System.Net.Sockets;

namespace Server
{
    class Server : TCPServer
    {
        UserManager userManager;
        SessionManager sessionManager;
        ChatroomManager chatroomManager;
        
        public volatile bool readLock = false;

        public Server()
        {
            userManager = new UserManager();
            userManager.load("users.db");
            sessionManager = new SessionManager();
            chatroomManager = new ChatroomManager();
            chatroomManager.load("chatrooms.db");
        }

        public UserManager UserManager
        {
            get
            {
                return userManager;
            }

            set
            {
                userManager = value;
            }
        }

        public SessionManager SessionManager
        {
            get
            {
                return sessionManager;
            }

            set
            {
                sessionManager = value;
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

        public void run()
        {
            checkDataThread = new Thread(new ThreadStart(this.checkData));
            checkDataThread.Start();

            checkQuitThread = new Thread(new ThreadStart(this.checkQuit));
            checkQuitThread.Start();

            listenerThread = new Thread(new ThreadStart(this.listen));
            listenerThread.Start();
        }

        private void listen()
        {
            while (this.Running)
            {
                try
                {
                    Console.WriteLine("Waiting for a new connection...");
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    Session session = new Session();
                    session.Client = client;
                    SessionManager.addSession(session);

                    Console.WriteLine("New client: " + session.Token);
                }
                catch (SocketException)
                {
                    // Here we catch a WSACancelBlockingCall exception because this.tcpListener is probably closed
                    Console.WriteLine("Listener thread closed");
                }
            }
        }

        private void checkData()
        {
            while (this.Running)
            {
                try
                {
                    readLock = true;

                    if (SessionManager.SessionList.Count > 0)
                    {
                        foreach (Session session in SessionManager.SessionList)
                        {
                            if(session != null && session.Client.GetStream().DataAvailable)
                            {
                                Thread.Sleep(10);
                                Message message = getMessage(session.Client.Client);

                                if(message != null)
                                {
                                    Thread processData = new Thread(() => this.processData(session, message));
                                    processData.Start();
                                }
                            }
                        }
                    }

                    readLock = false;
                }
                catch(InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
                
                Thread.Sleep(5);
            }
        }

        private void processData(Session session, Message message)
        {
            if (session.User != null)
            {
                switch (message.Head)
                {
                    case Message.Header.QUIT:
                        {
                            //On prévient l'utilisateur qu'il a été déconnecté
                            Message messageSuccess = new Message(Message.Header.QUIT);
                            messageSuccess.addData("success");
                            sendMessage(messageSuccess, session.Client.Client);

                            if(session.User.Chatroom != null)
                            {
                                //On prévient les autres utilisateurs que celui-ci est parti
                                Message messagePostBroadcast = new Message(Message.Header.POST);
                                messagePostBroadcast.addData("left the chatroom \"" + session.User.Chatroom.Name + "\"");
                                broadcastToChatRoom(session, messagePostBroadcast);
                            }
                            
                            session.Client.Close();
                            sessionManager.removeSession(session.Token);

                            Console.WriteLine("- User logout: " + session.Token);
                        }
                        break;

                    case Message.Header.JOIN_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            if (chatroomManager.ChatroomList.Any(x => x.Name == messageList[0]))
                            {
                                session.User.Chatroom = new Chatroom(messageList[0]);
                                Console.WriteLine("- " + session.User.Login + " joined the chatroom: " + messageList[0]);

                                //On prévient le client que le salon a bien été rejoint
                                Message messageSuccess = new Message(Message.Header.JOIN_CR);
                                messageSuccess.addData("success");
                                messageSuccess.addData(messageList[0]);
                                sendMessage(messageSuccess, session.Client.Client);

                                //On envoie au client un message à afficher de la part du serveur
                                Message messagePost = new Message(Message.Header.POST);
                                messagePost.addData("Serveur");
                                messagePost.addData(session.User.Login + " joined the chatroom \"" + messageList[0] + "\"");
                                sendMessage(messagePost, session.Client.Client);

                                //On broadcast à tous les participants de la conversations l'arrivée de l'utilisateur
                                Message messagePostBroadcast = new Message(Message.Header.POST);
                                messagePostBroadcast.addData("joined the chatroom \"" + messageList[0] + "\"");
                                broadcastToChatRoom(session, messagePostBroadcast);
                            }
                        }
                        catch (ChatroomUnknownException e)
                        {
                            //On prévient l'utilisateur qu'il n'a pas été ajouté à la conversation
                            Message messageSuccess = new Message(Message.Header.JOIN_CR);
                            messageSuccess.addData("error");
                            messageSuccess.addData(message.MessageList[0]);
                            sendMessage(messageSuccess, session.Client.Client);
                            messageSuccess.addData("Chatroom " + e.Message + " does not exist");
                        }
                        break;

                    case Message.Header.QUIT_CR:
                        try
                        {
                            if(session.User.Chatroom != null)
                            {
                                //On prévient l'utilisateur qu'il a quitté la conversation
                                Message messageSuccess = new Message(Message.Header.QUIT_CR);
                                messageSuccess.addData("success");
                                messageSuccess.addData(session.User.Chatroom.Name);
                                sendMessage(messageSuccess, session.Client.Client);

                                //On prévient les autres utilisateurs que celui-ci est parti
                                Message messagePostBroadcast = new Message(Message.Header.POST);
                                messagePostBroadcast.addData("left the chatroom \"" + session.User.Chatroom.Name + "\"");
                                broadcastToChatRoom(session, messagePostBroadcast);

                                Console.WriteLine("- " + session.User.Login + " left the chatroom: " + session.User.Chatroom.Name);

                                session.User.Chatroom = null;
                            }
                        }
                        catch (ChatroomUnknownException e)
                        {
                            //On prévient l'utilisateur que le salon n'existe pas
                            Message messageError = new Message(Message.Header.QUIT_CR);
                            messageError.addData("error");
                            messageError.addData(message.MessageList[0]);
                            sendMessage(messageError, session.Client.Client);

                            messageError.addData("Chatroom " + e.Message + " does not exist");
                        }
                        break;

                    case Message.Header.CREATE_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            ChatroomManager.addChatroom(new Chatroom(messageList[0]));
                            ChatroomManager.save("chatrooms.db");

                            //On prévient l'utilisateur que le salon a bien été ajouté
                            Message messageSuccess = new Message(Message.Header.CREATE_CR);
                            messageSuccess.addData("success");
                            messageSuccess.addData(messageList[0]);
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- " + session.User.Login + " : chatroom has been created: " + messageList[0]);
                        }
                        catch (ChatroomAlreadyExistsException e)
                        {
                            //On prévient l'utilisateur que le salon n'a pas été créé
                            Message messageError = new Message(Message.Header.CREATE_CR);
                            messageError.addData("error");
                            messageError.addData("Chatroom " + e.Message + " already exists");
                            sendMessage(messageError, session.Client.Client);
                        }
                        break;

                    case Message.Header.LIST_CR:
                        Message messageListCr = new Message(Message.Header.LIST_CR);

                        foreach (Chatroom chatroom in ChatroomManager.ChatroomList)
                        {
                            messageListCr.addData(chatroom.Name);
                        }

                        sendMessage(messageListCr, session.Client.Client);
                        break;

                    case Message.Header.POST:
                        Console.WriteLine("- " + session.User.Login + " : message received : " + message.MessageList[0]);
                        broadcastToChatRoom(session, message);
                        break;

                    case Message.Header.LIST_USERS:
                        List<string> chatroomWantedList = message.MessageList;
                        string chatroomWanted = chatroomWantedList[0];

                        Message messageListUsers = new Message(Message.Header.LIST_USERS);
                        
                        // For all users currently connected
                        foreach (Session localSession in SessionManager.SessionList)
                        {
                            // If the user is in the chatroom we want the userlist
                            if (localSession.User.Chatroom != null && 
                                localSession.User.Chatroom.Name == chatroomWanted)
                            {
                                messageListUsers.addData(localSession.User.Login);
                            }
                        }

                        sendMessage(messageListUsers, session.Client.Client);

                        break;
                }
            }
            else
            {
                switch (message.Head)
                {
                    case Message.Header.REGISTER:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            UserManager.addUser(messageList[0], messageList[1]);
                            UserManager.save("users.db");

                            //On prévient l'utilisateur que son compte a bien été enregistré
                            Message messageSuccess = new Message(Message.Header.REGISTER);
                            messageSuccess.addData("success");
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- Registration success : " + messageList[0]);
                        }
                        catch (UserAlreadyExistsException e)
                        {
                            //On prévient l'utilisateur que son compte n'a pas été créé
                            Message messageSuccess = new Message(Message.Header.REGISTER);
                            messageSuccess.addData("error");
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- Registration failed : " + e.Message);
                        }
                        break;

                    case Message.Header.JOIN:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            UserManager.authentify(messageList[0], messageList[1]);
                            session.User = new User(messageList[0], messageList[1]);
                            UserManager.save("users.db");

                            Message messageSuccess = new Message(Message.Header.JOIN);
                            messageSuccess.addData("success");
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- Login success : " + session.User.Login);
                        }
                        catch (WrongPasswordException e)
                        {
                            Message messageSuccess = new Message(Message.Header.JOIN);
                            messageSuccess.addData("error");
                            sendMessage(messageSuccess, session.Client.Client);
                            
                            Console.WriteLine("- Login failed : " + e.Message);
                        }

                        break;
                }
            }
        }

        private void checkQuit()
        {
            while (this.Running)
            {
                for (int i = 0; i < SessionManager.SessionList.Count; i++)
                {
                    Socket socket = SessionManager.SessionList[i].Client.Client;

                    if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
                    {
                        Console.WriteLine("- User logged out : " + SessionManager.SessionList[i].Token);

                        if (!readLock)
                        {
                            if (SessionManager.SessionList[i].User != null && 
                                SessionManager.SessionList[i].User.Chatroom != null)
                            {
                                //On prévient les autres utilisateurs que celui-ci est parti
                                Message messagePostBroadcast = new Message(Message.Header.POST);
                                messagePostBroadcast.addData("left the chatroom \"" + 
                                    SessionManager.SessionList[i].User.Chatroom.Name + "\"");
                                broadcastToChatRoom(SessionManager.SessionList[i], messagePostBroadcast);
                            }

                            SessionManager.SessionList[i].Client.Close();
                            sessionManager.removeSession(SessionManager.SessionList[i].Token);
                        }
                    }
                }

                Thread.Sleep(5);
            }
        }

        private void broadcastToChatRoom(Session session, Message message)
        {
            Chatroom chatroom = session.User.Chatroom;

            if(chatroom != null)
            {
                Message messageJoin = new Message(Message.Header.POST);
                messageJoin.addData(session.User.Login);
                messageJoin.addData(message.MessageList[0]);

                foreach(Session sessionUser in SessionManager.SessionList)
                {
                    if(sessionUser.User.Chatroom != null && sessionUser.User.Chatroom == chatroom && sessionUser.User != session.User)
                    {
                        sendMessage(messageJoin, sessionUser.Client.Client);
                    }
                }

                Console.WriteLine("- " + session.User.Login + "'s message broadcast");
            }
            else
            {
                Console.WriteLine("- User is not connected to any chatroom: " + session.User.Login);
            }
        }
    }
}
