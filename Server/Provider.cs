using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpShared;

namespace Client
{
    class Provider : ITcpProvider
    {
        private List<ITcpSubscriber> _subscribers;

        public Provider()
        {
            _subscribers = new List<ITcpSubscriber>();
        }

        public void RegisterSubscriber(ITcpSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void UnregisterSubscriber(ITcpSubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        public void NotifySubscribers() {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Notify();
            }
        }
    }
}
