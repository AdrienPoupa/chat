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
using ChatUser = Chat.Auth.User;

namespace Client.Views
{
    public partial class UserLogin : Form
    {
        protected Client client;

        public UserLogin(Client clientParam)
        {
            InitializeComponent();
            client = clientParam;
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            ChatMessage messageJoin = new ChatMessage(ChatMessage.Header.JOIN);
            messageJoin.addData(usernameTextBox.Text);
            messageJoin.addData(passwordTextBox.Text);
            client.sendMessage(messageJoin);

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
                client.User = new ChatUser(usernameTextBox.Text, passwordTextBox.Text);
                var frm = new Chat(client);
                frm.Location = this.Location;
                frm.StartPosition = FormStartPosition.Manual;
                frm.FormClosing += delegate { this.Show(); };
                frm.Show();
                this.Hide();
            }
            else if (reply.MessageList.First() == "error")
            {
                MessageBox.Show("Wrong password",
                     "Connection error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChatMessage messageRegister = new ChatMessage(ChatMessage.Header.REGISTER);
            messageRegister.addData(usernameTextBox.Text);
            messageRegister.addData(passwordTextBox.Text);
            client.sendMessage(messageRegister);

            ChatMessage register = client.getMessage();

            if (register == null)
            {
                MessageBox.Show("Server failure",
                     "Connection error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }

            if (register.MessageList.First() == "success")
            {
                MessageBox.Show("Registration success",
                    "Registration success. You can now login using your credentials.",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            else if (register.MessageList.First() == "error")
            {
                MessageBox.Show("Could not register",
                     "Connection error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        private void passwordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton_Click(sender, e);
            }
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
