using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace chat.chat
{
    interface Chatter
    {
        void receiveAMessage(string msg, Chatter c);

        string getAlias();

        void joinNotification(Chatter c);

        void quitNotification(Chatter c);
    }
}
