using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appConstantes
{
    public class CodigoOperacion
    {
        //public const string VALIDA_USUARIO = "COM001";
        #region Usuarios
        public const string VALIDA_USUARIO = "US001";
        public const string CAMBIAR_PASSWORD = "US002";
        #endregion
        
        public const string BUSCA_CLIENTE = "PED001";

     

        //CLIENTES
        public const string BUSCAR_CLIENTE = "PED004";
        //PASILLOS
        public const string CARGAR_PASILLO = "PED005";
        public const string INSERTAR_PASILLO = "PED006";
        public const string BUSCA_ARTICULO = "PED007";
        public const string CARGA_SERIE_CLIENTE = "PED008";

        //PEDIDOS
        public const string GUARDA_PEDIDO = "PED009";
        public const string MUESTRA_PEDIDOS = "PED010";
        public const string BUSCA_PEDIDOS = "PED011";
        public const string MUESTRA_PEDIDOS_ALMACEN = "PED012";
        public const string MUESTRA_PEDIDOS_PENDIENTES = "PED013";
        public const string MUESTRA_DETALLE_PEDIDO = "PED014";
        public const string ANULA_PEDIDO = "PED015";
        public const string BUSCA_PEDIDOS_FECHAS = "PED016";
        public const string BUSCA_PEDIDOS_ALMACEN = "PED017";
        public const string CAMBIA_ESTADO_PEDIDO = "PED018";
        public const string BUSCA_SOLICITUD = "PED019";
        public const string CAMBIA_ESTADO_LISTA = "PED020";
        public const string GUARDA_DETALLE = "PED021";
        public const string ELIMINA_DETALLE = "PED022";
        public const string MUESTRA_UBICACIONES_ARTICULO = "PED023";
        public const string OBTIENE_BOLSA = "PED024";
        public const string OBTIENE_DETALLE_PREPRACION = "PED025";

        public const string GUARDA_PREPARACION_BOLSA = "PED026";
        public const string INSERTA_MOVIMIENTO_KARDEX = "PED027";
        public const string REMUEVE_BOLSA_PEDIDO = "PED028";

        public const string MUESTRA_PASILLOS = "PED029";

        public const string MUESTRA_NIVELES = "PED030";
        public const string OBTIENE_PEDIDO_CONSULTA = "PED031";
        public const string REPORTE_PARTIDA_ALMACEN = "PED032";

        public const string OBTIENE_DATOS_PARTIDA = "PED033";

        public const string GENERA_PREGUIA = "PED034";
        public const string REPORTE_MOVIMIENTOS_PARTIDA = "PED035";
        public const string GUARDA_PRIORIDAD = "PED036";
        public const string MUESTRA_COLUMNAS = "PED037";
        public const string MUESTRA_CASILLEROS = "PED038";
        public const string AGREGA_PASILLO = "PED039";
        public const string MODIFICA_CASILLERO = "PED040";
        public const string AGREGA_NIVEL = "PED041";
        public const string AGREGA_CASILLERO = "PED042";
        public const string AGREGA_COLUMNA = "PED043";
        public const string DESHABILITA_CASILLERO = "PED044";
        public const string DEVUELVE_CASILLEROS = "PED045";
        public const string ELIMINA_NIVEL = "PED046";
        public const string ELIMINA_COLUMNA = "PED047";
        public const string DEVUELVE_NIVE_COLU = "PED048";
        public const string ELIMINA_PASILLO = "PED049";
        public const string DESHABILITA_COLUMNA = "PED050";
        public const string DESHABILITA_NIVEL = "PED051";
        public const string DESHABILITA_PASILLO = "PED052";

        //WEB TRACKING
        public const string OBTIENE_PEDIDO_TRACKING = "PED053";

        //Parametros
        public const string OBTIENE_PARAMETROS = "PED054";
        public const string ACTUALIZA_NOTIFICACIONES = "PED055";
        public const string VALIDA_ITEMS_EXCEL = "PED056";
        public const string REPORTE_MOVIMIENTOS_FECHAS = "PED057";

        public const string VALIDA_PREPARACION = "PED058";
        public const string VALIDA_PREPARACION_LIST = "PED059";




    }

}
