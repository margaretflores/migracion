using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using IBM.Data.DB2.iSeries;
using AccesoDatos;

namespace appLogica
{
    public class PEDIDOSEntitiesDB2 : IDisposable
    {

        internal BaseDatos DB2;

        public static string CadenaConexion { get; set; }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                    DB2.Desconectar();
                    DB2 = null;
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~PEDIDOSEntities() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region Constructor
        public PEDIDOSEntitiesDB2()
        {
            DB2 = new BaseDatos(CadenaConexion);
            DB2.Conectar();
        }

        #endregion

        #region Migracion DB2
        public List<appWcfService.PECAPE> MostrarPedidosApp2()
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_MostrarPedidosApp2", CommandType.StoredProcedure);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PECAPE>(cabeceraDataTable);
        }

        #endregion
    }
}
