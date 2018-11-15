using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appFew.dto
{
    /// <summary>
    /// Clase que contiene la informacion de la cabecera de un pedido para salida de almacen
    /// </summary>
    public class ClPedi
    {
        /// <summary>
        /// Codigo Cia
        /// </summary>
        public int PediCia { get; set; }

        /// <summary>
        /// Numero de pedido
        /// </summary>
        public string PediNrpe { get; set; }
        public string PEDITIPE { get; set; }

        /// <summary>
        /// Fecha de Pedido
        /// </summary>
        public DateTime PediFech { get; set; }

        /// <summary>
        /// Hora en la cual se emitio el pedido
        /// </summary>
        public DateTime PediHora { get; set; }

        /// <summary>
        /// Estado del pedido 1: solicitado, 2: entregado
        /// </summary>
        public string PediEsta { get; set; }

        /// <summary>
        /// Usuario que realizo la operacion de solicitud de pedido
        /// </summary>
        public string PediUser { get; set; }

        /// <summary>
        /// Fecha en la cual se creo el registro
        /// </summary>
        public DateTime PediFecr { get; set; }

        public decimal PEDIALMA { get; set; }
        public string PEDIFOLI { get; set; }

        public List<ClDepe> listaItems { get; set; }
    }
}