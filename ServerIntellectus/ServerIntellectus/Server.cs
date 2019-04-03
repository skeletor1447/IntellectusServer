using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerIntellectus
{
    public class Server
    {
        private Socket SocketServer;
        IPEndPoint localEndPoint;

        public Server(String IP,int puerto)
        {
            SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                localEndPoint = new IPEndPoint(IPAddress.Parse(IP), puerto);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                SocketServer.Bind(localEndPoint);
                SocketServer.Listen((int)SocketOptionName.MaxConnections);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
