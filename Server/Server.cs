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

        public bool readLock = false;

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
            Thread checkData = new Thread(new ThreadStart(this.checkData));
            checkData.Start();

            Thread checkQuit = new Thread(new ThreadStart(this.checkQuit));
            checkQuit.Start();

            while (true)
            {
                Console.WriteLine("Attente d'une nouvelle connexion...");
                TcpClient client = this.tcpListener.AcceptTcpClient();

                Session session = new Session();
                session.Client = client;

                SessionManager.addSession(session);

                Console.WriteLine("Nouveau client : " + session.Token);
            }
        }

        private void checkData()
        {
            while (true)
            {
                try
                {
                    readLock = true;

                    if (SessionManager.SessionList.Count > 0)
                    {
                        foreach (Session session in SessionManager.SessionList)
                        {
                            if(session.Client.GetStream().DataAvailable)
                            {
                                Message message = getMessage(session.Client.Client);

                                Thread processData = new Thread(() => this.processData(session, message));
                                processData.Start();
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
                        sendMessage(new Message(Message.Header.QUIT), session.Client.Client);
                        Console.WriteLine("- Utilisateur deconnecte : " + session.Token);

                        session.Client.Close();
                        sessionManager.removeSession(session.Token);
                        break;

                    case Message.Header.JOIN_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            Chatroom chatroom = new Chatroom(messageList[0]);
                            
                            session.User.Chatroom = ChatroomManager.getChatroom(chatroom);
                            Console.WriteLine("- La salle de discussion a bien ete rejointe : " + messageList[0]);

                            Message messagePost = new Message(Message.Header.POST);
                            messagePost.addData("Serveur");
                            messagePost.addData(session.User.Login + " a rejoint le salon de discussion \"" + session.User.Chatroom.Name + "\"");
                            sendMessage(messagePost, session.Client.Client);

                            Message messageJoinCr = new Message(Message.Header.JOIN_CR);
                            messageJoinCr.addData(session.User.Chatroom.Name);
                            sendMessage(messageJoinCr, session.Client.Client);
                        }
                        catch (ChatroomUnknownException e)
                        {
                            Console.WriteLine("- La salle de discussion existe deja : " + e.Message);
                        }
                        break;

                    case Message.Header.QUIT_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            ChatroomManager.getChatroom(new Chatroom(messageList[0]));

                            if(session.User.Chatroom != null && session.User.Chatroom.Name == messageList[0])
                            {
                                session.User.Chatroom = null;

                                Message messageJoinCr = new Message(Message.Header.QUIT_CR);
                                messageJoinCr.addData(messageList[0]);
                                sendMessage(messageJoinCr, session.Client.Client);

                                Console.WriteLine("- La salle de discussion a bien ete quittee : " + messageList[0]);
                            }
                            else
                            {
                                Console.WriteLine("- L'utilisateur ne fait pas partie de cette salle de discussion : " + messageList[0]);
                            }
                        }
                        catch (ChatroomUnknownException e)
                        {
                            Console.WriteLine("- La salle de discussion existe deja : " + e.Message);
                        }
                        break;

                    case Message.Header.CREATE_CR:
                        try
                        {
                            List<string> messageList = message.MessageList;
                            ChatroomManager.addChatroom(new Chatroom(messageList[0]));
                            ChatroomManager.save("chatrooms.db");

                            Message messageJoinCr = new Message(Message.Header.CREATE_CR);
                            messageJoinCr.addData(messageList[0]);
                            sendMessage(messageJoinCr, session.Client.Client);

                            Console.WriteLine("- La salle de discussion a bien ete creee : " + messageList[0]);
                        }
                        catch (ChatroomAlreadyExistsException e)
                        {
                            Console.WriteLine("- La salle de discussion existe deja : " + e.Message);
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

                            Console.WriteLine("- Enregistrement effectue : " + session.User.Login);
                        }
                        catch (UserAlreadyExistsException e)
                        {
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

                            Console.WriteLine("- Connexion reussie : " + session.User.Login);
                        }
                        catch (WrongPasswordException e)
                        {
                            sendMessage(new Message(Message.Header.JOIN), session.Client.Client);
                            Console.WriteLine("- Connextion impossible : " + e.Message);
                        }

                        break;
                }
            }
        }

        private void checkQuit()
        {
            while (true)
            {
                for (int i = 0; i < SessionManager.SessionList.Count; i++)
                {
                    Socket socket = SessionManager.SessionList[i].Client.Client;

                    if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
                    {
                        Console.WriteLine("Utilisateur deconnecte : " + SessionManager.SessionList[i].Token);

                        if (!readLock)
                        {
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

                Console.WriteLine("Message de " + session.User.Login + " diffuse");
            }
            else
            {
                Console.WriteLine("L'utilisateur n'est connecte a aucune salle de discussion : " + session.User.Login);
            }
        }
    }
}
