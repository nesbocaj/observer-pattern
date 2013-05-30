using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpShared
{
    public interface ITcpProvider
    {
        void RegisterSubscriber(ITcpSubscriber subscriber);

        void UnregisterSubscriber(ITcpSubscriber subscriber);

        void NotifySubscribers();
    }
}
