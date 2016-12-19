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
    /// <summary>
    /// Handle all chatrooms
    /// </summary>
    [Serializable]
    public class ChatroomManager
    {
        List<Chatroom> chatroomList;

        /// <summary>
        /// Create a list of all chatrooms
        /// </summary>
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

        /// <summary>
        /// Add a chatroom if it is not already in the list
        /// </summary>
        /// <param name="other">Chatroom to add</param>
        public void addChatroom(Chatroom other)
        {
            foreach (Chatroom chatroom in ChatroomList.ToList())
            {
                if (chatroom.Name == other.Name)
                {
                    throw new ChatroomAlreadyExistsException(chatroom.Name);
                }
            }

            ChatroomList.Add(other);
        }

        /// <summary>
        /// Remove a chatroom, based on its name
        /// </summary>
        /// <param name="name">Chatroom to delete</param>
        public void removeChatroom(string name)
        {
            Chatroom chatroomToDelete = null;

            foreach (Chatroom chatroom in ChatroomList.ToList())
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

        /// <summary>
        /// Load chatrooms stored in a static file.
        /// </summary>
        /// <param name="path">Path to the file</param>
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

        /// <summary>
        /// Save current chatrooms into a file
        /// </summary>
        /// <param name="path">Path to static file</param>
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
