using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpShared
{
    public interface ITcpSubscriber
    {
        void SubscribeTo(ITcpProvider provider);
        void Notify();
    }
}
