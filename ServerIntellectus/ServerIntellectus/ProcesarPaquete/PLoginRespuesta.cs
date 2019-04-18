using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerIntellectus.ProcesarPaquete
{
    public class PLoginRespuesta : IProcesarPaquete
    {
        Cliente Cliente { get; set; }
        IntellectusMensajes.LoginPeticion LoginPeticion { get; set; }

        public PLoginRespuesta(Cliente cliente, IntellectusMensajes.LoginPeticion loginPeticion)
        {
            this.Cliente = cliente;
            this.LoginPeticion = loginPeticion;
        }
        public void ProcesarPaquete()
        {
            //logica logueo de usuario
            IntellectusMensajes.LoginRespuesta loginRespuesta = null;
            try
            {
                try
                {
                    Server.lClientes.Where(x => x.ID == LoginPeticion.ID).Single();
                    loginRespuesta = new IntellectusMensajes.LoginRespuesta() { VERSION = LoginPeticion.VERSION, ID = LoginPeticion.ID, ESTADO = IntellectusMensajes.EstadoLogin.NOLOGUEADO, Mensaje = "No se ha podido loguear, ya existe un usuario conectado con esa cuenta." };

                    Utileria.ImprimirConColor("Intento de logueo ya existente desde IP: " + ((IPEndPoint)Cliente.socketCliente.RemoteEndPoint).Address.ToString(), ConsoleColor.Red);
                }
                catch
                {
                    loginRespuesta = new IntellectusMensajes.LoginRespuesta() { VERSION = LoginPeticion.VERSION, ID = LoginPeticion.ID, ESTADO = IntellectusMensajes.EstadoLogin.LOGUEADO, Mensaje = "Usuario logueado con éxito" };
                    Cliente.ID = LoginPeticion.ID;
                    Cliente.Estado = Cliente.EstadoCliente.LOGUEADO;

                    Utileria.ImprimirConColor("Usuario logueado ID: " + LoginPeticion.ID + " desde IP: " + ((IPEndPoint)Cliente.socketCliente.RemoteEndPoint).Address.ToString(), ConsoleColor.Green);
                }


                String mensajeAEnviar = JsonConvert.SerializeObject(loginRespuesta);

                IntellectusSocketIO.SocketIO.WriteInt(Cliente.socketCliente, (int)IntellectusMensajes.Paquete.LOGINRESPUESTA);

                byte[] buffer = Encoding.UTF8.GetBytes(mensajeAEnviar);

                IntellectusSocketIO.SocketIO.WriteInt(Cliente.socketCliente, buffer.Length);
                IntellectusSocketIO.SocketIO.Write(Cliente.socketCliente, buffer.Length, buffer);
            }
            catch(SocketException se)
            {
                Cliente.Estado = Cliente.EstadoCliente.DESCONECTADO;
            }
        }
    }
}
