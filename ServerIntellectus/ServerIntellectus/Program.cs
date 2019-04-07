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

            if (args.Length > 0)
            {
                try
                {
                    int puerto = int.Parse(args[0]);
                    try
                    {
                        ImprimirConColor("Iniciando servidor...", ConsoleColor.Green);
                        Server server = new Server(puerto);
                        server.Iniciar();
                        ImprimirConColor("Servidor iniciado correctamente.", ConsoleColor.Green);
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


        static void ImprimirConColor(String cadena, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(cadena);
            Console.ResetColor();
        }
    }
}
