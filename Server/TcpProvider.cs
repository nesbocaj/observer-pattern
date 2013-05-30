using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpShared;

namespace Server
{
    class TcpProvider
    {
        private static TcpProvider _instance;
        private TcpConnection _connection;
        private List<TcpSubscriber> _subscribers;

        private TcpProvider()
        {
            _subscribers = new List<TcpSubscriber>();
        }

        public static TcpProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TcpProvider();
                return _instance;
            }
        }

        public void RegisterSubscriber(TcpSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void UnregisterSubscriber(TcpSubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        public void NotifySubscribers(string txt) {
            if (_connection == null)
                _connection = TcpConnection.Instance;

            int i = 0;

            foreach (var subscriber in _subscribers)
            {
                _connection.Post(subscriber.Socket, txt);
                Console.WriteLine("notified subscriber {0} about {1}", i, txt);
                i++;
            }
        }
    }
}
