using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        private byte[] bytes;

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

        public Message getMessage()
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
        }
    }
}