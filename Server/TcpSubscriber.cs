using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TcpShared;

namespace Server
{
    class TcpSubscriber : ITcpSubscriber
    {
        private IPEndPoint _endPoint;

        public TcpSubscriber(System.Net.IPEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public IPEndPoint EndPoint
        {
            get { return _endPoint; }
        }

        public void SubscribeTo(ITcpProvider provider) { }

        public void Notify() { }
    }
}
