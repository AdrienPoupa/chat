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
        UserManager userManager = new UserManager();
        SessionManager sessionManager = new SessionManager();
        ChatroomManager chatroomManager = new ChatroomManager();
        
        public volatile bool readLock = false;

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
                    Console.WriteLine("Attente d'une nouvelle connexion...");
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    Session session = new Session();
                    session.Client = client;
                    SessionManager.addSession(session);

                    Console.WriteLine("Nouveau client : " + session.Token);
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
                                messagePostBroadcast.addData("a quitté le salon de discussion \"" + session.User.Chatroom.Name + "\"");
                                broadcastToChatRoom(session, messagePostBroadcast);
                            }
                            
                            session.Client.Close();
                            sessionManager.removeSession(session.Token);

                            Console.WriteLine("- Utilisateur deconnecte : " + session.Token);
                        }
                        break;

                    case Message.Header.JOIN_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            Chatroom chatroom = new Chatroom(messageList[0]);
                            
                            session.User.Chatroom = ChatroomManager.getChatroom(chatroom);
                            Console.WriteLine("- " + session.User.Login + " a rejoint le salon de discussion : " + messageList[0]);

                            //On prévient le client que le salon a bien été rejoint
                            Message messageSuccess = new Message(Message.Header.JOIN_CR);
                            messageSuccess.addData("success");
                            messageSuccess.addData(messageList[0]);
                            sendMessage(messageSuccess, session.Client.Client);

                            //On envoie au client un message à afficher de la part du serveur
                            Message messagePost = new Message(Message.Header.POST);
                            messagePost.addData("Serveur");
                            messagePost.addData(session.User.Login + " a rejoint le salon de discussion \"" + session.User.Chatroom.Name + "\"");
                            sendMessage(messagePost, session.Client.Client);

                            //On broadcast à tous les participants de la conversations l'arrivée de l'utilisateur
                            Message messagePostBroadcast = new Message(Message.Header.POST);
                            messagePostBroadcast.addData("a rejoint le salon de discussion \"" + session.User.Chatroom.Name + "\"");
                            broadcastToChatRoom(session, messagePostBroadcast);
                        }
                        catch (ChatroomUnknownException e)
                        {
                            //On prévient l'utilisateur qu'il n'a pas été ajouté à la conversation
                            Message messageSuccess = new Message(Message.Header.JOIN_CR);
                            messageSuccess.addData("error");
                            messageSuccess.addData(message.MessageList[0]);
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- " + session.User.Login + " : le salon de discussion existe deja : " + e.Message);
                        }
                        break;

                    case Message.Header.QUIT_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            ChatroomManager.getChatroom(new Chatroom(messageList[0]));

                            if(session.User.Chatroom != null && session.User.Chatroom.Name == messageList[0])
                            {
                                //On prévient l'utilisateur qu'il a quitté la conversation
                                Message messageSuccess = new Message(Message.Header.QUIT_CR);
                                messageSuccess.addData("success");
                                messageSuccess.addData(messageList[0]);
                                sendMessage(messageSuccess, session.Client.Client);

                                //On prévient les autres utilisateurs que celui-ci est parti
                                Message messagePostBroadcast = new Message(Message.Header.POST);
                                messagePostBroadcast.addData("a quitté le salon de discussion \"" + session.User.Chatroom.Name + "\"");
                                broadcastToChatRoom(session, messagePostBroadcast);

                                session.User.Chatroom = null;

                                Console.WriteLine("- " + session.User.Login + " a quitte le salon de discussion : " + messageList[0]);
                            }
                            else
                            {
                                //On prévient l'utilisateur qui n'a pas été éjecté de la conversation car il n'en faisait pas partie
                                Message messageError = new Message(Message.Header.QUIT_CR);
                                messageError.addData("error");
                                messageError.addData(messageList[0]);
                                sendMessage(messageError, session.Client.Client);

                                Console.WriteLine("- " + session.User.Login + " ne fait pas partie de cette salle de discussion : " + messageList[0]);
                            }
                        }
                        catch (ChatroomUnknownException e)
                        {
                            //On prévient l'utilisateur que le salon n'existe pas
                            Message messageError = new Message(Message.Header.QUIT_CR);
                            messageError.addData("error");
                            messageError.addData(message.MessageList[0]);
                            sendMessage(messageError, session.Client.Client);

                            Console.WriteLine("- " + session.User.Login + " : le salon de discussion n'existe pas : " + e.Message);
                        }
                        break;

                    case Message.Header.CREATE_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            ChatroomManager.addChatroom(new Chatroom(messageList[0]));
                            ChatroomManager.save("chatrooms.db");

                            //On prévient l'utilisateur que le salona bien été ajouté
                            Message messageSuccess = new Message(Message.Header.CREATE_CR);
                            messageSuccess.addData("success");
                            messageSuccess.addData(messageList[0]);
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- " + session.User.Login + " : le salon de discussion a bien ete cree : " + messageList[0]);
                        }
                        catch (ChatroomAlreadyExistsException e)
                        {
                            //On prévient l'utilisateur que le salonn'a pas été créé
                            Message messageError = new Message(Message.Header.CREATE_CR);
                            messageError.addData("error");
                            messageError.addData(message.MessageList[0]);
                            sendMessage(messageError, session.Client.Client);

                            Console.WriteLine("- " + session.User.Login + " : le salon de discussion existe deja : " + e.Message);
                        }
                        break;

                    case Message.Header.LIST_CR:
                        Message messageListCr = new Message(Message.Header.LIST_CR);

                        foreach (Chatroom chatroom in ChatroomManager.ChatroomList)
                        {
                            message.addData(chatroom.Name);
                        }

                        sendMessage(messageListCr, session.Client.Client);
                        break;

                    case Message.Header.POST:
                        Console.WriteLine("- " + session.User.Login + " : message recu : " + message.MessageList[0]);
                        broadcastToChatRoom(session, message);
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
                            UserManager.authentify(messageList[0], messageList[1]);
                            session.User = new User(messageList[0], messageList[1]);
                            UserManager.save("users.db");

                            //On prévient l'utilisateur que son compte a bien été enregistré
                            Message messageSuccess = new Message(Message.Header.REGISTER);
                            messageSuccess.addData("success");
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- Enregistrement effectue : " + session.User.Login);
                        }
                        catch (UserAlreadyExistsException e)
                        {
                            //On prévient l'utilisateur que son compte n'a pas été créé
                            Message messageSuccess = new Message(Message.Header.REGISTER);
                            messageSuccess.addData("error");
                            sendMessage(messageSuccess, session.Client.Client);

                            Console.WriteLine("- Enregistrement impossible : " + e.Message);
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

                            Console.WriteLine("- Connexion reussie : " + session.User.Login);
                        }
                        catch (WrongPasswordException e)
                        {
                            Message messageSuccess = new Message(Message.Header.JOIN);
                            messageSuccess.addData("error");
                            sendMessage(messageSuccess, session.Client.Client);
                            
                            Console.WriteLine("- Connextion impossible : " + e.Message);
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
                        Console.WriteLine("- Utilisateur deconnecte : " + SessionManager.SessionList[i].Token);

                        if (!readLock)
                        {
                            if (SessionManager.SessionList[i].User != null && 
                                SessionManager.SessionList[i].User.Chatroom != null)
                            {
                                //On prévient les autres utilisateurs que celui-ci est parti
                                Message messagePostBroadcast = new Message(Message.Header.POST);
                                messagePostBroadcast.addData("a quitté le salon de discussion \"" + 
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

                Console.WriteLine("- Message de " + session.User.Login + " diffuse");
            }
            else
            {
                Console.WriteLine("- L'utilisateur n'est connecte a aucune salle de discussion : " + session.User.Login);
            }
        }
    }
}
