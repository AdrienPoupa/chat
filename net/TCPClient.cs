using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace chat.net
{
    class TCPClient : MessageConnection
    {
        private int _port; // todo: get port from socket?
        private Socket socket;
        private IPAddress _adr;
        private IPEndPoint remoteEP;
        //private byte[] bytes;

        public void setServer(IPAddress adr, int port)
        {
            _port = port;
            _adr = adr;
        }

        public void connect()
        {
            remoteEP = new IPEndPoint(_adr, _port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(remoteEP);
        }

        public void close()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        /* Pour l'envoi et la réception de données, on est censé sérialiser l'objet Message
         * Surtout que dans ton code, à la réception, tu mets le header et la liste des messages dans l'attribut data */

        public Message getMessage()
        {
            try
            {
                NetworkStream strm = new NetworkStream(socket);
                IFormatter formatter = new BinaryFormatter();
                return (Message)formatter.Deserialize(strm);
            }
            catch (SerializationException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public void sendMessage(Message message)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                NetworkStream strm = new NetworkStream(socket);
                formatter.Serialize(strm, message);
            }
            catch (SerializationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public int getPort()
        {
            return _port;
        }

        public IPAddress getAddress()
        {
            return _adr;
        }

        /*public Message getMessage()
        {
            // Data buffer for incoming data.
            bytes = new byte[1024];

            // Receive the response from the remote device.
            int bytesRec = socket.Receive(bytes);

            Message msgReceived = new Message(bytes, bytesRec);

            return msgReceived;
        }

        public void sendMessage(Message m)
        {
            // Encode the data string into a byte array.
            byte[] msg = Encoding.ASCII.GetBytes(m.toString());

            // Send the data through the socket.
            socket.Send(msg);
        }*/
    }
}