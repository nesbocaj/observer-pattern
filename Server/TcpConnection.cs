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
        private IPEndPoint _requestEndpoint;
        private IPEndPoint _postEndpoint;
        private TcpListener _listener;
        private TcpClient _client;
        private TcpProvider _provider;

        private TcpConnection()
        {
            _requestEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);
            _postEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7001);
            _listener = new TcpListener(_requestEndpoint);
            _client = new TcpClient(_postEndpoint);
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
                    var parsed = ParseCommand(command);

                    var endpoint = new IPEndPoint(IPAddress.Parse(parsed[1]), int.Parse(parsed[2]));

                    var subscriber = new TcpSubscriber(endpoint);
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
            try
            {
                var stream = new NetworkStream(socket);
                var writer = new BinaryWriter(stream);
                writer.Write(txt);
            }
            catch (IOException ioe) { }
        }

        public void Post(IPEndPoint endpoint, string txt)
        {
            try
            {
                _client.Connect(endpoint);

                var writer = new BinaryWriter(_client.GetStream());
                writer.Write(txt);
            }
            catch (IOException ioe) { }
        }

        private List<string> ParseCommand(string command)
        {
            bool inQuotes = false;
            int i = 0;
            List<string> arguments = new List<string>();
            arguments.Add("");

            foreach (char c in command)
            {
                switch (c)
                {
                    case ' ':
                        if (!inQuotes)
                        {
                            arguments.Add("");
                            i++;
                        }
                        break;
                    case '"':
                        if (!inQuotes)
                            inQuotes = true;
                        else
                            inQuotes = false;
                        break;
                    default:
                        arguments[i] += c;
                        break;
                }
            }

            return arguments;
        }
    }
}
