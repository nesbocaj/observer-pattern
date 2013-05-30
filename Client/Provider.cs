using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Provider
    {
        private List<ISubscriber> _observers;

        public Provider()
        {
            _observers = new List<ISubscriber>();
        }

        public void RegisterSubscriber(ISubscriber observer)
        {
            _observers.Add(observer);
        }

        public void UnregisterSubscriber(ISubscriber observer)
        {
            _observers.Remove(observer);
        }

        public void NotifySubscribers() {
            foreach (var observer in _observers)
            {
                observer.Notify();
            }
        }
    }
}
