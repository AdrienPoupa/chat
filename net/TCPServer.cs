using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            IPAddress ipAddress = ipHostEntry.AddressList[0];

            waitSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            waitSocket.Bind(new IPEndPoint(ipAddress, port));
            waitSocket.Listen(2000);

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

        public Message getMessage()
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
        }

        public abstract void gereClient(Socket comm);
    }
}
