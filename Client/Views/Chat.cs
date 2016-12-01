using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat.Auth;
using Chat.Chat;
using Chat.Net;
using ChatMessage = Chat.Net.Message;

namespace Client.Views
{
    public partial class Chat : Form
    {
        private Client client;
        private Thread checkChartooms;
        private Thread checkUsers;
        public Chat(Client clientParam)
        {
            InitializeComponent();
            client = clientParam;

            BindingList<Chatroom> bs = new BindingList<Chatroom>(client.ChatroomManager.ChatroomList);
            chatrooms.DataSource = bs;

            BindingList<User> us = new BindingList<User>(client.UserManager.UserList);
            userlist.DataSource = us;

            checkChartooms = new Thread(new ThreadStart(this.getChatrooms));
            checkChartooms.Start();

            client.User.Chatroom = new Chatroom("");

            /*checkUsers = new Thread(new ThreadStart(this.getUsers));
            checkUsers.Start();*/
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sendButton_Click(object sender, EventArgs e)
        {

        }

        private void createChatroomButton_Click(object sender, EventArgs e)
        {
            var frm = new AddChatroom(client);
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            frm.Show();
        }

        private void chatrooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (client.User.Chatroom.Name != chatrooms.Text && chatrooms.Text != "")
            {
                ChatMessage quitCr = new ChatMessage(ChatMessage.Header.QUIT_CR);
                client.sendMessage(quitCr);

                client.User.Chatroom = new Chatroom(chatrooms.Text);
                ChatMessage joinCr = new ChatMessage(ChatMessage.Header.JOIN_CR);
                joinCr.addData(chatrooms.Text);
                client.sendMessage(joinCr);
            }
        }

        private void getChatrooms()
        {
            while (!client.Quit)
            {
                try
                {
                    ChatMessage messageChatrooms = new ChatMessage(ChatMessage.Header.LIST_CR);
                    client.sendMessage(messageChatrooms);
                    Thread.Sleep(5000);

                    // We cannot directly edit UI elements from a thread. Let's invoke the UI itself.
                    chatrooms.BeginInvoke(
                        (Action)(() =>
                        {
                            chatrooms.DataSource = null;
                            chatrooms.DataSource = client.ChatroomManager.ChatroomList;
                        })
                   );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void getUsers()
        {
            while (!client.Quit)
            {
                try
                {
                    ChatMessage messageUsers = new ChatMessage(ChatMessage.Header.LIST_USERS);

                    if (client.User.Chatroom != null && client.User.Chatroom.Name != "")
                    {
                        messageUsers.addData(client.User.Chatroom.Name);

                        client.sendMessage(messageUsers);
                        Thread.Sleep(5000);

                        // We cannot directly edit UI elements from a thread. Let's invoke the UI itself.
                        userlist.BeginInvoke(
                            (Action)(() =>
                            {
                                userlist.DataSource = null;
                                userlist.DataSource = client.UserManager.UserList;
                            })
                       );
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
