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

namespace Chat.Net
{
    [Serializable]
    public abstract class TCPServer
    {
        protected TcpClient commSocket;
        protected TcpListener tcpListener;
        protected int port;

        public void startServer(int port)
        {
            this.port = port;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            tcpListener = new TcpListener(ipAddress, port);
            tcpListener.Start();
        }

        public void stopServer()
        {
            tcpListener.Stop();
        }

        public Message getMessage(Socket socket)
        {
            Console.WriteLine("## Reception d'un message");

            try
            {
                NetworkStream strm = new NetworkStream(socket);
                IFormatter formatter = new BinaryFormatter();
                Message message = (Message)formatter.Deserialize(strm);
                Console.WriteLine("- header du message : " + message.Head);
                return message;
            }
            catch (SerializationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public void sendMessage(Message message, Socket socket)
        {
            Console.WriteLine("## Envoi d'un message : " + message.Head);

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
    }
}
