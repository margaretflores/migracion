using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace appWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IappService
    {

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "ejecutaServicio", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //RESOPE ejecutaServicio(PAROPE paramOperacion);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "validarUsuario?usuario={usuario}&clave={clave}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string validarUsuario(string usuario, string clave);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "validarUsuario2?usuario={usuario}&clave={clave}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RESOPE validarUsuario2(string usuario, string clave);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "mostrarPedidosApp", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //RESOPE mostrarPedidosApp();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarPedidosApp", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<PECAPE> mostrarPedidosApp();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarDetallePedidosApp?idpedido={idpedido}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<PEDEPE> mostrarDetallePedidosApp(decimal idpedido);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarDetallePedidos?idpedido={idpedido}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETALLE_PEDIDOS_Result> mostrarDetallePedidos(decimal idpedido);

        //[WebInvoke(Method = "POST", UriTemplate = "cambiaEstadoPedido?idpedido={idpedido}&idestado={idestado}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "cambiaEstadoPedido", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE cambiaEstadoPedido(PEESPE estadopedido);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "cambiaEstadoPedido2", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE cambiaEstadoPedido2(PEESPE estadopedido);
        //
        //mostrar_det_pedido
        //preparar_pedido
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarUbicacionesArticulo?articulo={articulo}&partida={partida}&idalmacen={idalmacen}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<PEUBIC> mostrarUbicacionesArticulo(string articulo, string partida, decimal idalmacen);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneBolsa?iddetalle={iddetalle}&empaque={empaque}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_BOLSA_Result> obtieneBolsa(decimal iddetalle, string empaque);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "guardaPreparacionBolsa", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE guardaPreparacionBolsa(PEBODP detallebolsa);

        //cambio 26/04
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "guardaPreparacionBolsa2", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE guardaPreparacionBolsa2(DTO_PEBODP paramOperacion);


        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneDetallePreparacionPedidos?iddetallepedido={iddetallepedido}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result> obtieneDetallePreparacionPedidos(decimal iddetallepedido);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "remueveBolsaPedido?idbolsapedido={idbolsapedido}&usuario={usuario}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RESOPE remueveBolsaPedido(decimal idbolsapedido, string usuario);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneDatosPartida?articulo={articulo}&partida={partida}&idalmacen={idalmacen}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RESOPE obtieneDatosPartida(string articulo, string partida, decimal idalmacen);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneTiposFolio", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<PETIFO> obtieneTiposFolio();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneFoliosUsuario?usuario={usuario}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_FOLIO_USUARIO_Result> obtieneFoliosUsuario(string usuario);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarPedidosInternos?tipofolios={tipofolios}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_OSAS_PENDIENTES_Result> mostrarPedidosInternos(string tipofolios);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarPedidosInternosPartida?tipofolios={tipofolios}&partida={partida}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_OSAS_PENDIENTES_Result> mostrarPedidosInternosPartida(string tipofolios, string partida);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarDetallePedidosInternos?folio={folio}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETALLE_OSA_Result> mostrarDetallePedidosInternos(string folio);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneDetallePreparacionPedInt?iddetallepedint={iddetallepedint}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result> obtieneDetallePreparacionPedInt(decimal iddetallepedint);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "cambiaEstadoPedInt", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE cambiaEstadoPedInt(PECAOS estadopedint);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "actualizaPreparacionItemOSA", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE actualizaPreparacionItemOSA(PEDEOS estadopedint);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneBolsaOsa?iddetalle={iddetalle}&empaque={empaque}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_BOLSA_OSA_Result> obtieneBolsaOsa(decimal iddetalle, string empaque);

        //ubicaciones
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneBolsaUbicacion?empaque={empaque}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_BOLSA_UBICACION_Result> obtieneBolsaUbicacion(string empaque);

        //consulta ubicaciones
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "consultaEmpaquesPartida?partida={partida}&articulo={articulo}&empaque={empaque}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_CONSULTA_EMPAQUES_PARTIDA_Result> consultaEmpaquesPartida(string partida, string articulo, string empaque);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "guardaBolsaUbicacion", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE guardaBolsaUbicacion(PEBOLS bolsa);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneDetalleBolsa?empaque={empaque}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETALLE_BOLSA_Result> obtieneDetalleBolsa(string empaque);

        //recepción
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarPedidosIntPendRecepcion?tipofolios={tipofolios}&folio={numfolio}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_OSAS_PENDIENTES_PLANTA_Result> mostrarPedidosIntPendRecepcion(string tipofolios, string numfolio);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "mostrarDetallePedidosIntPendRecepcion?folio={folio}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETALLE_OSA_PLANTA_Result> mostrarDetallePedidosIntPendRecepcion(string folio);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "conformidadRecepcionOsa", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE conformidadRecepcionOsa(appWcfService.DET_USP_OBTIENE_DETALLE_OSA_PLANTA_Result Listadetalles);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "cambiaestaDeosBodp", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE cambiaestaDeosBodp(DTO_USP_OBTIENE_DETALLE_OSA_Result paramOperacion);

        #region SIN EMPAQUE
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "guardaPreparacionBolsase", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE guardaPreparacionBolsase(PEBODP detallebolsa);

        //cambio 26/04
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "guardaPreparacionBolsa2se", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RESOPE guardaPreparacionBolsa2se(DTO_PEBODP paramOperacion);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneDetallePreparacionPedidosse?iddetallepedido={iddetallepedido}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> obtieneDetallePreparacionPedidosse(decimal iddetallepedido);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "remueveBolsaPedidose?idbolsapedido={idbolsapedido}&usuario={usuario}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RESOPE remueveBolsaPedidose(decimal idbolsapedido, string usuario);

        //DMA 17_10_2018
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "remueveBolsaPedidose2?idbolsapedido={idbolsapedido}&usuario={usuario}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RESOPE remueveBolsaPedidose2(string idbolsapedido, string usuario);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneDetallePreparacionPedIntse?iddetallepedint={iddetallepedint}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> obtieneDetallePreparacionPedIntse(decimal iddetallepedint);
        
        //DMA 17/10/2018
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "obtieneDetallePreparacionPedIntse2?iddetallepedint={iddetallepedint}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> obtieneDetallePreparacionPedIntse2(string iddetallepedint);

        #endregion
    }

}
