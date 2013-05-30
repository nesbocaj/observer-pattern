using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client");

            var connection = ProxyConnection.Instance;
            var subscriber = new Subscriber();

            connection.Post("watch");
            subscriber.Notify();
        }
    }
}
