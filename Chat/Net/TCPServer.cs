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
        protected volatile TcpClient commSocket;
        protected volatile TcpListener tcpListener;
        protected volatile Boolean running;
        protected int port;
        protected Thread checkDataThread;
        protected Thread checkQuitThread;
        protected Thread listenerThread;

        public bool Running
        {
            get
            {
                return running;
            }

            set
            {
                running = value;
            }
        }

        public void startServer(int port)
        {
            this.port = port;
            this.Running = false;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            try
            {
                tcpListener = new TcpListener(ipAddress, port);
                tcpListener.Start();
                this.Running = true;
            }
            catch(SocketException e)
            {
                Console.WriteLine("Connexion impossible : " + e.Message);
            }
            
        }

        public void stopServer()
        {
            Console.WriteLine("Stopping the server");
            this.Running = false;
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
            catch (DecoderFallbackException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(InvalidCastException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(OutOfMemoryException e)
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
