using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerIntellectus.ProcesarPaquete
{
    class PMensajePrivadoRespuesta : IProcesarPaquete
    {
        Cliente Cliente { get; set; }
        IntellectusMensajes.MensajePrivado MensajePrivado { get; set; }

        public PMensajePrivadoRespuesta(Cliente Cliente, IntellectusMensajes.MensajePrivado MensajePrivado)
        {
            this.Cliente = Cliente;
            this.MensajePrivado = MensajePrivado;
        }
        public void ProcesarPaquete()
        {
            Cliente destinatario = null;
            
            IntellectusMensajes.MensajePrivadoRespuesta mensajePrivadoRespuesta = new IntellectusMensajes.MensajePrivadoRespuesta { IDDestinario = MensajePrivado.IDDestinario, IDRemitente = MensajePrivado.IDDestinario, Mensaje = MensajePrivado.Mensaje, TipoMensaje = MensajePrivado.TipoMensaje, VERSION = MensajePrivado.VERSION };
            try
            {
                destinatario = Server.lClientes.Where(x => MensajePrivado.IDDestinario == x.ID && x.Estado == Cliente.EstadoCliente.LOGUEADO).Single();

                mensajePrivadoRespuesta.Recibido = true;
                mensajePrivadoRespuesta.Razon = "";

                   
            }
            catch//El destinatario no está conectado
            {
                mensajePrivadoRespuesta.Recibido = false;
                mensajePrivadoRespuesta.Razon = "El destinatario no está conectado";
            }

            String mensaje = JsonConvert.SerializeObject(mensajePrivadoRespuesta);


            try
            {
                IntellectusSocketIO.SocketIO.EnviarPaqueteCompleto(destinatario.socketCliente, (int)IntellectusMensajes.Paquete.MensajePrivadoRespuesta, mensaje);
            }
            catch
            {
               if(destinatario != null)
               {
                    destinatario.Estado = Cliente.EstadoCliente.DESCONECTADO;
               }
            }

          
        }

        
    }
}
