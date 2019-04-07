using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerIntellectus
{
    public class Utileria
    {
        public static void ImprimirConColor(String cadena, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(cadena);
            Console.ResetColor();
        }

        public static IPAddress ObtenerIPV4()
        {
            String strHostName = string.Empty;
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();
            //Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 0; i < addr.Length; i++)
            {
                if (addr[i].AddressFamily == AddressFamily.InterNetwork)
                    return addr[i];
            }

            return IPAddress.Any;
        }
    }
}
