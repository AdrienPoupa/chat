using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat.exception;

namespace chat.chat
{
    class TextGestTopics : TopicsManager
    {
        private Dictionary<string, Chatroom> chatrooms = new Dictionary<string, Chatroom>();

        public string createTopic(string topic)
        {
		if (!chatrooms.ContainsKey(topic)) {
                chatrooms.Add(topic, new TextChatroom(topic));
            } else {
                throw new ChatroomAlreadyExistsException(topic);
            }

            return topic;
        }

        public Chatroom joinTopic(string topic)
        {
            Chatroom chatroom;

            chatrooms.TryGetValue(topic, out chatroom);

            return chatroom;
        }

        public List<string> listTopics()
        {
            List<string> vect = new List<string>();

            foreach (KeyValuePair<string, Chatroom> entry in chatrooms)
            {
                vect.Add(entry.Key);
            }

            return vect;
        }
    }
}
