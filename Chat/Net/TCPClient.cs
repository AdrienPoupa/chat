using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Net
{
    /// <summary>
    /// Each client has a TCPClient instance, which contains a TcpClient object
    /// as well as the server's IPAddress and port, and a boolean used to know when to quit
    /// </summary>
    [Serializable]
    public abstract class TCPClient
    {
        protected int port;
        private Boolean quit;
        protected TcpClient tcpClient;
        protected IPAddress ipAddress;

        public int Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        public IPAddress IpAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
            }
        }

        public bool Quit
        {
            get
            {
                return quit;
            }

            set
            {
                quit = value;
            }
        }

        public TCPClient()
        {
            tcpClient = null;
            IpAddress = null;
        }

        public void setServer(IPAddress ipAddress, int port)
        {
            this.Port = port;
            this.IpAddress = ipAddress;
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void connect()
        {
            try
            {
                tcpClient = new TcpClient(ipAddress.ToString(), Port);
            }
            catch(SocketException e)
            {
                Quit = true;
                Console.WriteLine("Connection refused by the server: " + e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get a message from the server.
        /// Deserialize the object message received and return it
        /// </summary>
        /// <returns>Message object received</returns>
        public Message getMessage()
        {
            if (!Quit)
            {
                try
                {
                    NetworkStream strm = tcpClient.GetStream();
                    IFormatter formatter = new BinaryFormatter();
                    Message message = (Message)formatter.Deserialize(strm);
                    Console.WriteLine("## TCPClient Receiving a message: " + message.Head);
                    return message;
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("TCPClient sendMessage exception: " + e.Message);
                    Quit = true;
                }
                catch (IOException e)
                {
                    Console.WriteLine("TCPClient sendMessage exception: " + e.Message);
                    Quit = true;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("TCPClient sendMessage exception: " + e.Message);
                    Quit = true;
                }
            }

            return null;
        }

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message">Message to send</param>
        public void sendMessage(Message message)
        {
            if (!Quit)
            {
                Console.WriteLine("## TCPClient Sending a message: " + message.Head);

                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    NetworkStream strm = tcpClient.GetStream();
                    formatter.Serialize(strm, message);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("TCPClient sendMessage exception: " + e.Message);
                    Quit = true;
                }
                catch (IOException e)
                {
                    Console.WriteLine("TCPClient sendMessage exception: " + e.Message);
                    Quit = true;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("TCPClient sendMessage exception: " + e.Message);
                    Quit = true;
                }
            }
        }
    }
}