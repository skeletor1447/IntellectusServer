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
        private Socket SocketServer;
        private IPEndPoint localEndPoint;
        private List<Cliente> lClientes;
        private bool boolEscucharConexionesEntrantes;
        private Thread threadEscucharConexionesEntrantes; 
        public Server(String IP,int puerto)
        {
            boolEscucharConexionesEntrantes = true;

            lClientes = new List<Cliente>();
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

        public void Iniciar()
        {
            threadEscucharConexionesEntrantes = new Thread(this.EscucharConexionesEntrantes);
            threadEscucharConexionesEntrantes.Start();
            
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
                }
            }
        }
    }
}
