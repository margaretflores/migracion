using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appFew.dto
{
    public class ProveedorDetalle
    {
        /// <summary>
        /// Codigo proveedor
        /// </summary>
        public string ProCve { get; set; }

        /// <summary>
        /// Apellidos
        /// </summary>
        public string ProApellidos { get; set; }

        /// <summary>
        /// Nombres
        /// </summary>
        public string ProNom { get; set; }

        /// <summary>
        /// Nro Documento
        /// </summary>
        public string NroDocumento { get; set; }

        /// <summary>
        /// Apellidos nombres
        /// </summary>
        public string ApellidosNombres { get; set; }

        /// <summary>
        /// Direccion
        /// </summary>
        public string ProDir { get; set; }
        /// <summary>
        /// Departamento
        /// </summary>
        public string ProDpt { get; set; }
        /// <summary>
        /// Provincia
        /// </summary>
        public string ProPro { get; set; }
        /// <summary>
        /// Distrito
        /// </summary>
        public string ProDis { get; set; }
        /// <summary>
        /// Codigo Proveedor
        /// </summary>
        public string DccaCopr { get; set; }
        /// <summary>
        /// Conteo de facturas emitidas
        /// </summary>
        public int ConteoFacturas { get; set; }
        public decimal Enero { get; set; }
        public decimal Febrero { get; set; }
        public decimal Marzo { get; set; }
        public decimal Abril { get; set; }
        public decimal Mayo { get; set; }
        public decimal Junio { get; set; }
        public decimal Julio { get; set; }
        public decimal Agosto { get; set; }
        public decimal Septiembre { get; set; }
        public decimal Octubre { get; set; }
        public decimal Noviembre { get; set; }
        public decimal Diciembre { get; set; }
        /// <summary>
        /// Total de monto acumuludado
        /// </summary>
        public decimal Total { get; set; }

    }
}
