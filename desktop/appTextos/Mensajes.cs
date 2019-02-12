using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBTextos
{
    public class Mensajes
    {
        public readonly static string Mensaje_Error_Generico = "Ocurrió un problema, por favor vuelva a intentarlo más tarde";
        public readonly static string Mensaje_Sin_Respuesta = "No hubo respuesta, por favor vuelva a intentarlo más tarde";

        public readonly static string Mensaje_Sin_Empresas_Servicio = "Lo sentimos, no se encontró información de empresas de servicios";
        public readonly static string Mensaje_Sin_Servicio = "Lo sentimos, no se encontró información de servicios";

        public readonly static string Mensaje_Credenciales_No_Validas = "Usuario o clave no válidos";

        #region mensajes redirect error
        #endregion



        public readonly static string TITULO_BUSQUEDA_COBRANZAS = "Relación de cuentas por cobrar con vencimiento del {0} al {1}";
        public readonly static string TITULO_BUSQUEDA_DETALLE_COBRANZAS = "Vencimiento del {0} al {1}";
        public readonly static string TITULO_CORREOS_ENVIADOS = "Relación de correos enviados del {0} al {1}";

        public readonly static string MENSAJE_FECHAS_NO_VALIDAS = "Seleccione una fecha de inicio y una fecha fin para la búsqueda";
        public readonly static string MENSAJE_SELECCIONE_CLIENTES = "No ha seleccionado clientes para el envío de correos";
        public readonly static string MENSAJE_LOG_ENVIOS_ERROR = "Ocurrió un problema al grabar el historial de envios, contacte por favor a Sistemas";

        public readonly static string MENSAJE_CORREOS_ENVIADOS = "Correos Enviados: {0} ";
        public readonly static string MENSAJE_CORREOS_NO_ENVIADOS = "Correos No enviados, no se ha ingresado destinatario o zona: {0}";
        public readonly static string MENSAJE_CORREOS_ERROR = "Correos con error al enviar: {0}";
        public readonly static string MENSAJE_CORREOS_ERROR_VERIFICACION = "Correos No enviados por error al verificar si se envio anteriormente: {0}";
        public readonly static string MENSAJE_CORREOS_YA_ENVIADOS = "Correos No enviados, se enviaron anteriormente: {0}";


    }
}
