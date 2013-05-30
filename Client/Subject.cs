using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Subject
    {
        private List<IObserver> _observers;

        public Subject()
        {
            _observers = new List<IObserver>();
        }

        public void RegisterObserver(IObserver observer) { }

        public void UnregisterObserver(IObserver observer) { }

        public void NotifyObservers() { }
    }
}
