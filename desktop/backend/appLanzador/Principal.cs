using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;

using appWcfService;
using appConstantes;
using System.IO;

namespace appLanzador
{
    public class Principal
    {

        public Principal()
        {

        }

      
        public RESOPE EjecutaOperacion(PAROPE paramOperacion)
        {
            //appLogica.MKT _spn;
            appLogica.Principal _principal;
            //
            appLogica.appDB2 _appDB2;
            _appDB2 = null;

            RESOPE vpar;

            //_spn = null;
            vpar = new RESOPE();
            vpar.ESTOPE = false;
            try
            {
                switch (paramOperacion.CODOPE)
                {
                    case CodigoOperacion.VALIDA_USUARIO:
                        _principal = new appLogica.Principal();
                        vpar = _principal.ValidaUsuario(paramOperacion);
                        break;
                    case CodigoOperacion.CAMBIAR_PASSWORD:
                        _principal = new appLogica.Principal();
                        vpar = _principal.CambiaClave(paramOperacion);
                        break;
                    case CodigoOperacion.BUSCA_CLIENTE:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.BuscaCliente(paramOperacion);
                        break;
                    case CodigoOperacion.BUSCA_ARTICULO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.BuscaArticulo(paramOperacion);
                        break;
                    //
                    case CodigoOperacion.CARGA_SERIE_CLIENTE:
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.obtieneSeriesUsuario(paramOperacion);
                        break;
                    case CodigoOperacion.GUARDA_PEDIDO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.GuardaPedido(paramOperacion);
                        break;
                    case CodigoOperacion.MUESTRA_PEDIDOS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraPedidos(paramOperacion);
                        break;
                    //case CodigoOperacion.BUSCA_PEDIDOS:
                        //_spn = new appLogica.MKT();
                        //vpar = _spn.BuscaPedidos(paramOperacion);
                      //  break;
                    case CodigoOperacion.MUESTRA_PEDIDOS_ALMACEN:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraPedidosAlmacen(paramOperacion);
                        break;
                    //case CodigoOperacion.MUESTRA_PEDIDOS_PENDIENTES:
                        //_spn = new appLogica.MKT();
                        //vpar = _spn.MuestraPedidosPendientes(paramOperacion);
                        //break;
                    case CodigoOperacion.MUESTRA_DETALLE_PEDIDO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraDetallePedidos(paramOperacion);
                        break;
                    case CodigoOperacion.BUSCA_PEDIDOS_FECHAS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.BuscaPedidosFechas(paramOperacion);
                        break;
                    //case CodigoOperacion.ANULA_PEDIDO:
                        //_spn = new appLogica.MKT();
                        //vpar = _spn.AnulaPedidos(paramOperacion);
                      //  break;
                    case CodigoOperacion.BUSCA_PEDIDOS_ALMACEN:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.BuscaPedidosAlmacen(paramOperacion);
                        break;
                    //case CodigoOperacion.CAMBIA_ESTADO_PEDIDO:
                        //_spn = new appLogica.MKT();
                        //vpar = _spn.CambiaEstadoPedido(paramOperacion);
                      //  break;
                    case CodigoOperacion.BUSCA_SOLICITUD:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.Buscasolicitud(paramOperacion);
                        break;
                    case CodigoOperacion.CAMBIA_ESTADO_LISTA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.CambiaEstaList(paramOperacion);
                        break;
                    //case CodigoOperacion.GUARDA_DETALLE:
                        //_spn = new appLogica.MKT();
                        //vpar = _spn.GuardaDetalle(paramOperacion);
                      //  break;
                    case CodigoOperacion.ELIMINA_DETALLE:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.EliminaDetalle(paramOperacion);
                        break;
                    case CodigoOperacion.MUESTRA_UBICACIONES_ARTICULO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraUbicacionesArticulo(paramOperacion);
                        break;
                    case CodigoOperacion.OBTIENE_BOLSA:
                       // _spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ObtieneBolsa(paramOperacion);
                        break;
                    case CodigoOperacion.OBTIENE_DETALLE_PREPRACION:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ObtieneDetallePreparacionse(paramOperacion);
                        break;
                    case CodigoOperacion.MUESTRA_PASILLOS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraPasillos(paramOperacion);
                        break;
                    case CodigoOperacion.MUESTRA_NIVELES:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraNiveles(paramOperacion);
                        break;
                    case CodigoOperacion.OBTIENE_PEDIDO_CONSULTA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ObtienePedidoConsulta(paramOperacion);
                        break;
                    case CodigoOperacion.GUARDA_PREPARACION_BOLSA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.guardaPreparacionBolsase(paramOperacion);
                        break;
                    case CodigoOperacion.REMUEVE_BOLSA_PEDIDO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.remueveBolsaPedidose(paramOperacion);
                        break;
                    case CodigoOperacion.REPORTE_PARTIDA_ALMACEN:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ReportePartidaAlmacen(paramOperacion);
                        break;
                    case CodigoOperacion.OBTIENE_DATOS_PARTIDA:
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.obtieneDatosPartida(paramOperacion);
                        break;
                    case CodigoOperacion.GENERA_PREGUIA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.GeneraPreguia(paramOperacion);
                        break;
                    case CodigoOperacion.REPORTE_MOVIMIENTOS_PARTIDA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ReporteMovimientosPartida(paramOperacion);
                        break;
                    case CodigoOperacion.GUARDA_PRIORIDAD:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.GuardaPrioridad(paramOperacion);
                        break;
                    case CodigoOperacion.MUESTRA_COLUMNAS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraColumnas(paramOperacion);
                        break;
                    case CodigoOperacion.MUESTRA_CASILLEROS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.MuestraCasilleros(paramOperacion);
                        break;
                    case CodigoOperacion.AGREGA_PASILLO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.AgregaPasi(paramOperacion);
                        break;
                    case CodigoOperacion.MODIFICA_CASILLERO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ModificaCasillero(paramOperacion);
                        break;
                    case CodigoOperacion.AGREGA_CASILLERO:
                       // _spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.AgregaCasillero(paramOperacion);
                        break;
                    case CodigoOperacion.AGREGA_NIVEL:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.AgregaNivel(paramOperacion);
                        break;
                    case CodigoOperacion.AGREGA_COLUMNA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.AgregaColumna(paramOperacion);
                        break;
                    case CodigoOperacion.DESHABILITA_CASILLERO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.DeshabilitaCasillero(paramOperacion);
                        break;
                    case CodigoOperacion.DEVUELVE_CASILLEROS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.DevuelveCasilleros(paramOperacion);
                        break;
                    case CodigoOperacion.ELIMINA_NIVEL:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.EliminaNivel(paramOperacion);
                        break;
                    case CodigoOperacion.ELIMINA_COLUMNA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.EliminaColumna(paramOperacion);
                        break;
                    case CodigoOperacion.DEVUELVE_NIVE_COLU:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.DevuelveNiveyColu(paramOperacion);
                        break;
                    case CodigoOperacion.ELIMINA_PASILLO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.EliminaPasillo(paramOperacion);
                        break;
                    case CodigoOperacion.DESHABILITA_COLUMNA:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.DeshabilitaColumna(paramOperacion);
                        break;
                    case CodigoOperacion.DESHABILITA_NIVEL:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.DeshabilitaNivel(paramOperacion);
                        break;
                    case CodigoOperacion.DESHABILITA_PASILLO:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.DeshabilitaGeneral(paramOperacion);
                        break;
                    case CodigoOperacion.OBTIENE_PEDIDO_TRACKING:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ObtienePedidoTracking(paramOperacion);
                        break;
                    case CodigoOperacion.OBTIENE_PARAMETROS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ObtieneParametros(paramOperacion);
                        break;
                    case CodigoOperacion.ACTUALIZA_NOTIFICACIONES:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ActualizaNotificaciones(paramOperacion);
                        break;
                    case CodigoOperacion.VALIDA_ITEMS_EXCEL:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ValidaitemExcel(paramOperacion);
                        break;
                    case CodigoOperacion.REPORTE_MOVIMIENTOS_FECHAS:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ReporteMovimientosFechas(paramOperacion);
                        break;
                    case CodigoOperacion.VALIDA_PREPARACION:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ValidaPreparacion(paramOperacion);
                        break;
                    case CodigoOperacion.VALIDA_PREPARACION_LIST:
                        //_spn = new appLogica.MKT();
                        _appDB2 = new appLogica.appDB2();
                        vpar = _appDB2.ValidaPreparacionList(paramOperacion);
                        break;
                    //case CodigoOperacion.OBTENER_PEDIDOS:
                    //    _spn = new appLogica.MKT();
                    //    vpar = _spn.ObtienePedidos(paramOperacion);
                    //    break;                 
                    default:
                        vpar.MENERR = "OPERACION NO DEFINIDA";
                        break;
                }


            }
            catch (Exception ex)
            {
                vpar.MENERR = ex.Message;
                Util.EscribeLog(ex.Message);
                //throw ex;
            }
            finally
            {
                //if (_spn != null)
                //{
                //    //_spn.Finaliza();
                //    _spn = null;
                //}
                if (_appDB2 != null)
                {
                    _appDB2.Finaliza();
                    _appDB2 = null;
                }
            }

            return vpar;

        }

    }
}
