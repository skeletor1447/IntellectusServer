using System;
using System.Collections.Generic;
using System.Text;

namespace IntellectusMensajes
{
    public enum Paquete { LOGIN = 1, MensajePrivado = 2, VerificacionConexion = 3, LOGINRESPUESTA = 500, MensajePrivadoRespuesta = 501, VerificacionConexionRespuesta = 502};
    public enum EstadoLogin { NOLOGUEADO = 0, LOGUEADO = 1};
    public enum TipoMensaje { TEXTO = 0, IMAGEN = 1};
}
