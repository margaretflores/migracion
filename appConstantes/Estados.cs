using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appConstantes
{
    public class Estados
    {
        public static class EstadoLiquidacion
        {
            public readonly static string NUEVO = "N";
            public readonly static string GENERADO = "G";
            public readonly static string EMITIDO = "E";
            public readonly static string DESCARGADO = "D";
            public readonly static string ANULADO = "B";
            public readonly static string CONTABILIZADO = "C";

            public readonly static string DESCRIPCION_GENERADO = "Liquidaciones Generadas";
            public readonly static string DESCRIPCION_EMITIDO = "Liquidaciones Emitidas";
            public readonly static string DESCRIPCION_DESCARGADO = "Liquidaciones Ingresadas a Almacén";
            public readonly static string DESCRIPCION_CONTABILIZADO = "Liquidaciones Contabilizadas";
            public readonly static string DESCRIPCION_NUEVO = "Nuevas Liquidaciones";
        }

        public static class EstadoGuia
        {
            public readonly static string GENERADO = "G";
            public readonly static string EMITIDO = "E";
            public readonly static string DESCARGADO = "D";
            public readonly static string ANULADO = "B";

            public readonly static string DESCRIPCION_GENERADO = "Guía Generada";
            public readonly static string DESCRIPCION_EMITIDO = "Guía Emitida";
            public readonly static string DESCRIPCION_DESCARGADO = "Guía Descargada";
            public readonly static string DESCRIPCION_ANULADO = "Guía Anulada";
            public readonly static string DESCRIPCION_NUEVO = "Nueva Guía";
        }

        public static class EstadoMovimiento
        {
            public readonly static string GENERADO = "G";
            public readonly static string EMITIDO = "E";
            public readonly static string DESCARGADO = "D";
            public readonly static string ANULADO = "B";

            public readonly static string DESCRIPCION_GENERADO = "Documento Generado";
            public readonly static string DESCRIPCION_EMITIDO = "Documento Emitido";
            public readonly static string DESCRIPCION_DESCARGADO = "Documento Descargado";
            public readonly static string DESCRIPCION_ANULADO = "Documento Anulado";
            public readonly static string DESCRIPCION_NUEVO = "Nuevo Documento";
        }

        public static class EstadoRegistroPartido
        {
            public readonly static string GENERADO = "G";
            public readonly static string EMITIDO = "E";
            public readonly static string DESCARGADO = "D";
            public readonly static string ANULADO = "B";

            public readonly static string DESCRIPCION_GENERADO = "Categorizado de Lote Generado";
            public readonly static string DESCRIPCION_EMITIDO = "Categorizado de Lote Emitido";
            public readonly static string DESCRIPCION_DESCARGADO = "Categorizado de Lote Descargado";
            public readonly static string DESCRIPCION_ANULADO = "Categorizado de Lote Anulado";
            public readonly static string DESCRIPCION_NUEVO = "Nuevo Categorizado de Lote";
        }

        public static class EstadoParte
        {
            public readonly static string GENERADO = "G";
            public readonly static string EMITIDO = "E";
            public readonly static string LIQUIDADO = "L";
            public readonly static string DESCARGADO = "D";
            public readonly static string CONTABILIZADO = "C";
            public readonly static string ANULADO = "A";

            public readonly static string DESCRIPCION_GENERADO = "Parte Generado";
            public readonly static string DESCRIPCION_EMITIDO = "Parte Emitido";
            public readonly static string DESCRIPCION_LIQUIDADO = "Parte Liquidado";
            public readonly static string DESCRIPCION_DESCARGADO = "Parte Descargado";
            public readonly static string DESCRIPCION_ANULADO = "Parte Anulado";
            public readonly static string DESCRIPCION_NUEVO = "Nueva Parte";




        }

        public static class EstadoPedido
        {
            public readonly static string SOLICITADO = "1";
            public readonly static string ENTREGADO = "2";

            public readonly static string DESCRIPCION_SOLICITADO = "Pedido Solicitado";
            public readonly static string DESCRIPCION_ENTREGADO = "Pedido con Orden de Salida";

        }

        public static class EstadoOSA
        {
            public readonly static string EMITIDO = "E";
            public readonly static string ATENDIDO = "T";
        }


    }
}
