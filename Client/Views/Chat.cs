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
    /// <summary>
    /// Main WinForm used to see, send and receive messages
    /// We do a massive use of ThreadedBindingList because they support threads and
    /// are a great way to do 2-way binding
    /// </summary>
    public partial class Chat : Form
    {
        private Client client;
        private Thread checkChatrooms;
        private Thread checkUsers;
        private Thread checkServer;
        private ThreadedBindingList<Chatroom> chatroomsBindingList;
        private ThreadedBindingList<User> usersBindingList;
        private ThreadedBindingList<string> messagesBindingList;
        private Chatroom defaultChatroom;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientParam">Client instance</param>
        public Chat(Client clientParam)
        {
            InitializeComponent();
            client = clientParam;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Action performed on "Send" button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendButton_Click(object sender, EventArgs e)
        {
            ChatMessage messageToSend = new ChatMessage(ChatMessage.Header.POST);
            messageToSend.addData(messageTextBox.Text);
            client.sendMessage(messageToSend);
            messageTextBox.Clear();
        }

        /// <summary>
        /// Open the Add chatroom WinForm on click of "Create chatroom" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createChatroomButton_Click(object sender, EventArgs e)
        {
            var frm = new AddChatroom(client);
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            frm.Show();
        }

        /// <summary>
        /// Action performed on chatroom change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chatrooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do nothing if we select the dummy chatroom "Select a chatroom"
            // Meaning: leave the current chatroom, clear the interface
            if (chatrooms.Text == "Select a chatroom")
            {
                chatrooms.SelectedIndex = -1;
                ChatMessage quitCr = new ChatMessage(ChatMessage.Header.QUIT_CR);
                client.sendMessage(quitCr);
                if (messagesBindingList != null && usersBindingList != null)
                {
                    messagesBindingList.Clear();
                    usersBindingList.Clear();
                }
            }

            // Join the chatroom wanted otherwise
            if (chatrooms.Text != "" &&
                chatrooms.Text != "Select a chatroom" &&
                chatrooms.SelectedItem != null)
            {
                client.User.Chatroom = new Chatroom(chatrooms.Text);
                ChatMessage joinCr = new ChatMessage(ChatMessage.Header.JOIN_CR);
                joinCr.addData(chatrooms.Text);
                client.sendMessage(joinCr);
            }
        }

        /// <summary>
        /// Periodically check users (from a thread) connected to the current chatroom
        /// </summary>
        private void getUsers()
        {
            while (!client.Quit)
            {
                try
                {
                    // Now, check the users
                    if (client.User.Chatroom != null && client.User.Chatroom.Name != "")
                    {
                        // We need to invoke chatrooms (UI thread) to see the selected index
                        chatrooms.BeginInvoke(
                            (Action) (() =>
                            {
                                // If we are indeed connected to a chatroom
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

        /// <summary>
        /// Periodically check availables chatrooms (from a thread)
        /// </summary>
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

        /// <summary>
        /// Periodically check if the server is available
        /// </summary>
        private void getServer()
        {
            while (!client.Quit)
            {
                Thread.Sleep(2000);
            }

            if (client.Quit)
            {
                // Close the chat if the server is no longer available
                Console.WriteLine("Close from getServer");
                MessageBox.Show("The server is unreachable, please retry.",
                    "Connection error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                chatrooms.BeginInvoke(
                    (Action)(() =>
                    {
                        this.Close();
                    })
                );
            }
        }

        private void messageTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Allow users to hit "Enter" to send a message instead of clicking "Send"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Action performed after the constructor: create the ThreadedBindingLists and start the 2 threads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chat_Load(object sender, EventArgs e)
        {
            // When we close this form, we want to close it all!
            this.FormClosing += new FormClosingEventHandler(Chat_Closing);

            welcomeLabel.Text = "Welcome " + client.User.Login;

            chatroomsBindingList = new ThreadedBindingList<Chatroom>();
            client.ChatroomsBindingList = chatroomsBindingList;
            defaultChatroom = new Chatroom("Select a chatroom");
            chatroomsBindingList.Add(defaultChatroom);
            chatrooms.DataSource = chatroomsBindingList;

            usersBindingList = new ThreadedBindingList<User>();
            client.UsersBindingList = usersBindingList;
            userlist.DataSource = usersBindingList;

            messagesBindingList = new ThreadedBindingList<string>();
            client.MessagesBindingList = messagesBindingList;
            messages.DataSource = messagesBindingList;

            // Start the thread to check the chatrooms available
            checkChatrooms = new Thread(new ThreadStart(this.getChatrooms));
            checkChatrooms.Start();

            // Start the thread to check the users connected to the current chatroom
            checkUsers = new Thread(new ThreadStart(this.getUsers));
            checkUsers.Start();

            // Start the thread to check server
            checkServer = new Thread(new ThreadStart(this.getServer));
            checkServer.Start();
        }

        /// <summary>
        /// When we close the form, let the client know about it so it will close the thread gracefully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chat_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Client exiting");
            checkServer.Abort();
            client.Quit = true;
            client.Logged = false;
        }
    }
}
