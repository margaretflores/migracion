using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appFew.dto
{
    /// <summary>
    /// Clase utilitaria para la cabecera del parte de ingreso
    /// </summary>
    public class CcPaca
    {
        /// <summary>
        /// Cod Companhia
        /// </summary>
        public int PacaCia { get; set; }
        // ID agencia
        public string PacaIdag { get; set; }
        /// <summary>
        /// Descripcion de la agencia a la cual pertenece el parte de ingreso, utilitario para la impresion del parte de ingreso
        /// </summary>
        public string EstaDesc { get; set; }
        // Numero de parte de ingreso
        public string PacaNrpi { get; set; }
        // ID Almacen
        public string PacaIdal { get; set; }
        // Id dueño de fibra
        public string PacaIddf { get; set; }

        // datos adicionales del duenho de fibra
        public CcDufi duenhoFibra { get; set; }

        // Fecha
        public DateTime PacaFech { get; set; }
        // Tipo Moneda
        public string PacaTpmo { get; set; }
        // Tipo Cambio
        public double PacaTpca { get; set; }
        // Tipo Material
        public string PacaTpma { get; set; }
        // TOTAL
        public double PacaTota { get; set; }
        // Estado
        public string PacaEsta { get; set; }
        // descripcion del estado
        public string EstadoDescripcion { get; set; }
        // Procedencia
        public string PacaProc { get; set; }

        // lista de items del parte de ingreso
        public List<CcPade> listaItems { get; set; }

        // Campo de auditoria usuario
        public string PacaUser { get; set; }
        // Campo de auditoria fecha 
        public DateTime PacaFecr { get; set; }
        // Campo de auditoria Hora
        public DateTime PacaHocr { get; set; }

    }
}