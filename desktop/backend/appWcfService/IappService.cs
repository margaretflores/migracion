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

        [OperationContract]
        RESOPE EjecutaOperacion(PAROPE paramOperacion);

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "validarUsuario?usuario={usuario}&clave={clave}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string validarUsuario(string usuario, string clave);

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "mostrarPedidosApp", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //List<PECAPE> mostrarPedidosApp();

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "mostrarDetallePedidosApp?idpedido={idpedido}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //List<PEDEPE> mostrarDetallePedidosApp(decimal idpedido);

        ////[WebInvoke(Method = "POST", UriTemplate = "cambiaEstadoPedido?idpedido={idpedido}&idestado={idestado}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "cambiaEstadoPedido", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare )]
        //RESOPE cambiaEstadoPedido(PEESPE estadopedido);

        ////
        ////mostrar_det_pedido
        ////preparar_pedido
        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "mostrarUbicacionesArticulo?articulo={articulo}&partida={partida}&idalmacen={idalmacen}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //List<PEUBIC> mostrarUbicacionesArticulo(string articulo, string partida, decimal idalmacen);

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "obtieneBolsa?iddetalle={iddetalle}&empaque={empaque}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //List<USP_OBTIENE_BOLSA_Result> obtieneBolsa(decimal iddetalle, string empaque);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "guardaPreparacionBolsa", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //RESOPE guardaPreparacionBolsa(PEBODP detallebolsa);

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "obtieneDetallePreparacionPedidos?iddetallepedido={iddetallepedido}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result> obtieneDetallePreparacionPedidos(decimal iddetallepedido);

        //[OperationContract]
        //[WebInvoke(Method = "DELETE", UriTemplate = "remueveBolsaPedido?idbolsapedido={idbolsapedido}&usuario={usuario}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //RESOPE remueveBolsaPedido(decimal idbolsapedido, string usuario);

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "obtieneDatosPartida?articulo={articulo}&partida={partida}&idalmacen={idalmacen}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //RESOPE obtieneDatosPartida(string articulo, string partida, decimal idalmacen);

    }

}
