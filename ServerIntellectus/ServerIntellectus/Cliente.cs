using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerIntellectus
{
    public class Cliente
    {
        public enum EstadoCliente { LOGUEADO, NOLOGUEADO, DESCONECTADO}
        public Socket socketCliente { get; set; } 
        public EstadoCliente Estado { get; set; }
        public int ID { get; set; }
    }
}
