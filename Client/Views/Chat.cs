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
        private Thread checkChatrooms;
        private Thread checkUsers;
        private ThreadedBindingList<Chatroom> chatroomsBindingList;
        private ThreadedBindingList<User> usersBindingList;
        private ThreadedBindingList<string> messagesBindingList;
        public Chat(Client clientParam)
        {
            InitializeComponent();
            client = clientParam;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            ChatMessage messageToSend = new ChatMessage(ChatMessage.Header.POST);
            messageToSend.addData(messageTextBox.Text);
            client.sendMessage(messageToSend);
            messageTextBox.Clear();
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

        private void getUsers()
        {
            while (!client.Quit)
            {
                try
                {
                    // Now, check the users
                    if (client.User.Chatroom != null && client.User.Chatroom.Name != "")
                    {
                        chatrooms.BeginInvoke(
                            (Action)(() =>
                            {
                                if (chatrooms.Text != "")
                                {
                                    ChatMessage messageUsers = new ChatMessage(ChatMessage.Header.LIST_USERS);
                                    messageUsers.addData(chatrooms.Text);

                                    client.sendMessage(messageUsers);
                                }
                            })
                        );

                        Thread.Sleep(2000);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void messageTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void messageTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendButton_Click(sender, e);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Chat_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(Chat_Closing);

            welcomeLabel.Text = "Welcome " + client.User.Login;

            chatroomsBindingList = new ThreadedBindingList<Chatroom>();
            client.ChatroomsBindingList = chatroomsBindingList;
            chatrooms.DataSource = chatroomsBindingList;

            usersBindingList = new ThreadedBindingList<User>();
            client.UsersBindingList = usersBindingList;
            userlist.DataSource = usersBindingList;

            messagesBindingList = new ThreadedBindingList<String>();
            client.MessagesBindingList = messagesBindingList;
            messages.DataSource = messagesBindingList;

            checkChatrooms = new Thread(new ThreadStart(this.getChatrooms));
            checkChatrooms.Start();

            checkUsers = new Thread(new ThreadStart(this.getUsers));
            checkUsers.Start();

            client.User.Chatroom = new Chatroom("");
        }

        private void Chat_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Client exiting");
            client.Quit = true;
            client.Logged = false;
        }
    }
}
