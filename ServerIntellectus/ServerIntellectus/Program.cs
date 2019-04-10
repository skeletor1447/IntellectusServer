using Rollbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerIntellectus
{
    
    class Program
    {
        static String version = "1.0";

        static void Main(string[] args)
        {
            RollbarLocator.RollbarInstance.Configure(new RollbarConfig("fe9f3e0934734bae922fb1cb33fc1d4b"));
            RollbarLocator.RollbarInstance.Info("Rollbar is configured properly.");

            Utileria.ImprimirConColor("                             Servidor Intellectus              ", ConsoleColor.Blue);
            Utileria.ImprimirConColor("                                 Version " + version, ConsoleColor.White);

            if (args.Length > 0)
            {
                try
                {
                    int puerto = int.Parse(args[0]);
                    try
                    {
                        Utileria.ImprimirConColor("Iniciando servidor...", ConsoleColor.Green);
                        Server server = new Server(puerto);
                        server.Iniciar();
                        Utileria.ImprimirConColor("Servidor iniciado correctamente.", ConsoleColor.Green);
                        Console.WriteLine();
                        Utileria.ImprimirConColor("Servidor corriendo en la IP: " + ((IPEndPoint)server.SocketServer.LocalEndPoint).Address.ToString() +" en el Puerto: "+ ((IPEndPoint)server.SocketServer.LocalEndPoint).Port.ToString(), ConsoleColor.White);
                        Console.WriteLine();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                catch
                {
                    Console.WriteLine("Puerto no valido, ingrese por parametro el puerto");
                }
                
            }

            Console.ReadLine();
        }


        
    }
}
