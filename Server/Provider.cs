using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpShared;

namespace Server
{
    class Provider : ITcpProvider
    {
        private static Provider _instance;
        private List<ITcpSubscriber> _subscribers;

        private Provider()
        {
            _subscribers = new List<ITcpSubscriber>();
        }

        public static Provider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Provider();
                return _instance;
            }
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
