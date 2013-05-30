using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ReadWriteClient
{
    enum Behavior { ReadOnly, Write, ReadWrite };

    class ProxyConnection
    {
        private static ProxyConnection _instance = null;
        private TcpClient _readWriteClient, _readOnlyClient;
        private IPEndPoint _readWriteEndpoint, _readOnlyEndpoint;

        private ProxyConnection()
        {
            _readWriteClient = new TcpClient();
            _readOnlyClient = new TcpClient();
            _readWriteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);
            _readOnlyEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7001);
        }

        public static ProxyConnection Instance { get { return _instance == null ? _instance = new ProxyConnection() : _instance; } }

        public TcpClient ReadWriteClient
        {
            get { return _readWriteClient; }
        }

        public TcpClient ReadOnlyClient
        {
            get { return _readOnlyClient; }
        }

        public void Connect(TcpClient client, out SocketException ex)
        {
            ex = null;

            for (int i = 0; i < 10 && !ReadWriteClient.Connected; i++)
            {
                try
                {
                    if (client.Equals(ReadWriteClient))
                        client.Connect(_readWriteEndpoint);
                    if (client.Equals(ReadOnlyClient))
                        client.Connect(_readOnlyEndpoint);
                    break;
                }
                catch (SocketException se)
                {
                    ex = se;
                    continue;
                }
            }
        }

        public void DoWhenConnected(TcpClient client, Behavior behavior, SocketException se, out string result, string command = null)
        {
            result = null;

            if (client.Connected)
            {
                BinaryReader reader = null;
                BinaryWriter writer = null;

                // if the behavior is set to read it'll only read, if write it'll only write, if readwrite it'll do both
                if (behavior == Behavior.ReadOnly || behavior == Behavior.ReadWrite)
                    reader = new BinaryReader(client.GetStream());

                if (behavior == Behavior.Write || behavior == Behavior.ReadWrite)
                {
                    writer = new BinaryWriter(client.GetStream());
                    writer.Write(command);
                }
                if (behavior == Behavior.ReadOnly || behavior == Behavior.ReadWrite)
                {
                    try
                    {
                        result = reader.ReadString();
                    }
                    catch (System.IO.IOException ioe)
                    {
                        Console.WriteLine("Server has closed, and lef this message: {0}", ioe.Message);
                    }
                }

                if (client.Equals(ReadWriteClient))
                {
                    client.Close();
                    client = new TcpClient();
                }
            }
            else throw se;
        }

        public string Request(string txt)
        {
            var result = "";
            SocketException se = null;
            Connect(ReadWriteClient, out se);
            try
            {
                DoWhenConnected(ReadWriteClient, Behavior.ReadWrite, se, out result, txt);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(se.Message);
            }
            return result;
        }

        public void Post(string txt) // POST - Power On Self-Test
        {
            var result = ""; // never needed but required since there's an out parameter
            SocketException se = null;
            if (txt.Equals("watch", StringComparison.InvariantCultureIgnoreCase))
            {
                Connect(ReadOnlyClient, out se);
                try
                {
                    DoWhenConnected(ReadOnlyClient, Behavior.Write, se, out result, txt);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(se.Message);
                }
            }
            else
            {
                Connect(ReadWriteClient, out se);
                try
                {
                    DoWhenConnected(ReadWriteClient, Behavior.Write, se, out result, txt);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(se.Message);
                }
            }
        }
    }
}
