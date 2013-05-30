using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpShared;

namespace Server
{
    class TcpSubscriber : ITcpSubscriber
    {
        private Socket _socket;

        public TcpSubscriber(Socket socket)
        {
            _socket = socket;
        }

        public Socket Socket
        {
            get { return _socket; }
        }

        public void SubscribeTo(ITcpProvider provider) { }

        public void Notify() { }
    }
}
