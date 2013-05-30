using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Subscriber : ISubscriber
    {
        public void SubscribeTo(Provider provider)
        {
            provider.RegisterSubscriber(this);
        }

        public void Notify()
        {
            
        }
    }
}
