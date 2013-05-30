using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TcpShared;

namespace Server
{
    class TcpConnection : ITcpConnection
    {
        private static TcpConnection _instance;
        private IPAddress _address;
        private int _port;
        private TcpListener _listener;
        private TcpProvider _provider;

        private TcpConnection()
        {
            _address = IPAddress.Parse("127.0.0.1");
            _port = 7000;
            _listener = new TcpListener(_address, _port);
            _provider = TcpProvider.Instance;
        }

        public static TcpConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TcpConnection();
                return _instance;
            }
        }

        public void Listen()
        {
            var remote = default(EndPoint);
            _listener.Start();

            while (true)
            {
                try
                {
                    var current = _listener.AcceptSocket();
                    remote = current.RemoteEndPoint;

                    Console.WriteLine("200 OK - Connection accepted from {0}",
                        remote);

                    var clientThread = new Thread(() => ManageClient(current));
                    clientThread.Start();
                }
                catch (SocketException se)
                {
                    if (remote == null)
                        Console.WriteLine("410 GONE - Connection not established");
                    else
                        Console.WriteLine("410 GONE - Connection with {0} was interrupted",
                            remote);
                    Debug.WriteLine(se.Message);
                }
            }
        }

        public void ManageClient(Socket currentSocket)
        {
            var stream = new NetworkStream(currentSocket);
            var reader = new BinaryReader(stream);

            try
            {
                var command = reader.ReadString();

                if (command.Contains("watch"))
                {
                    var subscriber = new TcpSubscriber(currentSocket);
                    _provider.RegisterSubscriber(subscriber);
                }
                else
                    Post(currentSocket, Request(command));
            }
            catch (Exception ex)
            {
                Console.WriteLine("400 - BAD REQUEST");
                Debug.WriteLine(ex.Message);
            }

            stream.Close();
        }

        // for now just echoes the request
        public string Request(string txt)
        {
            var result = txt;

            return result;
        }

        public void Post(string txt) { }

        public void Post(Socket socket, string txt)
        {
            var stream = new NetworkStream(socket);

            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(txt);
            }
        }
    }
}
