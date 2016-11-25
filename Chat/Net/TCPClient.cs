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

        public void connect()
        {
            try
            {
                tcpClient = new TcpClient("127.0.0.1", Port);
            }
            catch(SocketException e)
            {
                Quit = true;
                Console.WriteLine("Connection refused by the server: " + e.Message);
                throw new Exception(e.Message);
            }
        }

        public void close()
        {
            throw new NotImplementedException();
        }

        public Message getMessage()
        {
            try
            {
                NetworkStream strm = tcpClient.GetStream();
                IFormatter formatter = new BinaryFormatter();
                Message message = (Message)formatter.Deserialize(strm);
                Console.WriteLine("## Receiving a message: " + message.Head);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public void sendMessage(Message message)
        {
            Console.WriteLine("## Sending a message: " + message.Head);

            try
            {
                IFormatter formatter = new BinaryFormatter();
                NetworkStream strm = tcpClient.GetStream();
                formatter.Serialize(strm, message);
            }
            catch (SerializationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}