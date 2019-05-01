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
        public static List<Cliente> lClientes;
        private bool boolEscucharConexionesEntrantes;
        private bool boolObtenerPeticiones;
        private Thread threadEscucharConexionesEntrantes;
        private Thread threadObtenerPeticiones;
        private Thread threadEnviarVerificacionConexion;
        public Server(int puerto)
        {
            boolEscucharConexionesEntrantes = true;
            boolObtenerPeticiones = true;

            lClientes = new List<Cliente>();
            SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                localEndPoint = new IPEndPoint(/*Utileria.ObtenerIPV4()*/IPAddress.Parse("127.0.0.1"), puerto);
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
            threadEnviarVerificacionConexion = new Thread(this.EnviarVerificacionConexion);
            threadEscucharConexionesEntrantes.Start();
            threadObtenerPeticiones.Start();
            threadEnviarVerificacionConexion.Start();
            
        }
        

        private void EnviarVerificacionConexion()
        {
            while(true)
            {
                Thread.Sleep(10000);

                IntellectusMensajes.VerificacionConexionRespuesta vcr = new IntellectusMensajes.VerificacionConexionRespuesta();
                vcr.ESTADO = "Verificando";
                vcr.VERSION = 1;
                lock(Server.lClientes)
                {
                    String mensaje = JsonConvert.SerializeObject(vcr);
                    foreach (var cliente in lClientes)
                    {
                        try
                        {
                            IntellectusSocketIO.SocketIO.EnviarPaqueteCompleto(cliente.socketCliente, (int)IntellectusMensajes.Paquete.VerificacionConexionRespuesta, mensaje);
                        }
                        catch(SocketException se)
                        {
                            Console.WriteLine("cliente desconectado");
                            cliente.Estado = Cliente.EstadoCliente.DESCONECTADO;
                        }
                    }
                }
                
            }
        }

        private void ObtenerPeticiones()
        {
            while(boolObtenerPeticiones)
            {
                lock(lClientes)
                {
                    ServidorServicios.ServidorServicesClient servidorServicesClient = null;
                    try
                    {
                        servidorServicesClient = new ServidorServicios.ServidorServicesClient();
                    }
                    catch(Exception es)
                    {
                        Console.WriteLine(es.Message);
                        Console.WriteLine("Es necesario reiniciar el servidor");
                        return;
                    }
                    List<Cliente> listaARemover = lClientes.Where(x => x.Estado == Cliente.EstadoCliente.DESCONECTADO).ToList();

                    foreach (var cliente in listaARemover)
                    {
                        bool echo = servidorServicesClient.OnlineAOffline(cliente.ID);
                    }

                    lClientes.RemoveAll(x => x.Estado == Cliente.EstadoCliente.DESCONECTADO);
                    
                    foreach(var cliente in lClientes)
                    {
                        if(cliente.socketCliente.Available > 0 )
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
                                cliente.Estado = Cliente.EstadoCliente.DESCONECTADO;
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
                case (int)IntellectusMensajes.Paquete.MensajePrivado:
                    IntellectusMensajes.MensajePrivado mensajePrivado = JsonConvert.DeserializeObject<IntellectusMensajes.MensajePrivado>(mensaje);
                    return new ProcesarPaquete.PMensajePrivadoRespuesta(cliente, mensajePrivado);
                    
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
