using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private TcpConnection _connection;
        private TcpProvider _provider;
        private string[] _messages;

        public Program()
        {
            Console.WriteLine("Provider Server");

            _connection = TcpConnection.Instance;
            _provider = TcpProvider.Instance;
            _messages = new string[] {
                "The",
                "quick",
                "brown",
                "fox",
                "jumps",
                "over",
                "the",
                "lazy",
                "dog"
            };

            var notifyWorker = new Thread(Notify);
            notifyWorker.Name = "Notify Worker";
            notifyWorker.Start();

            _connection.Listen();
        }

        static void Main(string[] args)
        {
            new Program();
        }

        private void Notify()
        {
            foreach (var message in _messages)
            {
                _provider.NotifySubscribers(message);
                Thread.Sleep(1000);
            }
        }
    }
}
