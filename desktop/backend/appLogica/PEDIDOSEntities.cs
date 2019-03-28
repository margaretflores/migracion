using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IBM.Data.DB2.iSeries;

namespace appLogica
{
    class PEDIDOSEntitiesDB2 : IDisposable
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

        public List<appWcfService.TCLIE> BuscaCliente(string valbus)//probar
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_BUSCA_CLIENTE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDCP", iDB2DbType.iDB2Numeric, valbus);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.TCLIE>(cabeceraDataTable);
        }

        public List<appWcfService.PECAPE> MuestraPedidos()//OBTIENE TODOS LOS PEDIDOS EN GENERAL se n usa para muestra pedidos, BuscaPedidosFechas
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_BUSCAPEDIDOSFECHAS", CommandType.StoredProcedure);
            //DB2.AsignarParamProcAlmac("PDEPEIDCP", iDB2DbType.iDB2Numeric, valbus);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParsePECAPE<appWcfService.PECAPE>(cabeceraDataTable);
        }

        public List<appWcfService.PECAPE> MuestraPedidosXSerie(string serie)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_BUSCAPEDIDOSFECHASCAPESERI", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAPESERI", iDB2DbType.iDB2Char, serie);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParsePECAPE<appWcfService.PECAPE>(cabeceraDataTable);
        }

        public List<appWcfService.PECAPE> MuestraPedidosXEstado(decimal estado)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_BUSCAPEDIDOSFECHASCAPEIDES", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAPEIDES", iDB2DbType.iDB2Numeric, estado);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParsePECAPE<appWcfService.PECAPE>(cabeceraDataTable);
        }
        public List<appWcfService.PECAPE> MuestraPedidosXEstadoXSerie(decimal estado, string serie)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_BUSCAPEDIDOSFECHASCAPEIDES_CAPESERI", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAPEIDES", iDB2DbType.iDB2Numeric, estado);
            DB2.AsignarParamProcAlmac("PCAPESERI", iDB2DbType.iDB2Char, serie);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParsePECAPE<appWcfService.PECAPE>(cabeceraDataTable);
        }

        public List<appWcfService.PECAPE> MuestraPedidosEntreFechas(DateTime feini, DateTime fefin)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_BUSCAPEDIDOSENTREFECHAS", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAPEFECHINI", iDB2DbType.iDB2Date, feini);
            DB2.AsignarParamProcAlmac("PCAPEFECHFIN", iDB2DbType.iDB2Date, fefin);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParsePECAPE<appWcfService.PECAPE>(cabeceraDataTable);
        }

        public List<appWcfService.PECAPE> MuestraPedidosEtados_2_3()
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_BUSCAPEDIDOSFECHASCAPEIDES2O3", CommandType.StoredProcedure);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParsePECAPE<appWcfService.PECAPE>(cabeceraDataTable);
        }

        public List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> MuestraDetallePedidos(decimal CAPEIDCP)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_MOSTRAR_DETALLE_PEDIDOS", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDCP", iDB2DbType.iDB2Numeric, CAPEIDCP);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>(cabeceraDataTable);
        }

        public List<appWcfService.USP_OBTIENE_UBICACIONES_Result> MuestraUbicacionesArticulo(string articulo, string partida, decimal almacen)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_OBTIENE_UBICACIONES", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@PARTI", iDB2DbType.iDB2Char, articulo);
            DB2.AsignarParamProcAlmac("@PPART", iDB2DbType.iDB2Char, partida);
            DB2.AsignarParamProcAlmac("@PALMA", iDB2DbType.iDB2Numeric, almacen);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_UBICACIONES_Result>(cabeceraDataTable);
        }

        public List<appWcfService.USP_OBTIENE_BOLSA_Result> ObtieneBolsa(decimal detpedido, string coem)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_OBTIENE_BOLSA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@PIDDP", iDB2DbType.iDB2Numeric, detpedido);
            DB2.AsignarParamProcAlmac("@PCOEM", iDB2DbType.iDB2Char, coem);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_BOLSA_Result>(cabeceraDataTable);
        }

        public List<appWcfService.PEPASI> MuestraPasillos()
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_MUESTRAPASILLO", CommandType.StoredProcedure);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PEPASI>(cabeceraDataTable);
        }

        public List<appWcfService.PENIVE> MuestraNiveles(decimal idpasillo)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_MUESTRANIVELES", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PIDPASILLO", iDB2DbType.iDB2Numeric, idpasillo);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PENIVE>(cabeceraDataTable);
        }

        public List<appWcfService.PECOLU> MuestraColumnas(decimal idpasillo)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_MUESTRA_COLUMNA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PIDPASI", iDB2DbType.iDB2Numeric, idpasillo);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PECOLU>(cabeceraDataTable);
        }

        public List<appWcfService.PECASI> MuestraCasilleros(decimal idpasillo, decimal idcolumna, string nivel)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_MUESTRA_CASILLEROS ", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PIDPASI", iDB2DbType.iDB2Numeric, idpasillo);
            DB2.AsignarParamProcAlmac("PIDCOLU", iDB2DbType.iDB2Numeric, idcolumna);
            DB2.AsignarParamProcAlmac("PIDNIVE", iDB2DbType.iDB2Char, nivel);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PECASI>(cabeceraDataTable);
        }

        public List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> ObtienePedidoConsulta(decimal idpedido)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_OBTIENE_PEDIDO_CONSULTA ", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@ID_PED", iDB2DbType.iDB2Numeric, idpedido);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(cabeceraDataTable);
        }

        public List<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result> ReportePartidaAlmacen(string partida, decimal almacen)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_REPORTE_EMPAQUES_PARTIDA ", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@PPART", iDB2DbType.iDB2Char, partida);
            DB2.AsignarParamProcAlmac("@PALMA", iDB2DbType.iDB2Numeric, almacen);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result>(cabeceraDataTable);
        }

        public List<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result> ReporteMovimientosPartida(string partida, decimal almacen)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_REPORTE_MOVIMIENTOS_PARTIDA ", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@PPART", iDB2DbType.iDB2Char, partida);
            DB2.AsignarParamProcAlmac("@PALMA", iDB2DbType.iDB2Numeric, almacen);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result>(cabeceraDataTable);
        }

        public List<appWcfService.USP_REPORTE_MOVIMIENTOS_FECHAS_Result> ReporteMovimientosFechas(DateTime feini, DateTime fefin)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_REPORTE_MOVIMIENTOS_FECHAS ", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@PFECHINI", iDB2DbType.iDB2Date, feini);
            DB2.AsignarParamProcAlmac("@PFECHFIN", iDB2DbType.iDB2Date, fefin);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_REPORTE_MOVIMIENTOS_FECHAS_Result>(cabeceraDataTable);
        }

        public List<appWcfService.PECASI> DevuelveCasilleros()
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_DEVUELVE_CASILLEROS ", CommandType.StoredProcedure);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PECASI>(cabeceraDataTable);
        }

        public List<appWcfService.PENIVE> DevuelveNiveles()
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_DEVUELVE_NIVELES ", CommandType.StoredProcedure);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PENIVE>(cabeceraDataTable);
        }

        public List<appWcfService.PECOLU> DevuelveColumna()
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_DEVUELVE_COLUMNA", CommandType.StoredProcedure);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PECOLU>(cabeceraDataTable);
        }

        public List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> ObtienePedidoTracking(decimal idpedido)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_OBTIENE_PEDIDO_CONSULTA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@ID_PED", iDB2DbType.iDB2Numeric, idpedido);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(cabeceraDataTable);
        }

        public List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> EnviaCorreoNotificacionPedido(decimal idpedido)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_OBTIENE_PEDIDO_CONSULTA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@ID_PED", iDB2DbType.iDB2Numeric, idpedido);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(cabeceraDataTable);
        }

        public List<appWcfService.PEPARM> ObtieneParametros()
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PEPARM", CommandType.StoredProcedure);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PEPARM>(cabeceraDataTable);
        }

        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> ObtieneDetallePreparacionse(decimal iddetallepedido)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@PIDDP", iDB2DbType.iDB2Numeric, iddetallepedido);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>(cabeceraDataTable);
        }

        public List<appWcfService.PEDEPE> ValidaitemExcel(string arti, string part, decimal alma)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_VALIDA_ITEM_EXCEL", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPECOAR", iDB2DbType.iDB2Char, arti);
            DB2.AsignarParamProcAlmac("PDEPEPART", iDB2DbType.iDB2Char, part);
            DB2.AsignarParamProcAlmac("PDEPEALMA", iDB2DbType.iDB2Numeric, alma);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PEDEPE>(cabeceraDataTable);
        }

        public List<appWcfService.PEDEPE> ValidaPreparacion(decimal idped)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_VALIDA_PREPARACION", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDDP", iDB2DbType.iDB2Numeric, idped);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PEDEPE>(cabeceraDataTable);
        }

        public List<appWcfService.PEDEPE> DetallexCabe(decimal idcabped)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_OBTIENE_DETALLE_CABECERA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDCP", iDB2DbType.iDB2Numeric, idcabped);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.PEDEPE>(cabeceraDataTable);
        }

        public appWcfService.PECAPE PECAPE_Find(decimal CAPEIDCP)
        {
            appWcfService.PECAPE cape = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PECAPE_Find", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, CAPEIDCP);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PECAPE>(cabeceraDataTable);
            if (list.Count > 0)
            {
                cape = list[0];
            }
            return cape;
        }

        public void PECAPE_UPDATE_ESTADO_EMITIDO(decimal pCAPEIDCP,
           decimal? pCAPEIDES = null,
           string pCAPEUSEM = null,
           DateTime? pCAPEFHEM = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_ESTADO_EMITIDO", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEUSEM", iDB2DbType.iDB2Char, pCAPEUSEM);
            DB2.AsignarParamProcAlmac("CAPEFHEM", iDB2DbType.iDB2TimeStamp, pCAPEFHEM);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_ESTADO_ENPREPARACION(decimal pCAPEIDCP,
           decimal? pCAPEIDES = null,
           string pCAPEUSIP = null,
          DateTime? pCAPEFHIP = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_ESTADO_ENPREPARACION", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEUSIP", iDB2DbType.iDB2Char, pCAPEUSIP);
            DB2.AsignarParamProcAlmac("CAPEFHIP", iDB2DbType.iDB2TimeStamp, pCAPEFHIP);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_ESTADO_ESP_APROB_REABIERTO(decimal pCAPEIDCP,
          string pCAPEUSMO,
          DateTime? pCAPEFEMO,
           string pCAPEUSAP,
          DateTime? pCAPEFEAP,
          decimal pCAPEIDES
          )
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_ESTADO_ESP_APROB_REABIERTO", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEUSAP", iDB2DbType.iDB2Char, pCAPEUSAP);
            DB2.AsignarParamProcAlmac("CAPEFEAP", iDB2DbType.iDB2TimeStamp, pCAPEFEAP);
            DB2.AsignarParamProcAlmac("CAPEUSMO", iDB2DbType.iDB2Char, pCAPEUSMO);
            DB2.AsignarParamProcAlmac("CAPEFEMO", iDB2DbType.iDB2TimeStamp, pCAPEFEMO);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_ESTADO_ESP_APROBACION(decimal pCAPEIDCP,
           decimal? pCAPEIDES = null,
           string pCAPEUSFP = null,
          DateTime? pCAPEFHFP = null,
           decimal? pCAPENUBU = null,
          decimal? pCAPETADE = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_ESTADO_ESP_APROBACION", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEFHFP", iDB2DbType.iDB2TimeStamp, pCAPEFHFP);
            DB2.AsignarParamProcAlmac("CAPEUSFP", iDB2DbType.iDB2Char, pCAPEUSFP);
            DB2.AsignarParamProcAlmac("CAPENUBU", iDB2DbType.iDB2Numeric, pCAPENUBU);
            DB2.AsignarParamProcAlmac("CAPETADE", iDB2DbType.iDB2Numeric, pCAPETADE);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_ESTADO_APROBADO(decimal pCAPEIDCP,
           decimal? pCAPEIDES = null,
           string pCAPEUSAP = null,
          DateTime? pCAPEFEAP = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_ESTADO_APROBADO", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEUSAP", iDB2DbType.iDB2Char, pCAPEUSAP);
            DB2.AsignarParamProcAlmac("CAPEFEAP", iDB2DbType.iDB2TimeStamp, pCAPEFEAP);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_ESTADO_ANULADO(decimal pCAPEIDCP,
           decimal? pCAPEIDES = null,
          string pCAPEUSMO = null,
          DateTime? pCAPEFEMO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_ESTADO_ANULADO", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEUSMO", iDB2DbType.iDB2Char, pCAPEUSMO);
            DB2.AsignarParamProcAlmac("CAPEFEMO", iDB2DbType.iDB2TimeStamp, pCAPEFEMO);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_GENERA_PRE_GUIA_1(decimal pCAPEIDCP,
         decimal? pCAPENUBU = null,
         decimal? pCAPETIPO = null,
         decimal? pCAPETADE = null,
         decimal? pCAPEIDTD = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_GENERA_PRE_GUIA_1", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPENUBU", iDB2DbType.iDB2Numeric, pCAPENUBU);
            DB2.AsignarParamProcAlmac("CAPETIPO", iDB2DbType.iDB2Numeric, pCAPETIPO);
            DB2.AsignarParamProcAlmac("CAPETADE", iDB2DbType.iDB2Numeric, pCAPETADE);
            DB2.AsignarParamProcAlmac("CAPEIDTD", iDB2DbType.iDB2Numeric, pCAPEIDTD);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_GENERA_PRE_GUIA_2(decimal pCAPEIDCP,
         decimal? pCAPEIDES = null,
         string pCAPEUSAP = null,
          DateTime? pCAPEFEAP = null,
         decimal? pCAPENUBU = null,
         decimal? pCAPETIPO = null,
         decimal? pCAPETADE = null,
         decimal? pCAPEIDTD = null,
          decimal? pCAPEDOCO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_GENERA_PRE_GUIA2", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEUSAP", iDB2DbType.iDB2Char, pCAPEUSAP);
            DB2.AsignarParamProcAlmac("CAPEFEAP", iDB2DbType.iDB2TimeStamp, pCAPEFEAP);
            DB2.AsignarParamProcAlmac("CAPENUBU", iDB2DbType.iDB2Numeric, pCAPENUBU);
            DB2.AsignarParamProcAlmac("CAPETIPO", iDB2DbType.iDB2Numeric, pCAPETIPO);
            DB2.AsignarParamProcAlmac("CAPETADE", iDB2DbType.iDB2Numeric, pCAPETADE);
            DB2.AsignarParamProcAlmac("CAPEIDTD", iDB2DbType.iDB2Numeric, pCAPEIDTD);
            DB2.AsignarParamProcAlmac("CAPEDOCO", iDB2DbType.iDB2Numeric, pCAPEDOCO);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_NUM_PRIO(decimal pCAPEIDCP,
           decimal? pCAPEPRIO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_NUM_PRIO", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEPRIO", iDB2DbType.iDB2Numeric, pCAPEPRIO);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_ES_PRIO(decimal pCAPEIDCP,
           decimal? pCAPEEPRI = null,
          DateTime? pCAPEFEPR = null,
          string pCAPEUSPR = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_ES_PRIO", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEEPRI", iDB2DbType.iDB2Numeric, pCAPEEPRI);
            DB2.AsignarParamProcAlmac("CAPEFEPR", iDB2DbType.iDB2TimeStamp, pCAPEFEPR);
            DB2.AsignarParamProcAlmac("CAPEUSPR", iDB2DbType.iDB2Char, pCAPEUSPR);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_NUME(decimal pCAPEIDCP,
          decimal? pCAPENUME)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_NUME", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("PCAPENUME", iDB2DbType.iDB2Numeric, pCAPENUME);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECAPE_UPDATE_GUARDA_PED(decimal pCAPEIDCP,
          string pCAPEIDCL = null,
          DateTime? pCAPEFECH = null,
          string pCAPEDIRE = null,
          decimal? pCAPEIDES = null,
          string pCAPEEMAI = null,
          string pCAPENOTI = null,
          string pCAPENOTG = null,
          decimal? pCAPETIPO = null,
          string pCAPEUSMO = null,
          DateTime? pCAPEFEMO = null,
          decimal? pCAPEIDTD = null,
          string pCAPEDEST = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECAPE_UPDATE_GUARDA_PED", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PCAPEIDCP", iDB2DbType.iDB2Numeric, pCAPEIDCP);
            DB2.AsignarParamProcAlmac("CAPEIDCL", iDB2DbType.iDB2Char, pCAPEIDCL);
            DB2.AsignarParamProcAlmac("CAPEFECH", iDB2DbType.iDB2Date, pCAPEFECH);
            DB2.AsignarParamProcAlmac("CAPEDIRE", iDB2DbType.iDB2VarChar, pCAPEDIRE);
            DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            DB2.AsignarParamProcAlmac("CAPEEMAI", iDB2DbType.iDB2VarChar, pCAPEEMAI);
            DB2.AsignarParamProcAlmac("CAPENOTI", iDB2DbType.iDB2Char, pCAPENOTI);
            DB2.AsignarParamProcAlmac("CAPENOTG", iDB2DbType.iDB2VarChar, pCAPENOTG);
            DB2.AsignarParamProcAlmac("CAPETIPO", iDB2DbType.iDB2Numeric, pCAPETIPO);
            DB2.AsignarParamProcAlmac("CAPEUSMO", iDB2DbType.iDB2Char, pCAPEUSMO);
            DB2.AsignarParamProcAlmac("CAPEFEMO", iDB2DbType.iDB2TimeStamp, pCAPEFEMO);
            DB2.AsignarParamProcAlmac("CAPEIDTD", iDB2DbType.iDB2Numeric, pCAPEIDTD);
            DB2.AsignarParamProcAlmac("CAPEDEST", iDB2DbType.iDB2VarChar, pCAPEDEST);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public appWcfService.PEDEPE PEDEPE_Find(decimal? DEPEIDDP)
        {
            appWcfService.PEDEPE depe = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PEDEPE_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDDP", iDB2DbType.iDB2Numeric, DEPEIDDP);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PEDEPE>(cabeceraDataTable);
            if (list.Count > 0)
            {
                depe = list[0];
            }
            return depe;
        }

        //public void PEDEPE_UPDATE(decimal pDEPEIDDP,
        // decimal? pDEPEIDCP  = null,
        // string pDEPECOAR  = null,
        // string pDEPEPART  = null,
        // string pDEPECONT  = null,
        // decimal? pDEPEALMA  = null,
        // decimal? pDEPECASO  = null,
        // decimal? pDEPEPESO  = null,
        // decimal? pDEPECAAT  = null,
        // decimal? pDEPEPEAT  = null,
        // decimal? pDEPEPERE  = null,
        // decimal? pDEPETADE  = null,
        // decimal? pDEPEPEBR  = null,
        // decimal? pDEPESTOC  = null,
        // decimal? pDEPEESTA  = null,
        // decimal? pDEPEDISP  = null,
        // string pDEPEDSAR  = null,
        // decimal? pDEPENUBU  = null,
        // string pDEPEUSMO = null,
        // DateTime? pDEPEFEMO = null,
        // decimal? pDEPESERS  = null,
        // decimal? pDEPESECR = null,
        // decimal? pDEPESECU = null)
        //{
        //    DB2.CrearComando("PRPEDAT.USP_PEDEPE_UPDATE", CommandType.StoredProcedure);
        //    DB2.AsignarParamProcAlmac("PDEPEIDDP", iDB2DbType.iDB2Numeric, pDEPEIDDP);
        //    if (pDEPEIDCP == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEIDCP", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEIDCP", iDB2DbType.iDB2Numeric, pDEPEIDCP);
        //    }
        //    if (pDEPECOAR == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECOAR", iDB2DbType.iDB2Char, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECOAR", iDB2DbType.iDB2Char, pDEPECOAR);
        //    }
        //    if (pDEPEPART == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPART", iDB2DbType.iDB2Char, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPART", iDB2DbType.iDB2Char, pDEPEPART);
        //    }
        //    if (pDEPECONT == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECONT", iDB2DbType.iDB2Char, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECONT", iDB2DbType.iDB2Char, pDEPECONT);
        //    }
        //    if (pDEPEALMA == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEALMA", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEALMA", iDB2DbType.iDB2Numeric, pDEPEALMA);
        //    }
        //    if (pDEPECASO == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECASO", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECASO", iDB2DbType.iDB2Numeric, pDEPECASO);
        //    }
        //    if (pDEPEPESO == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPESO", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPESO", iDB2DbType.iDB2Numeric, pDEPEPESO);
        //    }
        //    if (pDEPECAAT == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECAAT", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPECAAT", iDB2DbType.iDB2Numeric, pDEPECAAT);
        //    }
        //    if (pDEPEPEAT == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPEAT", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPEAT", iDB2DbType.iDB2Numeric, pDEPEPEAT);
        //    }
        //    if (pDEPEPERE == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPERE", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPERE", iDB2DbType.iDB2Numeric, pDEPEPERE);
        //    }
        //    if (pDEPETADE == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPETADE", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPETADE", iDB2DbType.iDB2Numeric, pDEPETADE);
        //    }
        //    if (pDEPEPEBR == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPEBR", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEPEBR", iDB2DbType.iDB2Numeric, pDEPEPEBR);
        //    }
        //    if (pDEPESTOC == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESTOC", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESTOC", iDB2DbType.iDB2Numeric, pDEPESTOC);
        //    }
        //    if (pDEPEESTA == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEESTA", iDB2DbType.iDB2Numeric, pDEPEESTA);
        //    }
        //    if (pDEPEDISP == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEDISP", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEDISP", iDB2DbType.iDB2Numeric, pDEPEDISP);
        //    }
        //    if (pDEPEDSAR == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEDSAR", iDB2DbType.iDB2VarChar, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEDSAR", iDB2DbType.iDB2VarChar, pDEPEDSAR);
        //    }
        //    if (pDEPENUBU == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPENUBU", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPENUBU", iDB2DbType.iDB2Numeric, pDEPENUBU);
        //    }

        //    if (pDEPEUSMO == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEUSMO", iDB2DbType.iDB2Char, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEUSMO", iDB2DbType.iDB2Char, pDEPEUSMO);
        //    }
        //    if (pDEPEFEMO == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPEFEMO", iDB2DbType.iDB2TimeStamp, pDEPEFEMO);
        //    }
        //    if (pDEPESERS == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESERS", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESERS", iDB2DbType.iDB2Numeric, pDEPESERS);
        //    }
        //    if (pDEPESECR == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESECR", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESECR", iDB2DbType.iDB2Numeric, pDEPESECR);
        //    }
        //    if (pDEPESECU == null)
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESECU", iDB2DbType.iDB2Numeric, DBNull.Value);
        //    }
        //    else
        //    {
        //        DB2.AsignarParamProcAlmac("DEPESECU", iDB2DbType.iDB2Numeric, pDEPESECU);
        //    }
        //    DB2.EjecutarProcedimientoAlmacenado();
        //}

        public void PEDEPE_UPDATE(decimal pDEPEIDDP,
           string pDEPEDSAR = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PEDEPE_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDDP", iDB2DbType.iDB2Numeric, pDEPEIDDP);
            if (pDEPEDSAR == null)
            {
                DB2.AsignarParamProcAlmac("DEPEDSAR", iDB2DbType.iDB2VarChar, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEDSAR", iDB2DbType.iDB2VarChar, pDEPEDSAR);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PEDEPE_UPDATE_GUARDA_PED(decimal pDEPEIDDP,
           decimal? pDEPEIDCP = null,
           string pDEPECOAR = null,
           string pDEPEPART = null,
           string pDEPECONT = null,
           decimal? pDEPEALMA = null,
           decimal? pDEPECASO = null,
           decimal? pDEPEPESO = null,
           decimal? pDEPEDISP = null,
           string pDEPEDSAR = null,
           string pDEPEUSMO = null,
           DateTime? pDEPEFEMO = null,
           decimal? pDEPESERS = null,
           decimal? pDEPESECU = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PEDEPE_UPDATE_GUARDA_PED", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDDP", iDB2DbType.iDB2Numeric, pDEPEIDDP);

            DB2.AsignarParamProcAlmac("DEPEIDCP", iDB2DbType.iDB2Numeric, pDEPEIDCP);

            DB2.AsignarParamProcAlmac("DEPECOAR", iDB2DbType.iDB2Char, pDEPECOAR);

            DB2.AsignarParamProcAlmac("DEPEPART", iDB2DbType.iDB2Char, pDEPEPART);

            DB2.AsignarParamProcAlmac("DEPECONT", iDB2DbType.iDB2Char, pDEPECONT);

            DB2.AsignarParamProcAlmac("DEPEALMA", iDB2DbType.iDB2Numeric, pDEPEALMA);

            DB2.AsignarParamProcAlmac("DEPECASO", iDB2DbType.iDB2Numeric, pDEPECASO);

            DB2.AsignarParamProcAlmac("DEPEPESO", iDB2DbType.iDB2Numeric, pDEPEPESO);

            DB2.AsignarParamProcAlmac("DEPEDISP", iDB2DbType.iDB2Numeric, pDEPEDISP);

            DB2.AsignarParamProcAlmac("DEPEDSAR", iDB2DbType.iDB2VarChar, pDEPEDSAR);

            DB2.AsignarParamProcAlmac("DEPEUSMO", iDB2DbType.iDB2Char, pDEPEUSMO);

            DB2.AsignarParamProcAlmac("DEPEFEMO", iDB2DbType.iDB2TimeStamp, pDEPEFEMO);

            DB2.AsignarParamProcAlmac("DEPESERS", iDB2DbType.iDB2Numeric, pDEPESERS);

            DB2.AsignarParamProcAlmac("DEPESECU", iDB2DbType.iDB2Numeric, pDEPESECU);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PEDEPE_UPDATE_GUARDA_ELIMINA_BOLSA(decimal pDEPEIDDP,
            decimal? pDEPEIDCP = null,
           decimal? pDEPECAAT = null,
           decimal? pDEPEPEAT = null,
           decimal? pDEPEPERE = null,
           decimal? pDEPETADE = null,
           decimal? pDEPEPEBR = null,
           string pDEPEUSMO = null,
           DateTime? pDEPEFEMO = null,
           decimal? pDEPESTOC = null)
        {
            DB2.CrearComando("PRPEDAT.PEDEPE_UPDATE_GUARDA_ELIMINA_BOLSA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDDP", iDB2DbType.iDB2Numeric, pDEPEIDDP);

            //DB2.AsignarParamProcAlmac("DEPEIDCP", iDB2DbType.iDB2Numeric, pDEPEIDCP);

            DB2.AsignarParamProcAlmac("DEPECAAT", iDB2DbType.iDB2Numeric, pDEPECAAT);

            DB2.AsignarParamProcAlmac("DEPEPEAT", iDB2DbType.iDB2Numeric, pDEPEPEAT);

            DB2.AsignarParamProcAlmac("DEPEPERE", iDB2DbType.iDB2Numeric, pDEPEPERE);

            DB2.AsignarParamProcAlmac("DEPETADE", iDB2DbType.iDB2Numeric, pDEPETADE);

            DB2.AsignarParamProcAlmac("DEPEPEBR", iDB2DbType.iDB2Numeric, pDEPEPEBR);

            DB2.AsignarParamProcAlmac("DEPESTOC", iDB2DbType.iDB2Numeric, pDEPESTOC);

            DB2.AsignarParamProcAlmac("DEPEUSMO", iDB2DbType.iDB2Char, pDEPEUSMO);

            DB2.AsignarParamProcAlmac("DEPEFEMO", iDB2DbType.iDB2TimeStamp, pDEPEFEMO);


            DB2.EjecutarProcedimientoAlmacenado();
        }


        public appWcfService.PECASI PECASI_Find(string pCASICOCA)
        {
            appWcfService.PECASI casi = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PECASI_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCASICOCA", iDB2DbType.iDB2Char, pCASICOCA);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PECASI>(cabeceraDataTable);
            if (list.Count > 0)
            {
                casi = list[0];
            }
            return casi;
        }

        public void PECASI_UPDATE(string pCASICOCA,
        decimal? pCASIIDPA = null,
        string pCASIIDNI = null,
        decimal? pCASIIDCO = null,
        decimal? pCASICAPA = null,
        decimal? pCASIESTA = null,
        decimal? pCASIALTU = null,
        decimal? pCASILARG = null,
        decimal? pCASIANCH = null,
        //string pCASIUSCR = null,
        //DateTime? pCASIFECR = null,
        string pCASIUSMO = null,
        DateTime? pCASIFEMO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECASI_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("CASICOCA", iDB2DbType.iDB2Char, pCASICOCA);
            if (pCASIIDPA == null)
            {
                DB2.AsignarParamProcAlmac("CASIIDPA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIIDPA", iDB2DbType.iDB2Numeric, pCASIIDPA);
            }
            if (pCASIIDNI == null)
            {
                DB2.AsignarParamProcAlmac("CASIIDNI", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIIDNI", iDB2DbType.iDB2Char, pCASIIDNI);
            }
            if (pCASIIDCO == null)
            {
                DB2.AsignarParamProcAlmac("CASIIDCO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIIDCO", iDB2DbType.iDB2Numeric, pCASIIDCO);
            }
            if (pCASICAPA == null)
            {
                DB2.AsignarParamProcAlmac("CASICAPA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASICAPA", iDB2DbType.iDB2Numeric, pCASICAPA);
            }
            if (pCASIESTA == null)
            {
                DB2.AsignarParamProcAlmac("CASIESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIESTA", iDB2DbType.iDB2Numeric, pCASIESTA);
            }
            if (pCASIALTU == null)
            {
                DB2.AsignarParamProcAlmac("CASIALTU", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIALTU", iDB2DbType.iDB2Numeric, pCASIALTU);
            }
            if (pCASILARG == null)
            {
                DB2.AsignarParamProcAlmac("CASILARG", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASILARG", iDB2DbType.iDB2Numeric, pCASILARG);
            }
            if (pCASIANCH == null)
            {
                DB2.AsignarParamProcAlmac("CASIANCH", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIANCH", iDB2DbType.iDB2Numeric, pCASIANCH);
            }
            //if (pCASIUSCR == null)
            //{
            //    DB2.AsignarParamProcAlmac("CASIUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("CASIUSCR", iDB2DbType.iDB2Char, pCASIUSCR);
            //}
            //if (pCASIFECR == null)
            //{
            //    DB2.AsignarParamProcAlmac("CASIFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("CASIFECR", iDB2DbType.iDB2TimeStamp, pCASIFECR);
            //}
            if (pCASIUSMO == null)
            {
                DB2.AsignarParamProcAlmac("CASIUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIUSMO", iDB2DbType.iDB2Char, pCASIUSMO);
            }
            if (pCASIFEMO == null)
            {
                DB2.AsignarParamProcAlmac("CASIFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIFEMO", iDB2DbType.iDB2TimeStamp, pCASIFEMO);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public appWcfService.PECOLU PECOLU_Find(decimal pcoluidco, decimal pcoluidpa)
        {
            appWcfService.PECOLU colu = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PECOLU_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCOLUIDCO", iDB2DbType.iDB2Numeric, pcoluidco);
            DB2.AsignarParamProcAlmac("PCOLUIDPA", iDB2DbType.iDB2Numeric, pcoluidpa);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PECOLU>(cabeceraDataTable);
            if (list.Count > 0)
            {
                colu = list[0];
            }
            return colu;
        }


        public void PECOLU_UPDATE(decimal pCOLUIDCO,
        decimal pCOLUIDPA,
        decimal? pCOLUESTA = null,
        //string pCOLUUSCR = null,
        //DateTime? pCOLUFECR = null,
        string pCOLUUSMO = null,
        DateTime? pCOLUFEMO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PECOLU_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("COLUIDCO", iDB2DbType.iDB2Numeric, pCOLUIDCO);
            DB2.AsignarParamProcAlmac("COLUIDPA", iDB2DbType.iDB2Numeric, pCOLUIDPA);

            if (pCOLUESTA == null)
            {
                DB2.AsignarParamProcAlmac("COLUESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("COLUESTA", iDB2DbType.iDB2Numeric, pCOLUESTA);
            }
            //if (pCOLUUSCR == null)
            //{
            //    DB2.AsignarParamProcAlmac("COLUUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("COLUUSCR", iDB2DbType.iDB2Char, pCOLUUSCR);
            //}
            //if (pCOLUFECR == null)
            //{
            //    DB2.AsignarParamProcAlmac("COLUFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("COLUFECR", iDB2DbType.iDB2TimeStamp, pCOLUFECR);
            //}
            if (pCOLUUSMO == null)
            {
                DB2.AsignarParamProcAlmac("COLUUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("COLUUSMO", iDB2DbType.iDB2Char, pCOLUUSMO);
            }
            if (pCOLUFEMO == null)
            {
                DB2.AsignarParamProcAlmac("COLUFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("COLUFEMO", iDB2DbType.iDB2TimeStamp, pCOLUFEMO);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }


        public appWcfService.PENIVE PENIVE_Find(string pniveidni, decimal pniveidpa)
        {
            appWcfService.PENIVE nive = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PENIVE_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PNIVEIDNI", iDB2DbType.iDB2Char, pniveidni);
            DB2.AsignarParamProcAlmac("PNIVEIDPA", iDB2DbType.iDB2Numeric, pniveidpa);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PENIVE>(cabeceraDataTable);
            if (list.Count > 0)
            {
                nive = list[0];
            }
            return nive;
        }

        public void PENIVE_UPDATE(string pNIVEIDNI,
        decimal pNIVEIDPA,
        decimal? pNIVEESTA = null,
        //string pNIVEUSCR = null,
        //DateTime? pNIVEFECR = null,
        string pNIVEUSMO = null,
        DateTime? pNIVEFEMO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PENIVE_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("NIVEIDNI", iDB2DbType.iDB2Char, pNIVEIDNI);
            DB2.AsignarParamProcAlmac("NIVEIDPA", iDB2DbType.iDB2Numeric, pNIVEIDPA);
            if (pNIVEESTA == null)
            {
                DB2.AsignarParamProcAlmac("NIVEESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("NIVEESTA", iDB2DbType.iDB2Numeric, pNIVEESTA);
            }
            //if (pNIVEUSCR == null)
            //{
            //    DB2.AsignarParamProcAlmac("NIVEUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("NIVEUSCR", iDB2DbType.iDB2Char, pNIVEUSCR);
            //}
            //if (pNIVEFECR == null)
            //{
            //    DB2.AsignarParamProcAlmac("NIVEFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("NIVEFECR", iDB2DbType.iDB2TimeStamp, pNIVEFECR);
            //}
            if (pNIVEUSMO == null)
            {
                DB2.AsignarParamProcAlmac("NIVEUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("NIVEUSMO", iDB2DbType.iDB2Char, pNIVEUSMO);
            }
            if (pNIVEFEMO == null)
            {
                DB2.AsignarParamProcAlmac("NIVEFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("NIVEFEMO", iDB2DbType.iDB2TimeStamp, pNIVEFEMO);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public appWcfService.PEPASI PEPASI_Find(decimal pniveidpa)
        {
            appWcfService.PEPASI pasi = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PEPASI_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("pPASIIDPA", iDB2DbType.iDB2Numeric, pniveidpa);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PEPASI>(cabeceraDataTable);
            if (list.Count > 0)
            {
                pasi = list[0];
            }
            return pasi;
        }

        public void PEPASI_UPDATE(decimal pPASIIDPA,
        decimal? pPASIESTA = null,
        //string pPASIUSCR = null,
        //DateTime? pPASIFECR = null,
        string pPASIUSMO = null,
        DateTime? pPASIFEMO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PEPASI_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PASIIDPA", iDB2DbType.iDB2Numeric, pPASIIDPA);
            if (pPASIESTA == null)
            {
                DB2.AsignarParamProcAlmac("PASIESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PASIESTA", iDB2DbType.iDB2Numeric, pPASIESTA);
            }
            //if (pPASIUSCR == null)
            //{
            //    DB2.AsignarParamProcAlmac("PASIUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PASIUSCR", iDB2DbType.iDB2Char, pPASIUSCR);
            //}
            //if (pPASIFECR == null)
            //{
            //    DB2.AsignarParamProcAlmac("PASIFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PASIFECR", iDB2DbType.iDB2TimeStamp, pPASIFECR);
            //}
            if (pPASIUSMO == null)
            {
                DB2.AsignarParamProcAlmac("PASIUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PASIUSMO", iDB2DbType.iDB2Char, pPASIUSMO);
            }
            if (pPASIFEMO == null)
            {
                DB2.AsignarParamProcAlmac("PASIFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PASIFEMO", iDB2DbType.iDB2TimeStamp, pPASIFEMO);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PENIVE_DELETE(string pniveidni, decimal pniveidpa)
        {
            DB2.CrearComando("PRPEDAT.USP_ELIMINAR_NIVEL", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PNIVEIDNI", iDB2DbType.iDB2Char, pniveidni);
            DB2.AsignarParamProcAlmac("PNIVEIDPA", iDB2DbType.iDB2Numeric, pniveidpa);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECOLU_DELETE(decimal pcoluidco, decimal pcoluidpa)
        {
            DB2.CrearComando("PRPEDAT.USP_ELIMINAR_COLUMNA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCOLUIDCO", iDB2DbType.iDB2Numeric, pcoluidco);
            DB2.AsignarParamProcAlmac("PCOLUIDPA", iDB2DbType.iDB2Numeric, pcoluidpa);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PEPASI_DELETE(decimal ppasiidpa)
        {
            DB2.CrearComando("PRPEDAT.USP_ELIMINAR_PASILLO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PPASIIDPA", iDB2DbType.iDB2Numeric, ppasiidpa);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PEDEPE_DELETE(decimal pdepeiddp, decimal pdepeidcp)
        {
            DB2.CrearComando("PRPEDAT.USP_ELIMINAR_DETALLE_PEDIDO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEPEIDDP", iDB2DbType.iDB2Numeric, pdepeiddp);
            DB2.AsignarParamProcAlmac("PDEPEIDCP", iDB2DbType.iDB2Numeric, pdepeidcp);
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PEPASI_INSERT(decimal pPASIIDPA,
        decimal? pPASIESTA = null,
        string pPASIUSCR = null,
        DateTime? pPASIFECR = null,
        string pPASIUSMO = null,
        DateTime? pPASIFEMO = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PE_INSERTA_PASILLO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PASIIDPA", iDB2DbType.iDB2Numeric, pPASIIDPA);
            if (pPASIESTA == null)
            {
                DB2.AsignarParamProcAlmac("PASIESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PASIESTA", iDB2DbType.iDB2Numeric, pPASIESTA);
            }
            if (pPASIUSCR == null)
            {
                DB2.AsignarParamProcAlmac("PASIUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PASIUSCR", iDB2DbType.iDB2Char, pPASIUSCR);
            }
            if (pPASIFECR == null)
            {
                DB2.AsignarParamProcAlmac("PASIFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PASIFECR", iDB2DbType.iDB2TimeStamp, pPASIFECR);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PENIVE_INSERT(string pNIVEIDNI,
        decimal pNIVEIDPA,
        decimal? pNIVEESTA = null,
        string pNIVEUSCR = null,
        DateTime? pNIVEFECR = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PE_INSERTA_NIVEL", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("NIVEIDNI", iDB2DbType.iDB2Char, pNIVEIDNI);
            DB2.AsignarParamProcAlmac("NIVEIDPA", iDB2DbType.iDB2Numeric, pNIVEIDPA);
            if (pNIVEESTA == null)
            {
                DB2.AsignarParamProcAlmac("NIVEESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("NIVEESTA", iDB2DbType.iDB2Numeric, pNIVEESTA);
            }
            if (pNIVEUSCR == null)
            {
                DB2.AsignarParamProcAlmac("NIVEUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("NIVEUSCR", iDB2DbType.iDB2Char, pNIVEUSCR);
            }
            if (pNIVEFECR == null)
            {
                DB2.AsignarParamProcAlmac("NIVEFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("NIVEFECR", iDB2DbType.iDB2TimeStamp, pNIVEFECR);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECOLU_INSERT(decimal pCOLUIDCO,
       decimal pCOLUIDPA,
       decimal? pCOLUESTA = null,
       string pCOLUUSCR = null,
       DateTime? pCOLUFECR = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PE_INSERTA_COLUMNA", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("COLUIDCO", iDB2DbType.iDB2Numeric, pCOLUIDCO);
            DB2.AsignarParamProcAlmac("COLUIDPA", iDB2DbType.iDB2Numeric, pCOLUIDPA);

            if (pCOLUESTA == null)
            {
                DB2.AsignarParamProcAlmac("COLUESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("COLUESTA", iDB2DbType.iDB2Numeric, pCOLUESTA);
            }
            if (pCOLUUSCR == null)
            {
                DB2.AsignarParamProcAlmac("COLUUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("COLUUSCR", iDB2DbType.iDB2Char, pCOLUUSCR);
            }
            if (pCOLUFECR == null)
            {
                DB2.AsignarParamProcAlmac("COLUFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("COLUFECR", iDB2DbType.iDB2TimeStamp, pCOLUFECR);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PECASI_INSERT(string pCASICOCA,
        decimal? pCASIIDPA = null,
        string pCASIIDNI = null,
        decimal? pCASIIDCO = null,
        decimal? pCASICAPA = null,
        decimal? pCASIESTA = null,
        decimal? pCASIALTU = null,
        decimal? pCASILARG = null,
        decimal? pCASIANCH = null,
        string pCASIUSCR = null,
        DateTime? pCASIFECR = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PE_INSERTA_CASILLERO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("CASICOCA", iDB2DbType.iDB2Char, pCASICOCA);
            if (pCASIIDPA == null)
            {
                DB2.AsignarParamProcAlmac("CASIIDPA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIIDPA", iDB2DbType.iDB2Numeric, pCASIIDPA);
            }
            if (pCASIIDNI == null)
            {
                DB2.AsignarParamProcAlmac("CASIIDNI", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIIDNI", iDB2DbType.iDB2Char, pCASIIDNI);
            }
            if (pCASIIDCO == null)
            {
                DB2.AsignarParamProcAlmac("CASIIDCO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIIDCO", iDB2DbType.iDB2Numeric, pCASIIDCO);
            }
            if (pCASICAPA == null)
            {
                DB2.AsignarParamProcAlmac("CASICAPA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASICAPA", iDB2DbType.iDB2Numeric, pCASICAPA);
            }
            if (pCASIESTA == null)
            {
                DB2.AsignarParamProcAlmac("CASIESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIESTA", iDB2DbType.iDB2Numeric, pCASIESTA);
            }
            if (pCASIALTU == null)
            {
                DB2.AsignarParamProcAlmac("CASIALTU", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIALTU", iDB2DbType.iDB2Numeric, pCASIALTU);
            }
            if (pCASILARG == null)
            {
                DB2.AsignarParamProcAlmac("CASILARG", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASILARG", iDB2DbType.iDB2Numeric, pCASILARG);
            }
            if (pCASIANCH == null)
            {
                DB2.AsignarParamProcAlmac("CASIANCH", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIANCH", iDB2DbType.iDB2Numeric, pCASIANCH);
            }
            if (pCASIUSCR == null)
            {
                DB2.AsignarParamProcAlmac("CASIUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIUSCR", iDB2DbType.iDB2Char, pCASIUSCR);
            }
            if (pCASIFECR == null)
            {
                DB2.AsignarParamProcAlmac("CASIFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CASIFECR", iDB2DbType.iDB2TimeStamp, pCASIFECR);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public decimal PECAPE_INSERT(string pCAPESERI = null,
           decimal? pCAPENUME = null,
           string pCAPEIDCL = null,
           DateTime? pCAPEFECH = null,
           string pCAPEDIRE = null,
           decimal? pCAPEIDES = null,
           string pCAPEEMAI = null,
           string pCAPENOTI = null,
           string pCAPENOTG = null,
           decimal? pCAPETIPO = null,
           string pCAPEUSCR = null,
           DateTime? pCAPEFECR = null,
           decimal? pCAPEIDTD = null,
           string pCAPEDEST = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PE_PECAPE_INSERT", CommandType.StoredProcedure);
            if (pCAPESERI == null)
            {
                DB2.AsignarParamProcAlmac("CAPESERI", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPESERI", iDB2DbType.iDB2Char, pCAPESERI);
            }
            if (pCAPENUME == null)
            {
                DB2.AsignarParamProcAlmac("CAPENUME", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPENUME", iDB2DbType.iDB2Numeric, pCAPENUME);
            }
            if (pCAPEIDCL == null)
            {
                DB2.AsignarParamProcAlmac("CAPEIDCL", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEIDCL", iDB2DbType.iDB2Char, pCAPEIDCL);
            }
            if (pCAPEFECH == null)
            {
                DB2.AsignarParamProcAlmac("CAPEFECH", iDB2DbType.iDB2Date, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEFECH", iDB2DbType.iDB2Date, pCAPEFECH);
            }
            if (pCAPEDIRE == null)
            {
                DB2.AsignarParamProcAlmac("CAPEDIRE", iDB2DbType.iDB2VarChar, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEDIRE", iDB2DbType.iDB2VarChar, pCAPEDIRE);
            }
            if (pCAPEIDES == null)
            {
                DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEIDES", iDB2DbType.iDB2Numeric, pCAPEIDES);
            }
            if (pCAPEEMAI == null)
            {
                DB2.AsignarParamProcAlmac("CAPEEMAI", iDB2DbType.iDB2VarChar, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEEMAI", iDB2DbType.iDB2VarChar, pCAPEEMAI);
            }
            if (pCAPENOTI == null)
            {
                DB2.AsignarParamProcAlmac("CAPENOTI", iDB2DbType.iDB2VarChar, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPENOTI", iDB2DbType.iDB2VarChar, pCAPENOTI);
            }
            if (pCAPENOTG == null)
            {
                DB2.AsignarParamProcAlmac("CAPENOTG", iDB2DbType.iDB2VarChar, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPENOTG", iDB2DbType.iDB2VarChar, pCAPENOTG);
            }
            if (pCAPETIPO == null)
            {
                DB2.AsignarParamProcAlmac("CAPETIPO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPETIPO", iDB2DbType.iDB2Numeric, pCAPETIPO);
            }
            if (pCAPEUSCR == null)
            {
                DB2.AsignarParamProcAlmac("CAPEUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEUSCR", iDB2DbType.iDB2Char, pCAPEUSCR);
            }
            if (pCAPEFECR == null)
            {
                DB2.AsignarParamProcAlmac("CAPEFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEFECR", iDB2DbType.iDB2TimeStamp, pCAPEFECR);
            }
            if (pCAPEIDTD == null)
            {
                DB2.AsignarParamProcAlmac("CAPEIDTD", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEIDTD", iDB2DbType.iDB2Numeric, pCAPEIDTD);
            }
            if (pCAPEDEST == null)
            {
                DB2.AsignarParamProcAlmac("CAPEDEST", iDB2DbType.iDB2VarChar, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("CAPEDEST", iDB2DbType.iDB2VarChar, pCAPEDEST);
            }

            DB2.AsignarParamSalidaProcAlmac("POCAPEIDCO", iDB2DbType.iDB2Decimal, 0);
            DB2.EjecutarProcedimientoAlmacenado();

            return Convert.ToDecimal(DB2.ObtieneParametro("POCAPEIDCO"));
        }

        public decimal PEDEPE_INSERT(
            decimal? pDEPEIDCP = null,
            string pDEPECOAR = null,
            string pDEPEPART = null,
            string pDEPECONT = null,
            decimal? pDEPEALMA = null,
            decimal? pDEPECASO = null,
            decimal? pDEPEPESO = null,
            decimal? pDEPECAAT = null,
            decimal? pDEPEPEAT = null,
            decimal? pDEPEPERE = null,
            //decimal? pDEPETADE = null,
            //decimal? pDEPEPEBR = null,
            //decimal? pDEPESTOC = null,
            decimal? pDEPEESTA = null,
            decimal? pDEPEDISP = null,
            string pDEPEDSAR = null,
            //decimal? pDEPENUBU = null,
            string pDEPEUSCR = null,
            DateTime? pDEPEFECR = null,
            //string pDEPEUSMO = null,
            //DateTime? pDEPEFEMO = null,
            decimal? pDEPESERS = null,
            //decimal? pDEPESECR = null,
            decimal? pDEPESECU = null)
        {
            DB2.CrearComando("PRPEDAT.USP_PE_PEDEPE_INSERT", CommandType.StoredProcedure);
            if (pDEPEIDCP == null)
            {
                DB2.AsignarParamProcAlmac("DEPEIDCP", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEIDCP", iDB2DbType.iDB2Numeric, pDEPEIDCP);
            }
            if (pDEPECOAR == null)
            {
                DB2.AsignarParamProcAlmac("DEPECOAR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPECOAR", iDB2DbType.iDB2Char, pDEPECOAR);
            }
            if (pDEPEPART == null)
            {
                DB2.AsignarParamProcAlmac("DEPEPART", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEPART", iDB2DbType.iDB2Char, pDEPEPART);
            }
            if (pDEPECONT == null)
            {
                DB2.AsignarParamProcAlmac("DEPECONT", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPECONT", iDB2DbType.iDB2Char, pDEPECONT);
            }
            if (pDEPEALMA == null)
            {
                DB2.AsignarParamProcAlmac("DEPEALMA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEALMA", iDB2DbType.iDB2Numeric, pDEPEALMA);
            }
            if (pDEPECASO == null)
            {
                DB2.AsignarParamProcAlmac("DEPECASO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPECASO", iDB2DbType.iDB2Numeric, pDEPECASO);
            }
            if (pDEPEPESO == null)
            {
                DB2.AsignarParamProcAlmac("DEPEPESO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEPESO", iDB2DbType.iDB2Numeric, pDEPEPESO);
            }
            if (pDEPECAAT == null)
            {
                DB2.AsignarParamProcAlmac("DEPECAAT", iDB2DbType.iDB2Numeric, 0);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPECAAT", iDB2DbType.iDB2Numeric, pDEPECAAT);
            }
            if (pDEPEPEAT == null)
            {
                DB2.AsignarParamProcAlmac("DEPEPEAT", iDB2DbType.iDB2Numeric, 0);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEPEAT", iDB2DbType.iDB2Numeric, pDEPEPEAT);
            }
            if (pDEPEPERE == null)
            {
                DB2.AsignarParamProcAlmac("DEPEPERE", iDB2DbType.iDB2Numeric, 0);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEPERE", iDB2DbType.iDB2Numeric, pDEPEPERE);
            }
            //if (pDEPETADE == null)
            //{
            //    DB2.AsignarParamProcAlmac("DEPETADE", iDB2DbType.iDB2Numeric, 0);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("DEPETADE", iDB2DbType.iDB2Numeric, pDEPETADE);
            //}
            //if (pDEPEPEBR == null)
            //{
            //    DB2.AsignarParamProcAlmac("DEPEPEBR", iDB2DbType.iDB2Numeric, 0);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("DEPEPEBR", iDB2DbType.iDB2Numeric, pDEPEPEBR);
            //}
            //if (pDEPESTOC == null)
            //{
            //    DB2.AsignarParamProcAlmac("DEPESTOC", iDB2DbType.iDB2Numeric, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("DEPESTOC", iDB2DbType.iDB2Numeric, pDEPESTOC);
            //}
            if (pDEPEESTA == null)
            {
                DB2.AsignarParamProcAlmac("DEPEESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEESTA", iDB2DbType.iDB2Numeric, pDEPEESTA);
            }
            if (pDEPEDISP == null)
            {
                DB2.AsignarParamProcAlmac("DEPEDISP", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEDISP", iDB2DbType.iDB2Numeric, pDEPEDISP);
            }
            if (pDEPEDSAR == null)
            {
                DB2.AsignarParamProcAlmac("DEPEDSAR", iDB2DbType.iDB2VarChar, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEDSAR", iDB2DbType.iDB2VarChar, pDEPEDSAR);
            }
            //if (pDEPENUBU == null)
            //{
            //    DB2.AsignarParamProcAlmac("DEPENUBU", iDB2DbType.iDB2Numeric, 0);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("DEPENUBU", iDB2DbType.iDB2Numeric, pDEPENUBU);
            //}
            if (pDEPEUSCR == null)
            {
                DB2.AsignarParamProcAlmac("DEPEUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEUSCR", iDB2DbType.iDB2Char, pDEPEUSCR);
            }
            if (pDEPEFECR == null)
            {
                DB2.AsignarParamProcAlmac("DEPEFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPEFECR", iDB2DbType.iDB2TimeStamp, pDEPEFECR);
            }
            //if (pDEPEUSMO == null)
            //{
            //    DB2.AsignarParamProcAlmac("DEPEUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("DEPEUSMO", iDB2DbType.iDB2Char, pDEPEUSMO);
            //}
            //if (pDEPEFEMO == null)
            //{
            //    DB2.AsignarParamProcAlmac("DEPEFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("DEPEFEMO", iDB2DbType.iDB2TimeStamp, pDEPEFEMO);
            //}
            if (pDEPESERS == null)
            {
                DB2.AsignarParamProcAlmac("DEPESERS", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPESERS", iDB2DbType.iDB2Numeric, pDEPESERS);
            }
            //if (pDEPESECR == null)
            //{
            //    DB2.AsignarParamProcAlmac("DEPESECR", iDB2DbType.iDB2Numeric, 0);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("DEPESECR", iDB2DbType.iDB2Numeric, pDEPESECR);
            //}
            if (pDEPESECU == null)
            {
                DB2.AsignarParamProcAlmac("DEPESECU", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("DEPESECU", iDB2DbType.iDB2Numeric, pDEPESECU);
            }

            DB2.AsignarParamSalidaProcAlmac("PODEPEIDDP", iDB2DbType.iDB2Decimal, 0);
            DB2.EjecutarProcedimientoAlmacenado();

            return Convert.ToDecimal(DB2.ObtieneParametro("PODEPEIDDP"));
        }

        public Decimal PECAPE_NUME(string pserie)
        {
            Decimal colu = 0;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_PECAPE_SERIE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAPESERI", iDB2DbType.iDB2Char, pserie);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PECAPE>(cabeceraDataTable);

            if (list.Count > 0)
            {
                colu = list[0].CAPENUME;
            }
            return colu;
        }

        public void PEKABO_INSERT(
            decimal pKABOIDBO,
            decimal? pKABOIDTM = null,
            DateTime? pKABOFECH = null,//date
            decimal? pKABOALMA = null,
            decimal? pKABOCANT = null,
            decimal? pKABOPESO = null,
            decimal? pKABOIDDP = null,
            decimal? pKABOIDDO = null,
            string pKABOPART = null,
            string pKABOITEM = null,
            decimal? pKABOTARA = null,
            decimal? pKABOPEBR = null,
            string pKABOUSCR = null,
            DateTime? pKABOFECR = null//timestamp
            )
        {
            DB2.CrearComando("PRPEDAT.USP_PE_INSERTAR_PEKABO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PKABOIDBO", iDB2DbType.iDB2Numeric, pKABOIDBO);
            if (pKABOIDTM == null)
            {
                DB2.AsignarParamProcAlmac("PKABOIDTM", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOIDTM", iDB2DbType.iDB2Numeric, pKABOIDTM);
            }
            if (pKABOFECH == null)
            {
                DB2.AsignarParamProcAlmac("PKABOFECH", iDB2DbType.iDB2Date, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOFECH", iDB2DbType.iDB2Date, pKABOFECH);
            }
            if (pKABOALMA == null)
            {
                DB2.AsignarParamProcAlmac("PKABOALMA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOALMA", iDB2DbType.iDB2Numeric, pKABOALMA);
            }
            if (pKABOCANT == null)
            {
                DB2.AsignarParamProcAlmac("PKABOCANT", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOCANT", iDB2DbType.iDB2Numeric, pKABOCANT);
            }
            if (pKABOPESO == null)
            {
                DB2.AsignarParamProcAlmac("PKABOPESO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOPESO", iDB2DbType.iDB2Numeric, pKABOPESO);
            }
            if (pKABOIDDP == null)
            {
                DB2.AsignarParamProcAlmac("PKABOIDDP", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOIDDP", iDB2DbType.iDB2Numeric, pKABOIDDP);
            }
            if (pKABOIDDO == null)
            {
                DB2.AsignarParamProcAlmac("PKABOIDDO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOIDDO", iDB2DbType.iDB2Numeric, pKABOIDDO);
            }
            if (pKABOPART == null)
            {
                DB2.AsignarParamProcAlmac("PKABOPART", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOPART", iDB2DbType.iDB2Char, pKABOPART);
            }
            if (pKABOITEM == null)
            {
                DB2.AsignarParamProcAlmac("PKABOITEM", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOITEM", iDB2DbType.iDB2Char, pKABOITEM);
            }
            if (pKABOTARA == null)
            {
                DB2.AsignarParamProcAlmac("PKABOTARA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOTARA", iDB2DbType.iDB2Numeric, pKABOTARA);
            }
            if (pKABOPEBR == null)
            {
                DB2.AsignarParamProcAlmac("PKABOPEBR", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOPEBR", iDB2DbType.iDB2Numeric, pKABOPEBR);
            }
            if (pKABOUSCR == null)
            {
                DB2.AsignarParamProcAlmac("PKABOUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOUSCR", iDB2DbType.iDB2Char, pKABOUSCR);
            }
            if (pKABOFECR == null)
            {
                DB2.AsignarParamProcAlmac("PKABOFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PKABOFECR", iDB2DbType.iDB2TimeStamp, pKABOFECR);
            }

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public appWcfService.PEDEOS PEDEOS_Find(decimal? DEOSIDDO)//NOSE USA SOLO PARA OSAS
        {
            appWcfService.PEDEOS deos = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EW_PEDEOS_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEOSIDDO", iDB2DbType.iDB2Numeric, DEOSIDDO);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PEDEOS>(cabeceraDataTable);
            if (list.Count > 0)
            {
                deos = list[0];
            }
            return deos;
        }

        public appWcfService.PEBODP PEBODP_Find(decimal BODPIDDE)
        {
            appWcfService.PEBODP bodp = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EW_PEBODP_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBODPIDDE", iDB2DbType.iDB2Numeric, BODPIDDE);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PEBODP>(cabeceraDataTable);
            if (list.Count > 0)
            {
                bodp = list[0];
            }
            return bodp;
        }

        public appWcfService.PEBOLS PEBOLS_Find_IDBO(decimal? BOLSIDBO)
        {
            appWcfService.PEBOLS bols = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PE_PEBOLS_FIND_IDBO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBOLSIDBO", iDB2DbType.iDB2Numeric, BOLSIDBO);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PEBOLS>(cabeceraDataTable);
            if (list.Count > 0)
            {
                bols = list[0];
            }
            return bols;
        }

        public List<appWcfService.PEBODP> PEBODP_Find_IDDP(decimal? BODPIDDP)
        {
            List<appWcfService.PEBODP> bodp = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PE_PEBODP_IDDP", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBODPIDDP", iDB2DbType.iDB2Numeric, BODPIDDP);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            bodp = Util.ParseDataTable<appWcfService.PEBODP>(cabeceraDataTable);
            return bodp;
        }

        public List<appWcfService.PEBODP> PEBODP_Find_IDDO(decimal? BODPIDDO)//SOLO PARA OSAS
        {
            List<appWcfService.PEBODP> bodp = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PE_PEBODP_IDDO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBODPIDDO", iDB2DbType.iDB2Numeric, BODPIDDO);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            bodp = Util.ParseDataTable<appWcfService.PEBODP>(cabeceraDataTable);
            return bodp;
        }

        public appWcfService.PROSAS PROSAS_Find(decimal? OSASCIA, string OSASFOLI, decimal? OSASSECU)//SOLO SE USA EN OSAS
        {
            appWcfService.PROSAS prosa = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EW_PROSAS_FIRSTORDEFAULT", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("POSASCIA", iDB2DbType.iDB2Numeric, OSASCIA);
            DB2.AsignarParamProcAlmac("POSASFOLI", iDB2DbType.iDB2Char, OSASFOLI);
            DB2.AsignarParamProcAlmac("POSASSECU", iDB2DbType.iDB2Numeric, OSASSECU);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PROSAS>(cabeceraDataTable);
            if (list.Count > 0)
            {
                prosa = list[0];
            }
            return prosa;
        }

        public List<appWcfService.PEBODP> PEBODP_Find_IDBO_ALMA_PART_COAR(decimal? BODPIDBO, decimal? BODPALMA, string BODPPART, string BODPCOAR)
        {
            List<appWcfService.PEBODP> bodp = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PE_PEBODP_WHERE_IDBO_ALMA_PART_ARTI", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBODPIDBO", iDB2DbType.iDB2Numeric, BODPIDBO);
            DB2.AsignarParamProcAlmac("PBODPALMA", iDB2DbType.iDB2Numeric, BODPALMA);
            DB2.AsignarParamProcAlmac("PBODPPART", iDB2DbType.iDB2Char, BODPPART);
            DB2.AsignarParamProcAlmac("PBODPCOAR", iDB2DbType.iDB2Char, BODPCOAR);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            bodp = Util.ParseDataTable<appWcfService.PEBODP>(cabeceraDataTable);
            return bodp;
        }

        public appWcfService.GMDEEM GMDEEM_Find_DEEMARTI(decimal? DEEMCIA, string DEEMCOEM, string DEEMPART, string DEEMARTI, string DEEMTIPE)
        {
            appWcfService.GMDEEM deem = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_GMDEEM_FIND_DEEMARTI", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEEMCIA", iDB2DbType.iDB2Numeric, DEEMCIA);
            DB2.AsignarParamProcAlmac("PDEEMCOEM", iDB2DbType.iDB2Char, DEEMCOEM);
            DB2.AsignarParamProcAlmac("PDEEMPART", iDB2DbType.iDB2Char, DEEMPART);
            DB2.AsignarParamProcAlmac("PDEEMARTI", iDB2DbType.iDB2Char, DEEMARTI);
            DB2.AsignarParamProcAlmac("PDEEMTIPE", iDB2DbType.iDB2Char, DEEMTIPE);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.GMDEEM>(cabeceraDataTable);
            if (list.Count > 0)
            {
                deem = list[0];
            }
            return deem;
        }

        public appWcfService.GMDEEM GMDEEM_Find_DEEMESBO(decimal? DEEMCIA, string DEEMCOEM, string DEEMTIPE, decimal? DEEMESBO)
        {
            appWcfService.GMDEEM deem = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_GMDEEM_FIND_DEEMESBO", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PDEEMCIA", iDB2DbType.iDB2Numeric, DEEMCIA);
            DB2.AsignarParamProcAlmac("PDEEMCOEM", iDB2DbType.iDB2Char, DEEMCOEM);
            DB2.AsignarParamProcAlmac("PDEEMTIPE", iDB2DbType.iDB2Char, DEEMTIPE);
            DB2.AsignarParamProcAlmac("PDEEMESBO", iDB2DbType.iDB2Numeric, DEEMESBO);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.GMDEEM>(cabeceraDataTable);
            if (list.Count > 0)
            {
                deem = list[0];
            }
            return deem;
        }

        public void PEBODP_DELETE(decimal? BODPIDDE)///BODPIDBO , BODPIDDP , BODPIDDE , BODPIDDO
        {
            DB2.CrearComando("PRPEDAT.USP_EP_PEBODP_DELETE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBODPIDDE", iDB2DbType.iDB2Numeric, BODPIDDE);
            DB2.EjecutarProcedimientoAlmacenado();
        }


        public void PEBODP_UPDATE(
            decimal PBODPIDDE,
            decimal? PBODPIDBO = null,
            decimal? PBODPIDDP = null,
            decimal? PBODPALMA = null,
            string PBODPPART = null,
            string PBODPCOAR = null,
            decimal? PBODPCANT = null,
            decimal? PBODPPESO = null,
            decimal? PBODPPERE = null,
            decimal? PBODPDIFE = null,
            decimal? PBODPIDDO = null,
            decimal? PBODPSTCE = null,
            decimal? PBODPINBO = null,
            decimal? PBODPTADE = null,
            decimal? PBODPPEBR = null,
            decimal? PBODPESTA = null,
            string PBODPUSMO = null,
            DateTime? PBODPFEMO = null,//timestamp
            decimal? PBODPSECR = null,
            decimal? PBODPTAUN = null,
            string PBODPAPOR = null
            )
        {
            DB2.CrearComando("PRPEDAT.USP_PEBODP_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBODPIDDE", iDB2DbType.iDB2Numeric, PBODPIDDE);
            if (PBODPIDBO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPIDBO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPIDBO", iDB2DbType.iDB2Numeric, PBODPIDBO);
            }
            if (PBODPIDDP == null)
            {
                DB2.AsignarParamProcAlmac("PBODPIDDP", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPIDDP", iDB2DbType.iDB2Numeric, PBODPIDDP);
            }
            if (PBODPALMA == null)
            {
                DB2.AsignarParamProcAlmac("PBODPALMA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPALMA", iDB2DbType.iDB2Numeric, PBODPALMA);
            }
            if (PBODPPART == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPART", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPART", iDB2DbType.iDB2Char, PBODPPART);
            }
            if (PBODPCOAR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPCOAR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPCOAR", iDB2DbType.iDB2Char, PBODPCOAR);
            }
            if (PBODPCANT == null)
            {
                DB2.AsignarParamProcAlmac("PBODPCANT", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPCANT", iDB2DbType.iDB2Numeric, PBODPCANT);
            }
            if (PBODPPESO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPESO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPESO", iDB2DbType.iDB2Numeric, PBODPPESO);
            }
            if (PBODPPERE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPERE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPERE", iDB2DbType.iDB2Numeric, PBODPPERE);
            }
            if (PBODPDIFE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPDIFE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPDIFE", iDB2DbType.iDB2Numeric, PBODPDIFE);
            }
            if (PBODPIDDO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPIDDO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPIDDO", iDB2DbType.iDB2Numeric, PBODPIDDO);
            }
            if (PBODPSTCE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPSTCE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPSTCE", iDB2DbType.iDB2Numeric, PBODPSTCE);
            }
            if (PBODPINBO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPINBO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPINBO", iDB2DbType.iDB2Numeric, PBODPINBO);
            }
            if (PBODPTADE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPTADE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPTADE", iDB2DbType.iDB2Numeric, PBODPTADE);
            }
            if (PBODPPEBR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPEBR", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPEBR", iDB2DbType.iDB2Numeric, PBODPPEBR);
            }
            if (PBODPESTA == null)
            {
                DB2.AsignarParamProcAlmac("PBODPESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPESTA", iDB2DbType.iDB2Numeric, PBODPESTA);
            }
            if (PBODPUSMO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPUSMO", iDB2DbType.iDB2Char, PBODPUSMO);
            }
            if (PBODPFEMO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPFEMO", iDB2DbType.iDB2TimeStamp, PBODPFEMO);
            }
            if (PBODPSECR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPSECR", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPSECR", iDB2DbType.iDB2Numeric, PBODPSECR);
            }
            if (PBODPTAUN == null)
            {
                DB2.AsignarParamProcAlmac("PBODPTAUN", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPTAUN", iDB2DbType.iDB2Numeric, PBODPTAUN);
            }
            if (PBODPAPOR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPAPOR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPAPOR", iDB2DbType.iDB2Char, PBODPAPOR);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public void PEDEOS_UPDATE( // NO SE USA SOLO PARA OSAS

            decimal pDEOSIDDO,
            decimal? pDEOSIDCO = null,
            decimal? pDEOSSECU = null,
            string pDEOSFOLI = null,
            string pDEOSPART = null,
            string pDEOSCOAR = null,
            decimal? pDEOSALMA = null,
            decimal? pDEOSPESO = null,
            decimal? pDEOSPEAT = null,
            decimal? pDEOSCAAT = null,
            decimal? pDEOSPERE = null,
            decimal? pDEOSTADE = null,
            decimal? pDEOSPEBR = null,
            decimal? pDEOSSTOC = null,
            decimal? pDEOSESTA = null,
            decimal? pDEOSPEOR = null,
            decimal? pDEOSCAOR = null,
            decimal? pDEOSESPA = null,
            string pDEOSUSCR = null,
            DateTime? pDEOSFECR = null,
            string pDEOSUSMO = null,
            DateTime? pDEOSFEMO = null,
            decimal? pDEOSSECR = null)

        {

            DB2.CrearComando("PRPEDAT.USP_PEDEOS_UPDATE", CommandType.StoredProcedure);

            DB2.AsignarParamProcAlmac("PDEOSIDDO", iDB2DbType.iDB2Numeric, pDEOSIDDO);

            if (pDEOSIDCO == null) { DB2.AsignarParamProcAlmac("PDEOSIDCO", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSIDCO", iDB2DbType.iDB2Numeric, pDEOSIDCO); }

            if (pDEOSSECU == null) { DB2.AsignarParamProcAlmac("PDEOSSECU", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSSECU", iDB2DbType.iDB2Numeric, pDEOSSECU); }

            if (pDEOSFOLI == null) { DB2.AsignarParamProcAlmac("PDEOSFOLI", iDB2DbType.iDB2Char, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSFOLI", iDB2DbType.iDB2Char, pDEOSFOLI); }

            if (pDEOSPART == null) { DB2.AsignarParamProcAlmac("PDEOSPART", iDB2DbType.iDB2Char, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSPART", iDB2DbType.iDB2Char, pDEOSPART); }

            if (pDEOSCOAR == null) { DB2.AsignarParamProcAlmac("PDEOSCOAR", iDB2DbType.iDB2Char, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSCOAR", iDB2DbType.iDB2Char, pDEOSCOAR); }

            if (pDEOSALMA == null) { DB2.AsignarParamProcAlmac("PDEOSALMA", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSALMA", iDB2DbType.iDB2Numeric, pDEOSALMA); }

            if (pDEOSPESO == null) { DB2.AsignarParamProcAlmac("PDEOSPESO", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSPESO", iDB2DbType.iDB2Numeric, pDEOSPESO); }

            if (pDEOSPEAT == null) { DB2.AsignarParamProcAlmac("PDEOSPEAT", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSPEAT", iDB2DbType.iDB2Numeric, pDEOSPEAT); }

            if (pDEOSCAAT == null) { DB2.AsignarParamProcAlmac("PDEOSCAAT", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSCAAT", iDB2DbType.iDB2Numeric, pDEOSCAAT); }

            if (pDEOSPERE == null) { DB2.AsignarParamProcAlmac("PDEOSPERE", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSPERE", iDB2DbType.iDB2Numeric, pDEOSPERE); }

            if (pDEOSTADE == null) { DB2.AsignarParamProcAlmac("PDEOSTADE", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSTADE", iDB2DbType.iDB2Numeric, pDEOSTADE); }

            if (pDEOSPEBR == null) { DB2.AsignarParamProcAlmac("PDEOSPEBR", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSPEBR", iDB2DbType.iDB2Numeric, pDEOSPEBR); }

            if (pDEOSSTOC == null) { DB2.AsignarParamProcAlmac("PDEOSSTOC", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSSTOC", iDB2DbType.iDB2Numeric, pDEOSSTOC); }

            if (pDEOSESTA == null) { DB2.AsignarParamProcAlmac("PDEOSESTA", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSESTA", iDB2DbType.iDB2Numeric, pDEOSESTA); }

            if (pDEOSPEOR == null) { DB2.AsignarParamProcAlmac("PDEOSPEOR", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSPEOR", iDB2DbType.iDB2Numeric, pDEOSPEOR); }

            if (pDEOSCAOR == null) { DB2.AsignarParamProcAlmac("PDEOSCAOR", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSCAOR", iDB2DbType.iDB2Numeric, pDEOSCAOR); }

            if (pDEOSESPA == null) { DB2.AsignarParamProcAlmac("PDEOSESPA", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSESPA", iDB2DbType.iDB2Numeric, pDEOSESPA); }

            if (pDEOSUSCR == null) { DB2.AsignarParamProcAlmac("PDEOSUSCR", iDB2DbType.iDB2Char, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSUSCR", iDB2DbType.iDB2Char, pDEOSUSCR); }

            if (pDEOSFECR == null) { DB2.AsignarParamProcAlmac("PDEOSFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSFECR", iDB2DbType.iDB2TimeStamp, pDEOSFECR); }

            if (pDEOSUSMO == null) { DB2.AsignarParamProcAlmac("PDEOSUSMO", iDB2DbType.iDB2Char, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSUSMO", iDB2DbType.iDB2Char, pDEOSUSMO); }

            if (pDEOSFEMO == null) { DB2.AsignarParamProcAlmac("PDEOSFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSFEMO", iDB2DbType.iDB2TimeStamp, pDEOSFEMO); }

            if (pDEOSSECR == null) { DB2.AsignarParamProcAlmac("PDEOSSECR", iDB2DbType.iDB2Numeric, DBNull.Value); }

            else { DB2.AsignarParamProcAlmac("PDEOSSECR", iDB2DbType.iDB2Numeric, pDEOSSECR); }



            DB2.EjecutarProcedimientoAlmacenado();

        }

        public void PEBOLS_UPDATE(
            decimal PBOLSIDBO,
            //string PBOLSCOAR = null,
            //decimal? PBOLSIDTC = null,
            //string PBOLSCOEM = null,
            //string PBOLSCOCA = null,
            //decimal? PBOLSALMA = null,
            decimal? PBOLSESTA = null,
            //string PBOLSUSCR = null,
            //DateTime? PBOLSFECR = null,
            string PBOLSUSMO = null,
            DateTime? PBOLSFEMO = null//,
                                      //string PBOLSUSUB = null,
                                      //DateTime? PBOLSFEUB = null
            )
        {
            DB2.CrearComando("PRPEDAT.USP_PEBOLS_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBOLSIDBO", iDB2DbType.iDB2Numeric, PBOLSIDBO);

            DB2.AsignarParamProcAlmac("PBOLSESTA", iDB2DbType.iDB2Numeric, DBNull.Value);

            DB2.AsignarParamProcAlmac("PBOLSUSMO", iDB2DbType.iDB2Char, PBOLSUSMO);

            DB2.AsignarParamProcAlmac("PBOLSFEMO", iDB2DbType.iDB2TimeStamp, PBOLSFEMO);

            DB2.EjecutarProcedimientoAlmacenado();
        }

        public appWcfService.GMCAEM GMCAEM_Find_first(decimal? CAEMCIA, string CAEMCOEM)
        {
            appWcfService.GMCAEM caem = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EP_GMCAEM_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PCAEMCIA", iDB2DbType.iDB2Numeric, CAEMCIA);
            DB2.AsignarParamProcAlmac("PCAEMCOEM", iDB2DbType.iDB2Char, CAEMCOEM);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.GMCAEM>(cabeceraDataTable);
            if (list.Count > 0)
            {
                caem = list[0];
            }
            return caem;
        }

        public appWcfService.PEBOLS PEBOLS_Find_first(string PBOLSCOEM)
        {
            appWcfService.PEBOLS bols = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_EW_PEBOLS_FIRST", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PBOLSCOEM", iDB2DbType.iDB2Char, PBOLSCOEM);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PEBOLS>(cabeceraDataTable);
            if (list.Count > 0)
            {
                bols = list[0];
            }
            return bols;
        }

        public decimal PEBOLS_INSERT(
            string PBOLSCOAR = null,
            //decimal? PBOLSIDTC = null,
            string PBOLSCOEM = null,
            string PBOLSCOCA = null,
            decimal? PBOLSALMA = null,
            decimal? PBOLSESTA = null,
            string PBOLSUSCR = null,
            DateTime? PBOLSFECR = null//,
                                      //string PBOLSUSMO = null,
                                      //DateTime? PBOLSFEMO = null,
                                      //string PBOLSUSUB = null,
                                      //DateTime? PBOLSFEUB = null
            )
        {
            DB2.CrearComando("PRPEDAT.USP_PEBOLS_INSERT", CommandType.StoredProcedure);
            if (PBOLSCOAR == null)
            {
                DB2.AsignarParamProcAlmac("PBOLSCOAR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBOLSCOAR", iDB2DbType.iDB2Char, PBOLSCOAR);
            }
            //if (PBOLSIDTC == null)
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSIDTC", iDB2DbType.iDB2Numeric, 0);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSIDTC", iDB2DbType.iDB2Numeric, PBOLSIDTC);
            //}
            if (PBOLSCOEM == null)
            {
                DB2.AsignarParamProcAlmac("PBOLSCOEM", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBOLSCOEM", iDB2DbType.iDB2Char, PBOLSCOEM);
            }
            if (PBOLSCOCA == null)
            {
                DB2.AsignarParamProcAlmac("PBOLSCOCA", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBOLSCOCA", iDB2DbType.iDB2Char, PBOLSCOCA);
            }
            if (PBOLSALMA == null)
            {
                DB2.AsignarParamProcAlmac("PBOLSALMA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBOLSALMA", iDB2DbType.iDB2Numeric, PBOLSALMA);
            }
            if (PBOLSESTA == null)
            {
                DB2.AsignarParamProcAlmac("PBOLSESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBOLSESTA", iDB2DbType.iDB2Numeric, PBOLSESTA);
            }
            if (PBOLSUSCR == null)
            {
                DB2.AsignarParamProcAlmac("PBOLSUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBOLSUSCR", iDB2DbType.iDB2Char, PBOLSUSCR);
            }
            if (PBOLSFECR == null)
            {
                DB2.AsignarParamProcAlmac("PBOLSFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBOLSFECR", iDB2DbType.iDB2TimeStamp, PBOLSFECR);
            }
            //if (PBOLSUSMO == null)
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSUSMO", iDB2DbType.iDB2Char, PBOLSUSMO);
            //}
            //if (PBOLSFEMO == null)
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSFEMO", iDB2DbType.iDB2TimeStamp, PBOLSFEMO);
            //}
            //if (PBOLSUSUB == null)
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSUSUB", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSUSUB", iDB2DbType.iDB2Char, PBOLSUSUB);
            //}
            //if (PBOLSFEUB == null)
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSFEUB", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PBOLSFEUB", iDB2DbType.iDB2TimeStamp, PBOLSFEUB);
            //}

            DB2.AsignarParamSalidaProcAlmac("POBOLSIDBO", iDB2DbType.iDB2Decimal, 0);
            DB2.EjecutarProcedimientoAlmacenado();

            return Convert.ToDecimal(DB2.ObtieneParametro("POBOLSIDBO"));
        }

        public void PEBODP_INSERT(
            decimal? PBODPIDBO = null,
            decimal? PBODPIDDP = null,
            decimal? PBODPALMA = null,
            string PBODPPART = null,
            string PBODPCOAR = null,
            decimal? PBODPCANT = null,
            decimal? PBODPPESO = null,
            decimal? PBODPPERE = null,
            decimal? PBODPDIFE = null,
            decimal? PBODPIDDO = null,
            decimal? PBODPSTCE = null,
            decimal? PBODPINBO = null,
            decimal? PBODPTADE = null,
            decimal? PBODPPEBR = null,
            decimal? PBODPESTA = null,
            string PBODPUSCR = null,
            DateTime? PBODPFECR = null,//timestamp
                                       //string PBODPUSMO = null,
                                       //DateTime? PBODPFEMO = null,//timestamp
            decimal? PBODPSECR = null,
            decimal? PBODPTAUN = null,
            string PBODPAPOR = null
            )
        {
            DB2.CrearComando("PRPEDAT.USP_PEBODP_INSERT", CommandType.StoredProcedure);
            if (PBODPIDBO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPIDBO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPIDBO", iDB2DbType.iDB2Numeric, PBODPIDBO);
            }
            if (PBODPIDDP == null)
            {
                DB2.AsignarParamProcAlmac("PBODPIDDP", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPIDDP", iDB2DbType.iDB2Numeric, PBODPIDDP);
            }
            if (PBODPALMA == null)
            {
                DB2.AsignarParamProcAlmac("PBODPALMA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPALMA", iDB2DbType.iDB2Numeric, PBODPALMA);
            }
            if (PBODPPART == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPART", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPART", iDB2DbType.iDB2Char, PBODPPART);
            }
            if (PBODPCOAR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPCOAR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPCOAR", iDB2DbType.iDB2Char, PBODPCOAR);
            }
            if (PBODPCANT == null)
            {
                DB2.AsignarParamProcAlmac("PBODPCANT", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPCANT", iDB2DbType.iDB2Numeric, PBODPCANT);
            }
            if (PBODPPESO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPESO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPESO", iDB2DbType.iDB2Numeric, PBODPPESO);
            }
            if (PBODPPERE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPERE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPERE", iDB2DbType.iDB2Numeric, PBODPPERE);
            }
            if (PBODPDIFE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPDIFE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPDIFE", iDB2DbType.iDB2Numeric, PBODPDIFE);
            }
            if (PBODPIDDO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPIDDO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPIDDO", iDB2DbType.iDB2Numeric, PBODPIDDO);
            }
            if (PBODPSTCE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPSTCE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPSTCE", iDB2DbType.iDB2Numeric, PBODPSTCE);
            }
            if (PBODPINBO == null)
            {
                DB2.AsignarParamProcAlmac("PBODPINBO", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPINBO", iDB2DbType.iDB2Numeric, PBODPINBO);
            }
            if (PBODPTADE == null)
            {
                DB2.AsignarParamProcAlmac("PBODPTADE", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPTADE", iDB2DbType.iDB2Numeric, PBODPTADE);
            }
            if (PBODPPEBR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPPEBR", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPPEBR", iDB2DbType.iDB2Numeric, PBODPPEBR);
            }
            if (PBODPESTA == null)
            {
                DB2.AsignarParamProcAlmac("PBODPESTA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPESTA", iDB2DbType.iDB2Numeric, PBODPESTA);
            }
            if (PBODPUSCR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPUSCR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPUSCR", iDB2DbType.iDB2Char, PBODPUSCR);
            }
            if (PBODPFECR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPFECR", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPFECR", iDB2DbType.iDB2TimeStamp, PBODPFECR);
            }
            //if (PBODPUSMO == null)
            //{
            //    DB2.AsignarParamProcAlmac("PBODPUSMO", iDB2DbType.iDB2Char, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PBODPUSMO", iDB2DbType.iDB2Char, PBODPUSMO);
            //}
            //if (PBODPFEMO == null)
            //{
            //    DB2.AsignarParamProcAlmac("PBODPFEMO", iDB2DbType.iDB2TimeStamp, DBNull.Value);
            //}
            //else
            //{
            //    DB2.AsignarParamProcAlmac("PBODPFEMO", iDB2DbType.iDB2TimeStamp, PBODPFEMO);
            //}
            if (PBODPSECR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPSECR", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPSECR", iDB2DbType.iDB2Numeric, PBODPSECR);
            }
            if (PBODPTAUN == null)
            {
                DB2.AsignarParamProcAlmac("PBODPTAUN", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPTAUN", iDB2DbType.iDB2Numeric, PBODPTAUN);
            }
            if (PBODPAPOR == null)
            {
                DB2.AsignarParamProcAlmac("PBODPAPOR", iDB2DbType.iDB2Char, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PBODPAPOR", iDB2DbType.iDB2Char, PBODPAPOR);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public appWcfService.PEPARM Parametros_find(decimal PPARMIDPA)
        {
            appWcfService.PEPARM param = null;
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_PEPARM_FIND", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PPARMIDPA", iDB2DbType.iDB2Numeric, PPARMIDPA);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            var list = Util.ParseDataTable<appWcfService.PEPARM>(cabeceraDataTable);
            if (list.Count > 0)
            {
                param = list[0];
            }
            return param;
        }

        public void PEPARM_UPDATE(decimal PPARMIDPA,
        string PPARMVAPA)
        {
            DB2.CrearComando("PRPEDAT.USP_PEPARM_UPDATE", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("PPARMIDPA", iDB2DbType.iDB2Char, PPARMIDPA);
            if (PPARMVAPA == null)
            {
                DB2.AsignarParamProcAlmac("PPARMVAPA", iDB2DbType.iDB2Numeric, DBNull.Value);
            }
            else
            {
                DB2.AsignarParamProcAlmac("PPARMVAPA", iDB2DbType.iDB2Numeric, PPARMVAPA);
            }
            DB2.EjecutarProcedimientoAlmacenado();
        }

        public List<appWcfService.USP_OBTIENE_ARTICULOS_Result> ObtieneArticulos(string contrato, string partida, string articulo, string selec)
        {
            DataTable cabeceraDataTable = null;
            DB2.CrearComando("PRPEDAT.USP_OBTIENE_ARTICULOS", CommandType.StoredProcedure);
            DB2.AsignarParamProcAlmac("@PNUPE", iDB2DbType.iDB2Numeric, contrato);
            DB2.AsignarParamProcAlmac("@PNUPA", iDB2DbType.iDB2Numeric, partida);
            DB2.AsignarParamProcAlmac("@PITEM", iDB2DbType.iDB2Numeric, articulo);
            DB2.AsignarParamProcAlmac("@PPRST", iDB2DbType.iDB2Numeric, selec);
            cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
            return Util.ParseDataTable<appWcfService.USP_OBTIENE_ARTICULOS_Result>(cabeceraDataTable);
        }

        #endregion
    }
}
