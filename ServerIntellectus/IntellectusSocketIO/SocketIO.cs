﻿using System;
using System.Net.Sockets;
using System.Text;

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
        /// <param name="numero"> Entero que se enviará al socket remoto.</param>
        public static void WriteInt(Socket socket, int numero)
        {
            socket.Send(BitConverter.GetBytes(numero), sizeof(Int32), SocketFlags.None);
        }
        /// <summary>Lee un entero de 32 bits del socket remoto.</summary>
        /// <param name="socket"> Socket remoto, donde se leerá una longitud de bytes.</param>
        /// <param name="longitud"> Entero que indica cuantos bytes se deben de leer del socket remoto.</param>
        public static String ReadString(Socket socket,int longitud)
        {
            byte[] buffer = new byte[longitud];
            int bytesLeidos = 0;
            int bytesLeidosTotal = 0;

            do
            {
                bytesLeidos = socket.Receive(buffer, longitud - bytesLeidosTotal, SocketFlags.None);
                bytesLeidosTotal += bytesLeidos;
            }
            while (bytesLeidosTotal < longitud && bytesLeidos > 0);

            if (bytesLeidosTotal != longitud)
                throw new Exception("Error al completar la lectura de los bytes, posible desconexion.");

            return Encoding.UTF8.GetString(buffer);
        }
        /// <summary>Lee un entero de 32 bits del socket remoto.</summary>
        /// <param name="socket"> Socket remoto, donde se enviará una longitud de bytes.</param>
        /// <param name="longitud"> Entero que indica cuantos bytes se deben de enviar al socket remoto.</param>
        /// <param name="buffer"> array de bytes que serán enviados al socket remoto.</param>
        public static void Write(Socket socket, int longitud, byte[] buffer)
        {
            int bytesLeidos = 0;
            int bytesLeidosTotal = 0;

            do
            {
                bytesLeidos = socket.Send(buffer, longitud - bytesLeidos, SocketFlags.None);
                bytesLeidosTotal += bytesLeidos;
            }
            while (bytesLeidosTotal < longitud && bytesLeidos > 0);

            if (bytesLeidosTotal != longitud)
                throw new Exception("Error al completar la escritura de los bytes, posible desconexion.");
        }
        /// <summary>
        /// Recibe un paquete completo del servidor.
        /// </summary>
        /// <param name="socket">Socket local que recibe un paquete del servidor.</param>
        /// <returns></returns>
        public static String ObtenerPaqueteCompleto(Socket socket)
        {
            int longitud = ReadInt(socket);
            String mensajeRespuesta = IntellectusSocketIO.SocketIO.ReadString(socket, longitud);

            return mensajeRespuesta;
        }
        /// <summary>
        /// Envia un paquete completo al servidor
        /// </summary>
        /// <param name="socket">Socket remoto, donde se enviará el paquete.</param>
        /// <param name="paquete">Entero que indica que tipo de paquete será enviado.</param>
        /// <param name="mensaje">String que se será enviado como cuerpo del paquete.</param>
        public static void EnviarPaqueteCompleto(Socket socket,int paquete, String mensaje)
        {
            WriteInt(socket, paquete);
            byte[] buffer = Encoding.UTF8.GetBytes(mensaje);
            WriteInt(socket, buffer.Length);
            Write(socket, buffer.Length, buffer);
        }
    }
}
