using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chat.net
{
    abstract class TCPServer : MessageConnection
    {
        private Socket commSocket;
        private Socket waitSocket; // todo: serversocket in the subject
        private int _port; // todo: get port from socket?
        private enum Mode { treatClient, treatConnections }
        private Mode mode;
        private byte[] bytes;

        public void startServer(int port)
        {
            mode = Mode.treatConnections;
            _port = port;
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[1];

            waitSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            waitSocket.Bind(new IPEndPoint(ipAddress, _port));
            waitSocket.Listen(2000); // 2000 connections allowed

            Thread t = new Thread(new ThreadStart(this.run));
            t.Start();
        }

        public void stopServer()
        {
            waitSocket.Shutdown(SocketShutdown.Both);
            waitSocket.Close();
        }

        public void run()
        {
            if (mode == Mode.treatConnections)
            {
                while (true)
                {
                    try
                    {
                        commSocket = waitSocket.Accept();
                        TCPServer myClone = this.DeepClone(); // todo: tester clone
                        myClone.mode = Mode.treatClient;
                        new Thread(new ThreadStart(myClone.run));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                gereClient(commSocket);
            }
        }

        public int getPort()
        {
            return _port;
        }

        /* Pour l'envoi et la réception de données, on est censé sérialiser l'objet Message
         * Surtout que dans ton code, à la réception, tu mets le header et la liste des messages dans l'attribut data */

        public Message getMessage()
        {
            try
            {
                NetworkStream strm = new NetworkStream(commSocket);
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
                NetworkStream strm = new NetworkStream(commSocket);
                formatter.Serialize(strm, message);
            }
            catch(SerializationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /*public Message getMessage()
        {
            // Data buffer for incoming data.
            bytes = new byte[1024];

            // Receive the response from the remote device.
            int bytesRec = commSocket.Receive(bytes);

            Message msgReceived = new Message(bytes, bytesRec);

            return msgReceived;
        }

        public void sendMessage(Message m)
        {
            // Encode the data string into a byte array.
            byte[] msg = Encoding.ASCII.GetBytes(m.toString());

            // Send the data through the socket.
            commSocket.Send(msg);
        }*/

        public abstract void gereClient(Socket comm);
    }
}
