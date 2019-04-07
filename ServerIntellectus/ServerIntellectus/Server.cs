using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerIntellectus
{
    public class Server
    {
        public Socket SocketServer;
        private IPEndPoint localEndPoint;
        private List<Cliente> lClientes;
        private bool boolEscucharConexionesEntrantes;
        private bool boolObtenerPeticiones;
        private Thread threadEscucharConexionesEntrantes;
        private Thread threadObtenerPeticiones;
        public Server(int puerto)
        {
            boolEscucharConexionesEntrantes = true;
            boolObtenerPeticiones = true;

            lClientes = new List<Cliente>();
            SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                localEndPoint = new IPEndPoint(Utileria.ObtenerIPV4(), puerto);
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

        public void Iniciar()
        {
            threadEscucharConexionesEntrantes = new Thread(this.EscucharConexionesEntrantes);
            threadObtenerPeticiones = new Thread(this.ObtenerPeticiones);
            threadEscucharConexionesEntrantes.Start();
            threadObtenerPeticiones.Start();
            
        }
        

        private void ObtenerPeticiones()
        {
            while(boolObtenerPeticiones)
            {
                lock(lClientes)
                {
                    foreach(var cliente in lClientes)
                    {
                        if(cliente.socketCliente.Available > 0)
                        {
                            try
                            {
                                ProcesarPaquete.IProcesarPaquete procesarPaquete = ObtenerPaqueteCompleto(cliente);
                                if (procesarPaquete != null)
                                    procesarPaquete.ProcesarPaquete();
                                else
                                    Console.WriteLine("No hay ningun paquete para procesar");
                            }
                            catch(SocketException se)//cliente desconectado
                            {
                                lClientes.Remove(cliente);
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private ProcesarPaquete.IProcesarPaquete ObtenerPaqueteCompleto(Cliente cliente)
        {
            int paquete = IntellectusSocketIO.SocketIO.ReadInt(cliente.socketCliente);
            int longitud = IntellectusSocketIO.SocketIO.ReadInt(cliente.socketCliente);
            String mensaje = IntellectusSocketIO.SocketIO.ReadString(cliente.socketCliente, longitud);

            switch (paquete)
            {
                case (int)IntellectusMensajes.Paquete.LOGIN:
                    IntellectusMensajes.LoginPeticion loginPeticion = JsonConvert.DeserializeObject<IntellectusMensajes.LoginPeticion>(mensaje);
                    return new ProcesarPaquete.PLoginRespuesta(cliente, loginPeticion);
                    
            }

            return null;
        }

        private void EscucharConexionesEntrantes()
        {
            while(boolEscucharConexionesEntrantes)
            {
                Socket socket = SocketServer.Accept();

                Cliente cliente = new Cliente() { Estado = Cliente.EstadoCliente.NOLOGUEADO, socketCliente = socket, ID = -1 };

                lock(lClientes)
                {
                    lClientes.Add(cliente);
                    Utileria.ImprimirConColor("Conexion entrante desde IP: " + ((IPEndPoint)cliente.socketCliente.RemoteEndPoint).Address.ToString(), ConsoleColor.Yellow);
                }
            }
        }
    }
}
