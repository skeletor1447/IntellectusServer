using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerIntellectus
{
    
    class Program
    {
        static String version = "1.0";

        static void Main(string[] args)
        {
            ImprimirConColor("                             Servidor Intellectus              ", ConsoleColor.Blue);
            ImprimirConColor("                                 Version " + version, ConsoleColor.White);

            Server server = new Server("192.168.1.71",8001);

            server.Iniciar();

            Console.ReadLine();
        }


        static void ImprimirConColor(String cadena, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(cadena);
            Console.ResetColor();
        }
    }
}
