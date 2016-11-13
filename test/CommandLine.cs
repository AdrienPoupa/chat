using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat.chat;

namespace chat.test
{
    class CommandLine
    {
        public static void Main()
        {
            Chatter bob = new TextChatter("Bob");
            Chatter joe = new TextChatter("Joe");
            TopicsManager gt = new TextGestTopics();
            gt.createTopic("java");
            gt.createTopic("UML");
            gt.listTopics();
            gt.createTopic("jeux");
            gt.listTopics();
            Chatroom cr = gt.joinTopic("jeux");
            cr.join(bob);
            cr.post("Je suis seul ou quoi ?", bob);
            cr.join(joe);
            cr.post("Tiens, salut Joe !", bob);
            cr.post("Toi aussi tu chat sur les forums de jeux pendant les TP, Bob ? ",joe);
        }
    }
}
