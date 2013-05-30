using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    interface ISubscriber
    {
        void SubscribeTo(Provider provider);
        void Notify();
    }
}
