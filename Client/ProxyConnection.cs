using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Client
{
    enum Behavior { ReadOnly, Write, ReadWrite };

    class ProxyConnection
    {
        private static ProxyConnection _instance = null;

        public TcpClient Client {get; set;}

        private ProxyConnection()
        {
            Client = new TcpClient();
        }

        public static ProxyConnection Instance { get { return _instance == null ? _instance = new ProxyConnection() : _instance; } }

        public void Connect(TcpClient connection, out SocketException ex)
        {
            ex = null;

            for (int i = 0; i < 10 && !connection.Connected; i++)
            {
                try
                {
                    connection.Connect(IPAddress.Parse("127.0.0.1"), 7000);
                    break;
                }
                catch (SocketException se)
                {
                    ex = se;
                    continue;
                }
            }
        }

        public void DoWhenConnected(bool connected, Behavior behavior, SocketException se, out string result, string command = null)
        {
            result = null;

            if (connected)
            {
                BinaryReader reader = null;
                BinaryWriter writer = null;

                // if the behavior is set to read it'll only read, if write it'll only write, if readwrite it'll do both
                if (behavior == Behavior.ReadOnly || behavior == Behavior.ReadWrite)
                    reader = new BinaryReader(Client.GetStream());

                if (behavior == Behavior.Write || behavior == Behavior.ReadWrite)
                {
                    writer = new BinaryWriter(Client.GetStream());
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

                //Client.Close();
            }
            else throw se;
        }

        public string Request(string txt)
        {
            var result = "";
            SocketException se = null;
            Connect(Client, out se);
            DoWhenConnected(Client.Connected, Behavior.ReadWrite, se, out result, txt);
            return result;
        }

        public void Post(string txt) // POST - Power On Self-Test
        {
            var result = ""; // never needed but required since there's an out parameter
            SocketException se = null;
            Connect(Client, out se);
            DoWhenConnected(Client.Connected, Behavior.ReadWrite, se, out result, txt);
        }
    }
}
