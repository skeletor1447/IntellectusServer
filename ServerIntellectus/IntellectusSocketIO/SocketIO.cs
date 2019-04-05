using System;
using System.Net.Sockets;

namespace IntellectusSocketIO
{
    public class SocketIO
    {
        /// <summary>Envía un entero de 32 bits al socket destino.</summary>
        /// <param name="socket"> Socket destino, donde se enviará un entero de 32 bits.</param>
        /// <param name="numero"> Entero que se enviará al socket destino.</param>
        public static void ReadInt(Socket socket, int numero)
        {
            socket.Receive(BitConverter.GetBytes(numero),sizeof(Int32),SocketFlags.None);
        }
    }
}
