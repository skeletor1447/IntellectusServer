using System;
using System.Collections.Generic;
using System.Text;

namespace IntellectusMensajes
{
    public class MensajePrivadoRespuesta
    {
        public int VERSION { get; set; }
        public long IDDestinario { get; set; }
        public long IDRemitente { get; set; }
        public byte[] Mensaje { get; set; }
        public TipoMensaje TipoMensaje { get; set; }
        public bool Recibido { get; set; }
        public String Razon { get; set; }
    }
}
