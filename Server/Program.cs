using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat.Chat;
using Server.Views;

namespace Server
{
    /// <summary>
    /// Server entry point, calling the WinForm which calls the Server class
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new ServerGUI());
        }
    }
}