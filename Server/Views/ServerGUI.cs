using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat.Chat;

namespace Server.Views
{
    /// <summary>
    /// Server's WinForm
    /// </summary>
    public partial class ServerGUI : Form
    {
        // That's our custom TextWriter class
        TextWriter _writer = null;
        private Server server;

        public ServerGUI()
        {
            InitializeComponent();
        }

        private void ServerGUI_Load(object sender, EventArgs e)
        {
            // Instantiate the writer
            _writer = new TextBoxStreamWriter(txtConsole);
            // Redirect the out Console stream
            Console.SetOut(_writer);
        }

        /// <summary>
        /// Handle the "Start/Stop server" bitton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start the server")
            {
                button1.Text = "Stop the server";
                server = new Server();
                server.startServer(Int32.Parse(textBox1.Text)); // Start the server

                if (server.Running)
                {
                    server.run();
                }
                else
                {
                    MessageBox.Show("Server failure",
                     "Connection error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                }
            }
            else
            {
                button1.Text = "Start the server";
                server.stopServer(); // Stop the server
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
