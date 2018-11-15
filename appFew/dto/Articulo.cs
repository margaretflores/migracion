using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appFew.dto
{
    public class Articulo
    {
        public string codArticulo { get; set; }
        public string descripcion { get; set; }

        /// <summary>
        /// variable que contiene el ultimo lote asociado a este articulo dependiendo de la agencia donde se consulte
        /// </summary>
        public string loteId { get; set; }
    }
}