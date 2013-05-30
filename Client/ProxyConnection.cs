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
    class ProxyConnection
    {
        private TcpClient _connection = null;

        private enum Behavior {Read, Write, ReadWrite};


        public ProxyConnection()
        {
            _connection = new TcpClient();
        }

        private void Connect(TcpClient connection, out SocketException ex)
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

        private void DoWhenConnected(bool connected, Behavior behavior, SocketException se, out string result, string command = null)
        {
            result = null;

            if (connected)
            {
                BinaryReader reader = null;
                BinaryWriter writer = null;

                // if the behavior is set to read it'll only read, if write it'll only write, if readwrite it'll do both
                if (behavior == Behavior.Read || behavior == Behavior.ReadWrite)
                    reader = new BinaryReader(_connection.GetStream());

                if (behavior == Behavior.Write || behavior == Behavior.ReadWrite)
                {
                    writer = new BinaryWriter(_connection.GetStream());
                    writer.Write(command);
                }
                if (behavior == Behavior.Read || behavior == Behavior.ReadWrite)
                    result = reader.ReadString();

                _connection.Close();
            }
            else throw se;
        }

        public string Request(string txt)
        {
            var result = "";
            SocketException se = null;
            Connect(_connection, out se);
            DoWhenConnected(_connection.Connected, Behavior.ReadWrite, se, out result, txt);
            return result;
        }

        public void Post(string txt) // POST - Power On Self-Test
        {
            var result = ""; // never needed but required since there's an out parameter
            SocketException se = null;
            Connect(_connection, out se);
            DoWhenConnected(_connection.Connected, Behavior.Write, se, out result, txt);

        }
    }
}
