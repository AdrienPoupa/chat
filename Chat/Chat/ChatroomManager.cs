using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Exceptions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chat.Chat
{
    [Serializable]
    public class ChatroomManager
    {
        List<Chatroom> chatroomList;

        public List<Chatroom> ChatroomList
        {
            get
            {
                return chatroomList;
            }

            set
            {
                chatroomList = value;
            }
        }

        public ChatroomManager()
        {
            ChatroomList = new List<Chatroom>();
        }

        public void addChatroom(Chatroom other)
        {

            foreach (Chatroom chatroom in ChatroomList)
            {
                if (chatroom.Name == other.Name)
                {
                    throw new ChatroomAlreadyExistsException(chatroom.Name);
                }
            }

            ChatroomList.Add(other);
        }

        public void removeChatroom(string name)
        {
            Chatroom chatroomToDelete = null;

            foreach (Chatroom chatroom in ChatroomList)
            {
                if (chatroom.Name == name)
                {
                   chatroomToDelete = chatroom;
                }
            }

            if (chatroomToDelete == null)
            {
                throw new ChatroomUnknownException(name);
            }

            ChatroomList.Remove(chatroomToDelete);
        }

        public Chatroom getChatroom(Chatroom other)
        {
            Chatroom getChatroom = null;

            foreach (Chatroom chatroom in ChatroomList)
            {
                if (chatroom.Name == other.Name)
                {
                    getChatroom = chatroom;
                }
            }

            if (getChatroom == null)
            {
                throw new ChatroomUnknownException(other.Name);
            }

            return getChatroom;
        }

        public void load(string path)
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    List<Chatroom> chatrooms = (List<Chatroom>)bin.Deserialize(stream);
                    chatroomList = chatrooms;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void save(string path)
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, ChatroomList);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
