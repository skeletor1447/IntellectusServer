﻿using Newtonsoft.Json;
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
        private bool boolObtenerPeticiones;
        private Thread threadEscucharConexionesEntrantes;
        private Thread threadObtenerPeticiones;
        public Server(String IP,int puerto)
        {
            boolEscucharConexionesEntrantes = true;
            boolObtenerPeticiones = true;

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
                            int paquete = IntellectusSocketIO.SocketIO.ReadInt(cliente.socketCliente);
                            int longitud = IntellectusSocketIO.SocketIO.ReadInt(cliente.socketCliente);
                            String mensaje = IntellectusSocketIO.SocketIO.ReadString(cliente.socketCliente,longitud);

                            IntellectusMensajes.LoginPeticion loginPeticion = JsonConvert.DeserializeObject<IntellectusMensajes.LoginPeticion>(mensaje);
                            ProcesarPaquete.IProcesarPaquete procesarPaquete = null;
                            switch(paquete)
                            {
                                case (int)IntellectusMensajes.Paquete.LOGIN:
                                    procesarPaquete = new ProcesarPaquete.PLoginRespuesta(cliente,loginPeticion);
                                    break;
                            }

                            if(procesarPaquete != null)
                                procesarPaquete.ProcesarPaquete();
                            else
                                Console.WriteLine("no hay ningun paquete para procesar");
                        }
                    }
                }
            }
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
