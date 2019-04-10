using System;
using System.Collections.Generic;
using System.Text;

namespace IntellectusMensajes
{
    public class MensajePrivado
    {
        public int VERSION { get; set; }
        public long IDDestinario { get; set; }
        public long IDRemitente { get; set; }
        public byte[] Mensaje { get; set; }
        public TipoMensaje TipoMensaje { get; set; }

    }
}
