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
using Chat.Chat;
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
            
            checkChartooms = new Thread(new ThreadStart(this.getChatrooms));
            checkChartooms.Start();
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
    }
}
