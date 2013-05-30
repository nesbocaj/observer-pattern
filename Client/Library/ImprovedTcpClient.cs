using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Library
{
    class ImprovedTcpClient : TcpClient
    {
        private bool _disposed;

        public ImprovedTcpClient() : base() { }

        public ImprovedTcpClient(System.Net.IPEndPoint localEP) : base(System.Net.IPEndPoint localEP) { }

        public bool Disposed
        {
            get { return _disposed; }
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
