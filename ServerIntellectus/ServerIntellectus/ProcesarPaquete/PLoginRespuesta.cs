using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

            IntellectusMensajes.LoginRespuesta loginRespuesta = new IntellectusMensajes.LoginRespuesta() { VERSION = 1, ID = 1001, ESTADO = (int)IntellectusMensajes.EstadoLogin.NOLOGUEADO, Mensaje = "no se ha podido loguear, ya existe un cliente conectado." };

            String mensajeAEnviar = JsonConvert.SerializeObject(loginRespuesta);

            IntellectusSocketIO.SocketIO.WriteInt(Cliente.socketCliente,(int)IntellectusMensajes.Paquete.LOGINRESPUESTA);

            byte[] buffer = Encoding.UTF8.GetBytes(mensajeAEnviar);

            IntellectusSocketIO.SocketIO.WriteInt(Cliente.socketCliente, buffer.Length);
            IntellectusSocketIO.SocketIO.Write(Cliente.socketCliente, buffer.Length, buffer);
        }
    }
}
