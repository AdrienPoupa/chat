using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatMessage = Chat.Net.Message;

namespace Client.Views
{
    public partial class Chat : Form
    {
        private Client client;
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

        }

        private void createChatroomButton_Click(object sender, EventArgs e)
        {
            /*ChatMessage chatroomsMessage = new ChatMessage(ChatMessage.Header.CREATE_CR);

            client.sendMessage(chatroomsMessage);

            ChatMessage reply = client.getMessage();*/

            var frm = new AddChatroom(client);
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            frm.Show();
            //this.Hide();
        }
    }
}
