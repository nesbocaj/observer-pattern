using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpShared;

namespace ReadWriteClient
{
    class Subscriber : ITcpSubscriber
    {
        private ProxyConnection _prox;

        public void SubscribeTo(ITcpProvider provider)
        {
            provider.RegisterSubscriber(this);
        }

        public void Notify()
        {
            if (_prox == null)
                _prox = ProxyConnection.Instance;

            SocketException se = null;
            String result = "";

            _prox.Connect(_prox.ReadOnlyClient, out se);

            while (_prox.ReadOnlyClient.Connected)
            {
                _prox.DoWhenConnected(_prox.ReadOnlyClient, Behavior.ReadOnly, se, out result);
                Console.WriteLine(result);
            }

            _prox.ReadOnlyClient.Close();
        }
    }
}
