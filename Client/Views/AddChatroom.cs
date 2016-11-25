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
    public partial class AddChatroom : Form
    {
        private Client client;
        public AddChatroom(Client clientParam)
        {
            InitializeComponent();
            client = clientParam;
        }

        private void createChatroomButton_Click(object sender, EventArgs e)
        {
            ChatMessage chatroomsMessage = new ChatMessage(ChatMessage.Header.CREATE_CR);
            chatroomsMessage.addData(chatroomTextBox.Text);

            client.sendMessage(chatroomsMessage);

            ChatMessage reply = client.getMessage();

            if (reply == null)
            {
                MessageBox.Show("Server failure",
                     "Connection error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }

            if (reply.MessageList.First() == "success")
            {
                this.Close();
            }
            else if (reply.MessageList.First() == "error")
            {
                MessageBox.Show(reply.MessageList[1],
                    "Could not create chatroom",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }
    }
}
