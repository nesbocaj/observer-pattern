using System;
using System.Collections.Generic;
using System.Linq;
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
            
        }
    }
}
