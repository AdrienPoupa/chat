using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using Chat.Net;
using Client.Views;

namespace Client
{
    class Program
    {
        /// <summary>
        /// Client entry point.
        /// Calls the first WinForm, which then calls Client and so on
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new Welcome());
        }
    }
}
