using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpShared;

namespace Client
{
    class Subscriber : ITcpSubscriber
    {
        public void SubscribeTo(ITcpProvider provider)
        {
            provider.RegisterSubscriber(this);
        }

        public void Notify()
        {
            var prox = ProxyConnection.Instance;
            SocketException se = null;
            String result = "";

            prox.Connect(out se);
            while (prox.Client.Connected)
            {
                prox.DoWhenConnected(Behavior.ReadOnly, se, out result);
                Console.WriteLine(result);
            }

            prox.Client.Close();
        }
    }
}
