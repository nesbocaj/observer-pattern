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

        public string Request(string txt)
        {
            var result = "";
            SocketException se = null;

            Connect(_connection, out se);

            if (_connection.Connected)
            {
                var reader = new BinaryReader(_connection.GetStream());
                var writer = new BinaryWriter(_connection.GetStream());

                writer.Write(txt);
                result = reader.ReadString();

            }
            else throw se;

            return result;
        }

        public void Post(string txt)
        {

        }
    }
}
