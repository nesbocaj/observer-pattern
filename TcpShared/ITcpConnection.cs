using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpShared
{
    public interface ITcpConnection
    {
        string Request(string txt);
        void Post(string txt);
    }
}
