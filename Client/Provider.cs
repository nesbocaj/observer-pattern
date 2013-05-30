using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Provider
    {
        private List<ISubscriber> _subscribers;

        public Provider()
        {
            _subscribers = new List<ISubscriber>();
        }

        public void RegisterSubscriber(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void UnregisterSubscriber(ISubscriber subscriber)
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
