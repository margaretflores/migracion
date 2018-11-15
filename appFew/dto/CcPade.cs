using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appFew.dto
{
    /// <summary>
    /// Clase utilitaria para el detalle del parte de ingreso
    /// </summary>
    public class CcPade
    {
        /// <summary>
        /// Cod Companhia
        /// </summary>
        public int PadeCia { get; set; }
        /// <summary>
        /// ID Detalle 1,2,3...,1,2,3..  
        /// </summary>
        public int PadeIdde { get; set; }
        /// <summary>
        /// ID Agencia 
        /// </summary>
        public string PadeIdag { get; set; }
        /// <summary>
        /// Numero de parte de ingreso 
        /// </summary>
        public string PadeNrpi { get; set; }
        /// <summary>
        /// ID articulo 
        /// </summary>
        public string PadeIdar { get; set; }
        /// <summary>
        /// Campo auxiliar para mostrar descripcion de articulo
        /// </summary>
        public string DescripcionArticulo { get; set; }
        /// <summary>
        /// Campo auxiliar para mostrar descripcion de tipo material
        /// </summary>
        public string TipoMaterial { get; set; }
        /// <summary>
        /// ID lote 
        /// </summary>
        public string PadeIdlo { get; set; }
        /// <summary>
        /// Nro sacos 
        /// </summary>
        public int PadeNrsa { get; set; }
        /// <summary>
        /// Peso Bruto 
        /// </summary>
        public double PadePebr { get; set; }
        /// <summary>
        /// Peso Tara
        /// </summary>
        public double PadePeta { get; set; }
        /// <summary>
        /// Peso Neto 
        /// </summary>
        public double PadePene { get; set; }
        /// <summary>
        /// Peso Neto Quintales 
        /// </summary>
        public double PadeQqne { get; set; }
        /// <summary>
        /// Precio unitario quintales 
        /// </summary>
        public double PadeQqpu { get; set; }
        /// <summary>
        /// Precio Unitario
        /// </summary>
        public double PadePrun { get; set; }
        // Precio Total
        public double PadePrto { get; set; }
    }
}