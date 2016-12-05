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
            if (client.User.Chatroom != null && 
                client.User.Chatroom.Name != chatrooms.Text && 
                chatrooms.Text != "")
            {
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
                    Thread.Sleep(2000);

                    // We cannot directly edit UI elements from a thread. Let's invoke the UI itself.
                    chatrooms.BeginInvoke(
                        (Action)(() =>
                        {
                            this.chatrooms.SelectedIndexChanged -= new EventHandler(chatrooms_SelectedIndexChanged);
                            chatrooms.DataSource = null;
                            chatrooms.DataSource = client.ChatroomManager.ChatroomList;
                            this.chatrooms.SelectedIndexChanged += new EventHandler(chatrooms_SelectedIndexChanged);
                        })
                   );

                    // Now, check the users
                    if (client.User.Chatroom != null && client.User.Chatroom.Name != "")
                    {
                        chatrooms.BeginInvoke(
                            (Action)(() =>
                            {
                                ChatMessage messageUsers = new ChatMessage(ChatMessage.Header.LIST_USERS);
                                messageUsers.addData(chatrooms.Text);

                                client.sendMessage(messageUsers);
                            })
                        );
                        
                        Thread.Sleep(2000);

                        // We cannot directly edit UI elements from a thread. Let's invoke the UI itself.
                        userlist.BeginInvoke(
                            (Action)(() =>
                            {
                                userlist.SelectionMode = SelectionMode.MultiExtended;
                                userlist.DataSource = null;
                                userlist.DataSource = client.UserManager.UserList;
                                userlist.SelectionMode = SelectionMode.None;
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
