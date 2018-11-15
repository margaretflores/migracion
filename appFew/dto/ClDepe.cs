using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appFew.dto
{
    /// <summary>
    /// Detalle Pedido
    /// </summary>
    public class ClDepe
    {
        /// <summary>
        /// Codigo cia
        /// </summary>
        public int DepeCia { get; set; }

        /// <summary>
        /// Nro de pedido
        /// </summary>
        public String DepeNrpe { get; set; }
        public string DEPETIPE { get; set; }

        /// <summary>
        /// Id item detalle pedido
        /// </summary>
        public int DepeItde { get; set; }

        /// <summary>
        /// Codigo de articulo
        /// </summary>
        public string DepeIdar { get; set; }

        /// <summary>
        /// Codigo lote
        /// </summary>
        public string DepeIdlo { get; set; }

        /// <summary>
        /// Identificador almacen
        /// </summary>
        public int DepeAlmd { get; set; }

        /// <summary>
        /// Cantidad en kilos
        /// </summary>
        public Decimal DepeCakg { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public string DepeUser { get; set; }

        /// <summary>
        /// Fecha creacion del registro
        /// </summary>
        public DateTime DepeFecr { get; set; }
    }
}