using System;
using System.Net.Sockets;

namespace IntellectusSocketIO
{
    public class SocketIO
    {
        /// <summary>Lee un entero de 32 bits del socket remoto.</summary>
        /// <param name="socket"> Socket remoto, donde se leerá un entero de 32 bits.</param>
        public static int ReadInt(Socket socket)
        {
            byte[] buffer = new byte[sizeof(Int32)];

            socket.Receive(buffer,sizeof(Int32),SocketFlags.None);

            return BitConverter.ToInt32(buffer,0);
        }

        /// <summary>Lee un entero de 32 bits del socket remoto.</summary>
        /// <param name="socket"> Socket remoto, donde se enviará un entero de 32 bits.</param>
        /// <param name="numero"> entero que se enviará al socket remoto.</param>
        public static void WriteInt(Socket socket, int numero)
        {
            socket.Send(BitConverter.GetBytes(numero), sizeof(Int32), SocketFlags.None);
        }
    }
}
